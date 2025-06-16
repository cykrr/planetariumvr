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
using System;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class OrbitDrawer : MonoBehaviour
{
    public Transform center;
    public float a = 1f;
    public float b = 1f;
    public int segments = 100;

    private LineRenderer lineRenderer;
    Color c1 = Color.gray;

    [System.Obsolete]
    private void Awake()
    {
        // LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.useWorldSpace = false;
        lineRenderer.loop = true;
        lineRenderer.positionCount = segments + 1;
        lineRenderer.startWidth = 0.007f;
        lineRenderer.endWidth = 0.007f;
        lineRenderer.startColor = c1;
        lineRenderer.endColor = c1;

        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.widthCurve = AnimationCurve.Constant(0f, a * 0.05f, a * 0.05f);
        lineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        // Anti-aliasing setup
        lineRenderer.textureMode = LineTextureMode.Tile;
        lineRenderer.material.EnableKeyword("_ALPHABLEND_ON");
        lineRenderer.material.renderQueue = 3000;
        lineRenderer.SetColors(c1, c1);
        GenerateEllipsePoints(lineRenderer);


    }

    private void GenerateEllipsePoints(LineRenderer line)
    {
        float angle = 0f;
        float angleStep = 2 * Mathf.PI / segments;

        for (int i = 0; i < segments + 1; i++)
        {
            line.SetPosition(i, new Vector3(
                Mathf.Cos(angle) * a,
                0,
                Mathf.Sin(angle) * b
            ));
            angle += angleStep;
        }

    }

    // Adds a lineRenderer to this transform and
    // makes the line renderer fade at the end.



    private void Start()
    {
        DrawOrbit();
    }

    private void Update()
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

