using UnityEngine;

public class DragAndDropController : MonoBehaviour
{
    public RayPointer rayPointer;
    private Ray ray;
    private Vector3 offset;
    private float rayDistance = 10f;
    private Plane dragPlane;
    GameObject hitObject;
    SharedObject draggingObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ray = rayPointer.ray;
    }

    // Update is called once per frame
    void Update()
    {
        WiimoteReceiver receiver = WiimoteReceiver.instance;
        ray = rayPointer.ray;

        if (draggingObject != null && receiver.ButtonA()) {
            if (dragPlane.Raycast(ray, out float enter)) {
                Vector3 hitPoint = ray.GetPoint(enter);
                draggingObject.CmdMoveObject(hitPoint + offset);
            }
        }

        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance)){
            hitObject = hit.collider.gameObject;
            // print("hit " + hitObject.name);
            if (receiver.ButtonA() && hitObject.tag == "Interactable" && draggingObject == null) {
                draggingObject = hitObject.GetComponent<SharedObject>(); 
                dragPlane = new Plane(-transform.forward, hit.point);
                offset = draggingObject.transform.position - hit.point;
            }
        } 
        if (!receiver.ButtonA()) {
            draggingObject = null;
        }
    }
}
