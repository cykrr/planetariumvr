using System.Linq;
using Mirror;
using UnityEngine;

public class PlanetOrbit : NetworkBehaviour
{
    [SyncVar]
    public bool isOrbiting = false;
    public GameObject orbitPathPrefab;

    private float angle = 0f;

    // Ellipse parameters
    public float a = 5f; // semi-major axis
    public float b = 3f; // semi-minor axis
    public float orbitSpeed = 0.5f;

    public float sunScale = 1f;

    private Vector3 ellipseCenter;
    private Vector3 initialOffset;
    private SharedObject sharedObject;
    float[] planetSizes = {
    0.3f, 0.552f, 0.556f, 0.513f, 1.5f, 1.337f, 0.835f, 0.824f
    };
    private string[] planetIndex = new string[] {
    "Mercurio",
    "Venus",
    "Tierra",
    "Marte",
    "Júpiter",
    "Saturno",
    "Urano",
    "Neptuno"
};

    void Start()
    {
        sharedObject = GetComponent<SharedObject>();
    }

    void Update()
    {
        if (isOrbiting && isServer)
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
            Transform sun = collision.gameObject.transform;

            if (isServer)
            {
                GameObject orbitPath = Instantiate(orbitPathPrefab);
                orbitPath.name = "OrbitPath_" + sharedObject.gameObject.name;
                OrbitDrawer drawer = orbitPath.GetComponent<OrbitDrawer>();
                drawer.center = sun;

                initialOffset = new Vector3(a, 0, 0); // Example: start at perihelion
                transform.position = sun.position + initialOffset;
                drawer.a = a;
                drawer.b = b;
                drawer.segments = 100; // Adjust as needed for smoothness

                NetworkServer.Spawn(orbitPath);
            }

            int index = planetIndex.Contains(transform.gameObject.name) ? 
                System.Array.IndexOf(planetIndex, transform.gameObject.name) : 0;

            transform.localScale = new Vector3(planetSizes[index] / sunScale , planetSizes[index] / sunScale , planetSizes[index] / sunScale);

            Debug.Log("PlanetOrbit: Entered trigger with sun");
            // Set the initial position to the sun's position;

            // Shrink to 50%
            // redObject.SetScale(transform.localScale * 0.5f);

            // Initialize orbit
            ellipseCenter = sun.position;
            initialOffset = transform.position - sun.position;

            // Optional: Set a and b dynamically based on current position

            isOrbiting = true;
        }
    }

    void OnDrawGizmos()
{
    Gizmos.color = Color.red;
    Gizmos.DrawSphere(ellipseCenter, 0.2f); // Where ellipse is centered
    Gizmos.color = Color.yellow;
    // Gizmos.DrawSphere(sun.position, 0.2f);   // Where the Sun (focus) is
}

}
