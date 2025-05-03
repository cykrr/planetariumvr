using UnityEngine;

public class DragAndDropSceneController : MonoBehaviour
{
    private GameObject cuerpoCelestePrefab;
    private Vector3 posicionSol = new Vector3(2, 0, 5);
    private Vector3 posicionPlanetas = new Vector3(-3, 2, 5);

    private string[] planetas = new string[] {
        "Mercurio",
        "Venus",
        "Tierra",
        "Marte",
        "Júpiter",
        "Saturno",
        "Urano",
        "Neptuno"
    };

    private string[] texPlanetas = new string[] {
    "URP_Mercury_2k",
    "URP_Venus_2k",
    "URP_Earth_2k",
    "URP_Mars_2k",
    "URP_Jupiter_2k",
    "URP_Saturn_2k",
    "URP_Uranus_2k",
    "URP_Neptune_2k"
    };

    private string texSol = "Sun";


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cuerpoCelestePrefab = (GameObject)Resources.Load("Prefabs/CuerpoCeleste", typeof(GameObject));
        GameObject grupoPlanetas = new GameObject("GrupoPlanetas");
        grupoPlanetas.transform.position = posicionPlanetas;
        grupoPlanetas.transform.LookAt(Vector3.zero);


        GameObject sol = Instantiate(cuerpoCelestePrefab);
        sol.name = "Modelo Sol";
        sol.transform.position = posicionSol;

        float radius = .8f; // Adjust based on sphere size
        int q = 0;
        int r = 0;

        for (int i = 0; i < planetas.Length; i++)
        {
            GameObject planeta = Instantiate(cuerpoCelestePrefab);
            planeta.AddComponent<PlanetOrbit>();
            PlanetOrbit orbit=  planeta.GetComponent<PlanetOrbit>();
            orbit.sun = sol.transform;
            orbit.a += i*0.1f;
            orbit.b += i*0.1f;

            planeta.name = planetas[i];
            planeta.transform.SetParent(grupoPlanetas.transform);

            // Position in hex pattern
            Vector3 hexPos = GetHexPosition(q, r, radius) + posicionPlanetas;
            planeta.transform.position = hexPos;

            // Load and assign texture
            Material mat = Resources.Load<Material>("Materials/" + texPlanetas[i]);
            if (mat != null)
                planeta.GetComponent<MeshRenderer>().material = mat;

            // Update q and r to wrap around
            q++;
            if (q >= 4) // 4 per row
            {
                q = 0;
                r++;
            }
        }



    }

    // Update is called once per frame
    void Update()
    {

    }

    private Vector3 GetHexPosition(int q, int r, float radius)
    {
        float width = Mathf.Sqrt(3) * radius;
        float height = 2f * radius;
        float x = width * (q + r / 2f);
        float y = 0f; // all on the same y-plane
        float z = height * 0.75f * r;
        return new Vector3(x - radius *3, -z, y);
    }
}

