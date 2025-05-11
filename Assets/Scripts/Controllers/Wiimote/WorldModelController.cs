using UnityEngine;

public class WorldModelController : MonoBehaviour
{
    void Start()
    {
        // Calibrate the sensor to find the zero-offset values for each axis
        // CalibrateSensor();
    }

    void Update()
    {
            transform.rotation = Quaternion.Euler(WiimoteReceiver.instance.rotation);
    }

}
