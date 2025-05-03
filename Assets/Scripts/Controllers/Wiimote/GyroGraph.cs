using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class AngleGrapher : MonoBehaviour
{
    [Tooltip("Number of data points to keep in the graph")]
    public int bufferSize = 200;
    [Tooltip("Horizontal spacing between samples (pixels per sample)")]
    public float timeScale = 2f;
    [Tooltip("Vertical scale for angles (pixels per degree)")]
    public float angleScale = 1f;

    public WiimoteReceiver receiver;
    float[] bufX, bufY, bufZ, accX, accY, accZ, bufPitch, bufRoll;
    float minX = Mathf.Infinity,
     maxX = -Mathf.Infinity,
     minY= Mathf.Infinity,
     maxY = -Mathf.Infinity,
     minZ= Mathf.Infinity,
     maxZ = -Mathf.Infinity;

    private void Awake()
    {
        bufX = new float[bufferSize];
        bufY = new float[bufferSize];
        bufZ = new float[bufferSize];

        accX = new float[bufferSize];
        accY = new float[bufferSize];
        accZ = new float[bufferSize];

        bufPitch = new float[bufferSize];
        bufRoll = new float[bufferSize];
    }

    private void Update()
    {
        if (receiver == null)
            return;

        if (minX > receiver.rotation.x) minX = receiver.rotation.x;
        if (minY > receiver.rotation.y) minY = receiver.rotation.y;
        if (minZ > receiver.rotation.z) minZ = receiver.rotation.z;

        if (maxX < receiver.rotation.x) maxX = receiver.rotation.x;
        if (maxY < receiver.rotation.y) maxY = receiver.rotation.y;
        if (maxZ < receiver.rotation.z) maxZ = receiver.rotation.z;

        ShiftAdd(bufX, receiver.rotation.x);
        ShiftAdd(bufY, receiver.rotation.y);
        ShiftAdd(bufZ, receiver.rotation.z);
        ShiftAdd(accX, receiver.accel.x*10000);
        ShiftAdd(accY, receiver.accel.y*10000);
        ShiftAdd(accZ, receiver.accel.z*10000);
        ShiftAdd(bufPitch, receiver.pitchRoll.x);
        ShiftAdd(bufRoll, receiver.pitchRoll.y);


    }

    void ShiftAdd(float[] buf, float val)
    {
        for (int i = 0; i < buf.Length - 1; i++)
            buf[i] = buf[i + 1];
        buf[buf.Length - 1] = val;
    }

    private void OnGUI()
    {
        DrawGraph(new Rect(10, 100, bufferSize * timeScale, 400));
        DrawAccelGraph(new Rect(510, 100, bufferSize * timeScale, 400));
        DrawAccelGraph(new Rect(510, 100, bufferSize * timeScale, 400));
        DrawYawPitchGraph(new Rect(710, 100, bufferSize * timeScale, 400));
    }

    void DrawGraph(Rect rect)
    {
        float dx = maxX - minX;
        float dy = maxY - minY;
        float dz = maxZ - minZ;
        GUI.BeginGroup(rect);
        GUI.Label(rect, $"{receiver.rotation.x:+0.00;-0.00; 0.00} {receiver.rotation.y:+0.00;-0.00; 0.00} {receiver.rotation.z:+0.00;-0.00; 0.00}");
        GUI.Label(new Rect(20, 40, 300, 20), $"{dx:+0.00;-0.00; 0.00} {dy:+0.00;-0.00; 0.00} {dz:+0.00;-0.00; 0.00}");
        DrawLine(bufX, Color.red, 100f);
        DrawLine(bufY, Color.green, 200f);
        DrawLine(bufZ, Color.blue, 300f);

        GUI.EndGroup();
    }

    void DrawYawPitchGraph(Rect rect)
    {
        GUI.BeginGroup(rect);
        GUI.Label(new Rect(20, 40, 300, 20), $"{receiver.pitchRoll.x:+0.00;-0.00; 0.00} {receiver.pitchRoll.y:+0.00;-0.00; 0.00}");
        DrawLine(bufPitch, Color.red, 100f);
        DrawLine(bufRoll, Color.green, 200f);

        GUI.EndGroup();
    }

    void DrawAccelGraph(Rect rect)
    {
        GUI.BeginGroup(rect);
        GUI.Label(new Rect(20, 40, 300, 20), $"{receiver.accel.x:+0.00;-0.00; 0.00} {receiver.accel.y:+0.00;-0.00; 0.00} {receiver.accel.z:+0.00;-0.00; 0.00}");
        DrawLine(accX, Color.red, 200f);
        DrawLine(accY, Color.green, 300f);
        DrawLine(accZ, Color.blue, 400f);

        GUI.EndGroup();
    }

    void DrawLine(float[] buf, Color color, float verticalOffset)
    {
        Vector2 prev = new Vector2(0, verticalOffset - buf[0] * angleScale);

        // Handles.color = color;
        for (int i = 1; i < buf.Length; i++)
        {
            Vector2 current = new Vector2(i * timeScale, verticalOffset - buf[i] * angleScale);
            // Handles.DrawLine(prev, current);
            prev = current;
        }
    }
}
