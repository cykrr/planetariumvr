using UnityEngine;

public class WorldModelController : MonoBehaviour
{
    public WiimoteReceiver receiver;
    
    
    void Start()
    {
        // Calibrate the sensor to find the zero-offset values for each axis
        // CalibrateSensor();
    }

    void Update()
    {

            Vector3 rotation = receiver.rotation;


            // Apply the rotation to the object
            transform.rotation = Quaternion.Euler(rotation.y, rotation.x, rotation.z);
    }


}
