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
    float[] bufX, bufY, bufZ;

    private void Awake()
    {
        bufX = new float[bufferSize];
        bufY = new float[bufferSize];
        bufZ = new float[bufferSize];
    }

    private void Update()
    {
        if (receiver == null)
            return;

        ShiftAdd(bufX, receiver.rotation.x);
        ShiftAdd(bufY, receiver.rotation.y);
        ShiftAdd(bufZ, receiver.rotation.z);


    }

    void ShiftAdd(float[] buf, float val)
    {
        for (int i = 0; i < buf.Length - 1; i++)
            buf[i] = buf[i + 1];
        buf[buf.Length - 1] = val;
    }

    private void OnGUI()
    {
        DrawGraph(new Rect(10, 50, bufferSize * timeScale, 400));
    }

    void DrawGraph(Rect rect)
    {
        GUI.BeginGroup(rect);
        GUI.Label(rect, $"{receiver.rotation.x} {receiver.rotation.y} {receiver.rotation.z}");
        DrawLine(bufX, Color.red, 100f);
        DrawLine(bufY, Color.green, 200f);
        DrawLine(bufZ, Color.blue, 300f);

        GUI.EndGroup();
    }

    void DrawLine(float[] buf, Color color, float verticalOffset)
    {
        Vector2 prev = new Vector2(0, verticalOffset - buf[0] * angleScale);

        Handles.color = color;
        for (int i = 1; i < buf.Length; i++)
        {
            Vector2 current = new Vector2(i * timeScale, verticalOffset - buf[i] * angleScale);
            Handles.DrawLine(prev, current);
            prev = current;
        }
    }
}
