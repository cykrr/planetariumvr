using UnityEngine;
using UnityEngine.UI;

public class RaycastHighlight : MonoBehaviour
{
    public Camera mainCamera;
    private GameObject lastHitObject;
    private float rayDistance = 100f;

    void Update()
    {
        if (true || Input.GetMouseButtonDown(0)) // Always active, or use a button
        {
            Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

            if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
            {
                if (hit.collider.CompareTag("Planet"))
                {
                    // De-highlight previous object if it's different
                    if (lastHitObject != null && lastHitObject != hit.collider.gameObject)
                    {
                        lastHitObject.GetComponent<Renderer>().materials[1].SetFloat("_Scale", 0f);
                    }

                    lastHitObject = hit.collider.gameObject;
                    if (Input.GetMouseButton(0))
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
        }
    }

}
