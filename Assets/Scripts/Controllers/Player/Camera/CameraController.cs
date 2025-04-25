/*
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody; // assign your player object here
    float xRotation = 0f;
    float yRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // optional: hide and lock the cursor
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        yRotation += mouseX;
        // xRotation = Mathf.Clamp(xRotation, -90f, 90f); // avoid flipping

        // Apply vertical rotation to the camera
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);

        // Apply horizontal rotation to the player body (i.e., yaw)
        if (playerBody != null)
            playerBody.Rotate(Vector3.up * mouseX);
    }
}
*/

using UnityEngine;

public class GyroCameraController : MonoBehaviour
{
    private bool gyroEnabled;
    private Gyroscope gyro;
    private Quaternion rotFix;

    private bool calibrated = false;
    public int frameCount = 0;
    private const int calibrationFrames = 100;
    private Quaternion accumulatedRotation = Quaternion.identity;
    private Quaternion origin = Quaternion.identity;
    public bool IsGyroEnabled() => gyroEnabled;
    public bool IsCalibrated() => calibrated;
    public Vector3 GetRawGyroEuler() {
        if (gyro != null)
            return gyro.attitude.eulerAngles;
        else 
            return Vector3.one;
    }

    public Vector3 GetOrigin()
    {
        return origin.eulerAngles;
    }


    void Start()
    {
        gyroEnabled = EnableGyro();
    }

    bool EnableGyro()
    {
        if (SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            gyro.enabled = true;

            // Unity + gyro rotation fix (because phone has different coordinate space)
            rotFix = new Quaternion(0, 0, 1, 0);
            return true;
        }
        return false;
    }

    void Update()
    {
        if (!gyroEnabled) return;
        frameCount++;

        if (!calibrated)
        {
            // Accumulate rotation over first 10 frames
            accumulatedRotation = gyro.attitude;

            if (frameCount >= calibrationFrames)
            {
                Quaternion corrected = gyro.attitude * rotFix;
                origin = Quaternion.Inverse(corrected);  // Key change
                calibrated = true;
            }
        }
        else
        {
            // Apply relative rotation from origin
            Quaternion deviceRotation = gyro.attitude * rotFix;
            transform.localRotation = origin * deviceRotation;
        }
    }
}

