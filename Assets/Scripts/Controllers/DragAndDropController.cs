using UnityEngine;

public class DragAndDropController : MonoBehaviour
{
    public RayPointer rayPointer;
    public WiimoteReceiver receiver;
    private Ray ray;
    private Vector3 offset;
    private float rayDistance = 10f;
    private Plane dragPlane;
    GameObject draggingObject, hitObject, lastHitObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ray = rayPointer.ray;

        
    }

    // Update is called once per frame
    void Update()
    {
        ray = rayPointer.ray;

        if (draggingObject != null && receiver.ButtonB()) {
            if (dragPlane.Raycast(ray, out float enter)) {
                Vector3 hitPoint = ray.GetPoint(enter);
                draggingObject.transform.position = hitPoint + offset;

            }
        }

        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance)){
            hitObject = hit.collider.gameObject;
            // print("hit " + hitObject.name);
            if (receiver.ButtonB()) {
                draggingObject = hitObject; 
                dragPlane = new Plane(-transform.forward, hit.point);
                offset = draggingObject.transform.position - hit.point;
            }
        } 
        if (!receiver.ButtonB()) {
            draggingObject = null;
        }
    }
}
