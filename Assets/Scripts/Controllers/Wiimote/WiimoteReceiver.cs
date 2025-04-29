using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

// using System.Numerics;
using System.Text;
using System.Threading;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class WiimoteReceiver : MonoBehaviour
{
    public Vector3 rotation;
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
    public int calibrationFrameLimit = 100;

    private Vector3 _gyro = new Vector3(0, 0, 0);
    public Vector3 _accel = new Vector3(0, 0, 0);
    Vector3 ww = new Vector3(0, 0, 0);

    static Vector3 X1 = new Vector3(131f, 131f, 159f);
    static Vector3 X2 = new Vector3(131f, 157f, 133f);
    static Vector3 X3 = new Vector3(157.5f, 130.5f, 134f);

    static Vector3 X0 = new Vector3(
        X1.x + X2.x / 2f,
        X1.y + X3.y / 2f,
        X2.z + X3.z / 2f
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
        Vector3 w = (_gyro - gyroOffset) * gyroScaling;

        Vector3 accel = new Vector3(
            (_accel.x - X0.x) / (X3.x - X0.x),
            (_accel.y - X0.y) / (X2.y - X0.y),
            (_accel.z - X0.z) / (X1.z - X0.z)
        );

        // Pitch (around X), Roll (around Z) from accelerometer
        float pitch = Mathf.Atan2(accel.y, Mathf.Sqrt(accel.z * accel.z + accel.x * accel.x)) * Mathf.Rad2Deg;
        float roll = Mathf.Atan2(accel.z, accel.x) * Mathf.Rad2Deg;

        // 1. Integrate gyro (Euler angles in deg)
        Vector3 gyroDelta = w * Time.deltaTime; // Ensure w is in deg/sec
        Vector3 gyroAngle = rotation + gyroDelta;

        // 2. Complementary filter
        float HPF = 0.98f;
        float LPF = 0.02f;
        // Yaw no correction available!
        // rotation.x = gyroAngle.x;
        // Pitch and roll corrections
        rotation.y = pitch;
        rotation.z = roll;
    }

    // Button "A" for quick recalibrate
    if ((buttons & 2048) != 0)
    {
        rotation.x = 0;
        rotation.y = 0;
        rotation.z = 0;
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
