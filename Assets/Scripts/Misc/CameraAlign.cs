using UnityEngine;

public class CameraAlign : MonoBehaviour
{
    void Start()
    {
        float offset = AppController.instance.currentCameraRotation.y;
        transform.rotation = Quaternion.Euler(0, offset, 0);
    }

}
