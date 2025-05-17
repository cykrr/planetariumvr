using UnityEngine;

public class PlanetOrbit : MonoBehaviour
{
    public Transform sun;  // Assign "Sol" in the Inspector or find it by name
    private bool isOrbiting = false;
    private float angle = 0f;

    // Ellipse parameters
    public float a = 5f; // semi-major axis
    public float b = 3f; // semi-minor axis
    public float orbitSpeed = 0.5f;
    private Vector3 ellipseCenter;
    private Vector3 initialOffset;
    private SharedObject sharedObject;

    void Start()
    {
        sharedObject = GetComponent<SharedObject>();
    }

    void Update()
    {
        if (isOrbiting && sun != null)
        {
            angle += orbitSpeed * Time.deltaTime;
            float x = a * Mathf.Cos(angle);
            float z = b * Mathf.Sin(angle);

            Vector3 newPos = new Vector3(x, 0, z) + ellipseCenter;
            sharedObject.position = newPos;
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name == "Modelo Sol" && !isOrbiting)
        {
            // Shrink to 50%
            sharedObject.SetScale(transform.localScale * 0.5f);

            // Initialize orbit
            ellipseCenter = sun.position;
            initialOffset = transform.position - sun.position;

            // Optional: Set a and b dynamically based on current position

            isOrbiting = true;
        }
    }
}
