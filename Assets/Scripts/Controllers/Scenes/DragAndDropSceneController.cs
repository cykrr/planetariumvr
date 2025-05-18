using Mirror;
using UnityEngine;

public class DragAndDropSceneController : NetworkBehaviour
{
    public GameObject planetaPrefab;
    public GameObject solPrefab;

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


    public override void OnStartServer()
    {
        base.OnStartServer();

        /*
        GameObject grupoPlanetas = new GameObject("grupoPlanetas");
        grupoPlanetas.transform.position = posicionPlanetas;
        grupoPlanetas.transform.LookAt(Vector3.zero);
        */


        GameObject sol = Instantiate(solPrefab);
        sol.name = "Modelo Sol";
        sol.GetComponent<SharedObject>().position = posicionSol;
        NetworkServer.Spawn(sol);

        float radius = .8f; // Adjust based on sphere size
        int q = 0;
        int r = 0;

        for (int i = 0; i < planetas.Length; i++)
        {
            GameObject planeta = Instantiate(planetaPrefab);

            PlanetOrbit orbit = planeta.GetComponent<PlanetOrbit>();
            orbit.a += i*0.1f;
            orbit.b += i*0.1f;

            planeta.name = planetas[i];
            // planeta.transform.SetParent(grupoPlanetas.transform);

            // Position in hex pattern
            Vector3 hexPos = GetHexPosition(q, r, radius) + posicionPlanetas;

            SharedObject sharedObject = planeta.GetComponent<SharedObject>();
            sharedObject.position = hexPos;
            sharedObject.materialName = texPlanetas[i];
            sharedObject.tagName = "Interactable";

            NetworkServer.Spawn(planeta);

            // Update q and r to wrap around
            q++;
            if (q >= 4) // 4 per row
            {
                q = 0;
                r++;
            }
        }
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

