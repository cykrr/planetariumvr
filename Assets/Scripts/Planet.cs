using TMPro;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public GameObject sphere;
    public TextMeshPro label;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void Initialize(Material material, string name)
    {
        sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);

    }

    void Start()
    {
        this.transform.position -= new Vector3(0, 0, -7);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
