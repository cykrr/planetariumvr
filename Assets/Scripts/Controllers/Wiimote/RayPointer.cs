using UnityEngine;

public class RayPointer : MonoBehaviour
{
    Vector3 tipPosition;
    private LineRenderer lineRenderer;
    float rayDistance = 10f;

    public GameObject hitObject;
    public GameObject lastHitObject;
    public Ray ray;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        GameObject lineObj = new GameObject("RayLine");
        lineRenderer = lineObj.AddComponent<LineRenderer>();

        // Setup LineRenderer appearance
        lineRenderer.startWidth = 0.01f;
        lineRenderer.endWidth = .01f;
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

        ray = new Ray(tipPosition, gameObject.transform.forward);

        Vector3 tip = gameObject.transform.position +
         gameObject.transform.forward * (gameObject.transform.localScale.z / 2f);

        Vector3 end = tip + gameObject.transform.forward * 500f;


        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
        {
            if (hit.collider.CompareTag("Interactable"))
            {
                hitObject = hit.collider.gameObject;
                // De-highlight previous object if it's different
                if (lastHitObject != null && lastHitObject != hit.collider.gameObject)
                {
                    lastHitObject.GetComponent<Renderer>().materials[1].SetFloat("_Scale", 0f);
                }

                lastHitObject = hit.collider.gameObject;
                end = hit.point;
                if (WiimoteReceiver.instance.ButtonAClick())
                {
                    ButtonController clickable = lastHitObject.GetComponent<ButtonController>();
                    if (clickable != null)
                    {
                        clickable.callback();
                    }
                }

                Material glowMaterial = hit.collider.GetComponent<Renderer>().materials[1];
                glowMaterial.SetFloat("_Scale", 1.05f);
            }
            else
            {
                // Ray hit something but not a planet
                if (lastHitObject != null)
                {
                    lastHitObject.GetComponent<Renderer>().materials[1].SetFloat("_Scale", 0f);
                    lastHitObject = null;
                }
            }
        }
        else
        {
            // Ray hit nothing at all
            if (lastHitObject != null)
            {
                lastHitObject.GetComponent<Renderer>().materials[1].SetFloat("_Scale", 0f);
                lastHitObject = null;
            }
        }

        lineRenderer.SetPosition(0, tip);
        lineRenderer.SetPosition(1, end);
    }

}
