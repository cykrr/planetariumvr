using UnityEngine;

public class RayPointer : MonoBehaviour
{
    Vector3 tipPosition;
    public LineRenderer lineRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Ray ray = new Ray(tipPosition, gameObject.transform.forward);

        GameObject lineObj = new GameObject("RayLine");
        lineRenderer = lineObj.AddComponent<LineRenderer>();

        // Setup LineRenderer appearance
        lineRenderer.startWidth = 1f;
        lineRenderer.endWidth = 1f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
        lineRenderer.positionCount = 2;
        
    }

    // Update is called once per frame
    void Update()
    {
        tipPosition = gameObject.transform.position +
            gameObject.transform.forward *
            (gameObject.transform.localScale.z / 2f);
        Vector3 tip = gameObject.transform.position +
         gameObject.transform.forward * (gameObject.transform.localScale.z/2f);

        Vector3 end = tip + gameObject.transform.forward*500f;
        
        lineRenderer.SetPosition(0, tip);
        lineRenderer.SetPosition(1, end);
    }
}
