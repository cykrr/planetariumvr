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



            // Apply the rotation to the object
            transform.rotation = Quaternion.Euler(receiver.rotation);
    }


}
