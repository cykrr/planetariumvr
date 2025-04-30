using System;
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

    ushort buttons;

    // Gyro calibration
    private const float HPF = 0.92f;
    private const float LPF = .02f;
    private const float gyroScaling = 1f / 13.768f;
    private const float accelScaling = 1f / 16384f;

    private Vector3 gyroOffset = new Vector3(0, 0, 0);
    // private Vector3 accelOffset = new Vector3(0, 0, 0);
    private int frameCount = 0;
    public int calibrationFrameLimit = 200;

    private Vector3 _gyro = new Vector3(0, 0, 0);
    private Vector3 _accel = new Vector3(0, 0, 0);
    public Vector3 accel = new Vector3(0, 0, 0);
    Vector3 ww = new Vector3(0, 0, 0);

    static Vector3 X1 = new Vector3(106f, 131f, 159f);
    static Vector3 X2 = new Vector3(106f, 157f, 133f);
    static Vector3 X3 = new Vector3(158f, 130.5f, 134f);

    static Vector3 X0 = new Vector3(
        132f, //while standing
        131f,
        159f
    );






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
        ww += w*Time.deltaTime;

        pitchRoll.x = Mathf.Atan2(accel.y, Mathf.Sqrt(accel.x*accel.x + accel.y*accel.y))*Mathf.Rad2Deg; // pitch
        pitchRoll.y = Mathf.Atan2(-accel.x, accel.z) * Mathf.Rad2Deg; // roll
        // 1. Integrate gyro (Euler angles in deg)

        // 2. Complementary filter
        float HPF = 0.98f;
        float LPF = 0.02f;
        // Yaw no correction available!
        rotation.x = HPF * ww.x + LPF * pitchRoll.x;
        rotation.y = -ww.z * 0.7f;
        rotation.z = HPF * ww.y * 0.7f + LPF * pitchRoll.y;
        // Pitch and roll corrections
        // rotation.y = pitch;
        // rotation.z = roll;
        // rotation = w;
    }

    // Button "A" for quick recalibrate
    if ((buttons & 2048) != 0)
    {
        rotation.x = 0;
        rotation.y = 0;
        rotation.z = 0;
        ww.x = 0; ww.y = 0; ww.z = 0;
    }
}    


    // Calibrate the sensor by finding the resting values for each axis
    private void CalibrateSensor()
    {
        gyroOffset += _gyro / 100f;
        // accelOffset += _accel / 100f;
        frameCount++;
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

}
