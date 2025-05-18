using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;


// using System.Numerics;
using System.Text;
using System.Threading;
using Unity.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class WiimoteReceiver : MonoBehaviour
{
    public Vector3 rotation;
    public Vector3 pitchRoll = new Vector3(0, 0, 0);
    public Vector3 rotationAngle = new Vector3(0, 0, 0);
    UdpClient client;
    Thread receiveThread;

    private ushort buttons;

    // Gyro calibration
    private const float HPF = 0.92f;
    private const float LPF = .02f;
    private const float gyroScaling = 1f / 13.768f;
    private const float accelScaling = 1f / 16384f;

    private Vector3 gyroOffset = new Vector3(0, 0, 0);
    // private Vector3 accelOffset = new Vector3(0, 0, 0);
    private int frameCount = 0;
    public int calibrationFrameLimit = 5000;

    private Vector3 _gyro = new Vector3(0, 0, 0);
    private Vector3 _accel = new Vector3(0, 0, 0);
    private Vector3 _accelFiltered = new Vector3(0, 0, 0);
    public Vector3 accel = new Vector3(0, 0, 0);

    static Vector3 X1 = new Vector3(106f, 131f, 159f);
    static Vector3 X2 = new Vector3(106f, 157f, 133f);
    static Vector3 X3 = new Vector3(158f, 130.5f, 134f);

    static Vector3 X0 = new Vector3(
        135f, //while standing
        137f,
        163f
    );

    /*
    static Vector3 X0 = new Vector3(
        132f, //while standing
        131f,
        159f
    );
    */

    private bool isPressedA = false;
    private bool cooldownA = false;
    private bool isPressedB = false;
    private bool cooldownB = false;
    private bool isPressedHome = false;
    private bool cooldownHome = false;


    public static WiimoteReceiver instance;
    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        client = new UdpClient(9050);
        receiveThread = new Thread(ReceiveData);
        receiveThread.IsBackground = true; // So it closes when app exits
        receiveThread.Start();
    }

    void Update()
    {
        if (frameCount < calibrationFrameLimit)
        {
            CalibrateSensor();
            rotation = Vector3.zero;
        }
        else
        {
            Vector3 w = _gyro - gyroOffset;
            w *= 595/8192f;
            // w.x -= 5.5f;
            // w.y -= 5.5f;
            // w.z -= 5.5f;

            accel.x = (_accel.x -X0.x) / 26f; //min + max /2
            // accel.x = (_accel.x - X0.x)/(X3.x - X0.x);
            // accel.y = _accel.y; 
            accel.y = (_accel.y - X0.y)/27f;
            accel.z = (_accel.z - X0.z)/27f + 1;//(_accel.z - X0.z)/(X1.z - X0.z);
            accel = accel.normalized;

            _accelFiltered = accel;//0.95f * _accelFiltered + 0.05f * accel;

            pitchRoll.x = Mathf.Atan2(_accelFiltered.y, Mathf.Sqrt(_accelFiltered.x*_accelFiltered.x + _accelFiltered.z*_accelFiltered.z) + 0.001f)*Mathf.Rad2Deg; // pitch
            pitchRoll.y = Mathf.Atan2(-_accelFiltered.x, _accelFiltered.z+ 0.0001f) * Mathf.Rad2Deg; // roll
            // 1. Integrate gyro (Euler angles in deg)

            // 2. Complementary filter
            // Yaw no correction available!
            float alpha = 0.98f;
            rotation.x = alpha * (rotation.x + w.x  * Time.deltaTime) + (1 - alpha) * pitchRoll.x;
            rotation.z = alpha * (rotation.z + w.y * Time.deltaTime) + (1 - alpha) * pitchRoll.y;
            rotation.y -= w.z * 0.7f*  Time.deltaTime; // Yaw uncorrecte
            // Pitch and roll corrections
            // rotation.y = pitch;
            // rotation.z = roll;
            // rotation = w;
        }


    }

    // Calibrate the sensor by finding the resting values for each axis
    private void CalibrateSensor()
    {
        gyroOffset += _gyro;
        // accelOffset += _accel / 100f;
        frameCount++;
        if (frameCount == calibrationFrameLimit)
            gyroOffset /= calibrationFrameLimit;
    }

    void OnApplicationQuit()
    {
        receiveThread.Abort();
        client.Close();
    }

    void ReceiveData()
    {
        while (true)
        {
            IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
            byte[] data = client.Receive(ref anyIP);
            if (data.Length >= 14)
            {
                // Using BitConverter to convert 2-byte chunks to ushort
                ushort phi = BitConverter.ToUInt16(data, 0);
                ushort theta = BitConverter.ToUInt16(data, 2);
                ushort psi = BitConverter.ToUInt16(data, 4);
                ushort x = BitConverter.ToUInt16(data, 6);
                ushort y = BitConverter.ToUInt16(data, 8);
                ushort z = BitConverter.ToUInt16(data, 10);
                buttons = BitConverter.ToUInt16(data, 12);

                // Scale here if you want, or later in Update()
                _gyro.x = phi; _gyro.y = theta; _gyro.z = psi;
                _accel.x = x; _accel.y = y; _accel.z = z;
            }
        }
    }

    public bool ButtonA() {
        return (buttons & 2048) != 0;
    }


    public bool ButtonB() {
        return (buttons & 1024) != 0;
    }

    public bool ButtonHome()
    {
        return (buttons & 32768) != 0;
    }

    public bool ButtonAClick()
    {
        if (ButtonA())
        {
            if (!cooldownA && !isPressedA)
            {
                cooldownA = true;
                isPressedA = true;
                StartCoroutine(ResetIsPressedA());
                return true;
            }
        } 
        else
        {
            isPressedA = false;
        }
        return false;
    }

    public bool ButtonBClick()
    {
        if (ButtonB())
        {
            if (!cooldownB && !isPressedB)
            {
                cooldownB = true;
                isPressedB = true;
                StartCoroutine(ResetIsPressedB());
                return true;
            }
        }
        else
        {
            isPressedB = false;
        }
        return false;
    }

    public bool ButtonHomeClick()
    {
        if (ButtonHome())
        {
            if (!cooldownHome && !isPressedHome)
            {
                cooldownHome = true;
                isPressedHome = true;
                StartCoroutine(ResetIsPressedHome());
                return true;
            }
        }
        else
        {
            isPressedHome = false;
        }
        return false;
    }

    IEnumerator ResetIsPressedA()
    {
        yield return new WaitForSeconds(0.5f);
        cooldownA = false;
    }

    IEnumerator ResetIsPressedB()
    {
        yield return new WaitForSeconds(0.5f);
        cooldownB = false;
    }

    IEnumerator ResetIsPressedHome()
    {
        yield return new WaitForSeconds(0.5f);
        cooldownHome = false;
    }
}

