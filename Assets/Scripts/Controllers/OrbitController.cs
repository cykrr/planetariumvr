// using UnityEngine;
//
// [RequireComponent(typeof(LineRenderer))]
// public class OrbitDrawer : MonoBehaviour
// {
//     public Transform center;
//     public float radius = 5f;
//     public int segments = 100;
//     private LineRenderer lineRenderer;
//
//     void Start()
//     {
//         lineRenderer = GetComponent<LineRenderer>();
//         lineRenderer.useWorldSpace = false;
//         lineRenderer.loop = true;
//         lineRenderer.positionCount = segments;
//
//         DrawOrbit();
//     }
//
//     void DrawOrbit()
//     {
//         Vector3[] points = new Vector3[segments];
//         float angleStep = 360f / segments;
//
//         for (int i = 0; i < segments; i++)
//         {
//             float angle = Mathf.Deg2Rad * angleStep * i;
//             float x = Mathf.Cos(angle) * radius;
//             float z = Mathf.Sin(angle) * radius;
//             points[i] = new Vector3(x, 0, z);
//         }
//
//         lineRenderer.SetPositions(points);
//     }
//
//     void OnValidate()
//     {
//         if (lineRenderer != null && Application.isPlaying)
//             DrawOrbit();
//     }
// }
//
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class OrbitDrawer : MonoBehaviour
{
    public Transform center;
    public float a = 1f;
    public float b = 1f;
    public int segments = 100;

    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        DrawOrbit();
    }

    private void DrawOrbit()
    {
        lineRenderer.positionCount = segments + 1;
        for (int i = 0; i <= segments; i++)
        {
            float angle = i * 2 * Mathf.PI / segments;
            float x = Mathf.Cos(angle) * a;
            float z = Mathf.Sin(angle) * b;
            Vector3 pos = new Vector3(x, 0f, z);
            lineRenderer.SetPosition(i, pos);
        }

        // Make the orbit centered around the sun
        transform.position = center ? center.position : Vector3.zero;
    }
}

