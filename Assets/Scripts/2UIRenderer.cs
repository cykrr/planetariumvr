using System;
using UnityEngine;

public class UIRenderer : MonoBehaviour
{
    public GyroCameraController gyroController; // Assign this in the Inspector

    void OnGUI()
    {
        if (gyroController == null) return;

        GUIStyle style = new GUIStyle();
        style.fontSize = 30;
        style.normal.textColor = Color.green;
        //Debug.Log("ASDASJNDAKSJNFJKNFAKS");
        GUILayout.BeginArea(new Rect(20, 20, Screen.width, Screen.height));
        GUILayout.Label("Gyro Enabled: " + gyroController.IsGyroEnabled(), style);
        GUILayout.Label("Calibrated: " + gyroController.IsCalibrated(), style);
        GUILayout.Label("Raw Gyro: " + gyroController.GetRawGyroEuler().ToString("F2"), style);
        GUILayout.Label("Origin: " + gyroController.GetOrigin(), style);
        // GUILayout.Label("Frame Count: " + gyroController.frameCount, style);
        GUILayout.Label("LocalRotation: " + gyroController.transform.rotation.eulerAngles, style);
        GUILayout.EndArea();
    }
}
