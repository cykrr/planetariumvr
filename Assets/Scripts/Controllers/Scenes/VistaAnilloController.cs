using TMPro;
using UnityEngine;

public class VistaAnilloController : MonoBehaviour
{
    public GameObject cuerpoCelestePrefab;

    private string[] cuerpoNames = new string[] {
        "Sol",
        "Mercurio",
        "Venus",
        "Tierra",
        "Marte",
        "Júpiter",
        "Saturno",
        "Urano",
        "Neptuno"
    };

    private string[] modelNames = new string[] {
    "Sun",
    "URP_Mercury_2k",
    "URP_Venus_2k",
    "URP_Earth_2k",
    "URP_Mars_2k",
    "URP_Jupiter_2k",
    "URP_Saturn_2k",
    "URP_Uranus_2k",
    "URP_Neptune_2k"
    };
    private float r = 3;



    void Start()
    {
        for (int i = 0; i < modelNames.Length; i++)
        {
            float angle = 2 * Mathf.PI * i / modelNames.Length + 2 * Mathf.PI/4;
            float x = r * Mathf.Cos(angle);
            float z = r * Mathf.Sin(angle);

            GameObject cuerpo = Instantiate(cuerpoCelestePrefab);
            cuerpo.transform.position = new Vector3(x, 0, z);

            // Cargar modelo
            Material mat = Resources.Load<Material>("Materials/" + modelNames[i]);
            MeshRenderer renderer = cuerpo.GetComponent<MeshRenderer>();
            Material[] mats = renderer.materials;
            mats[0] = mat;
            renderer.materials = mats;

            // Crear el objeto de texto en la posici�n (x, z) pero con y = 2
            GameObject texto = new GameObject("Texto_" + cuerpoNames[i]);
            texto.transform.position = new Vector3(x, -0.9f, z);

            // A�adir un componente TextMeshPro (o TextMesh si no usas TMP)
            TextMeshPro textMeshPro = texto.AddComponent<TextMeshPro>();
            textMeshPro.text = cuerpoNames[i]; // Asignar el nombre del planeta
            textMeshPro.fontSize = 2; // Ajusta el tama�o de la fuente si es necesario
            textMeshPro.alignment = TextAlignmentOptions.Center; // Alineaci�n centrada

            // Hacer que la etiqueta mire hacia el origen (0, 0, 0)
            texto.transform.LookAt(Vector3.zero);
            texto.transform.Rotate(0, 180, 0);

            // Interacción con control
            ButtonController buttonController = cuerpo.GetComponent<ButtonController>();
            buttonController.SetCallback(() =>
            {
                SceneController.instance.LoadScene("VistaDetallada");
            });
        }
    }
}
