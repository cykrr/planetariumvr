using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VistaAnilloController : MonoBehaviour
{
    public GameObject cuerpoCelestePrefab;
    private float r = 3;



    void Start()
    {
        List<CuerpoCeleste> cuerposCelestes = AppController.instance.GetCuerposCelestes();

        for (int i = 0; i < cuerposCelestes.Count; i++)
        {
            float angle = 2 * Mathf.PI * i / cuerposCelestes.Count + 2 * Mathf.PI/4;
            float x = r * Mathf.Cos(angle);
            float z = r * Mathf.Sin(angle);

            GameObject cuerpo = Instantiate(cuerpoCelestePrefab);
            cuerpo.transform.position = new Vector3(x, 0, z);

            // Cargar modelo
            Material mat = Resources.Load<Material>("Materials/" + cuerposCelestes[i].modelo);
            MeshRenderer renderer = cuerpo.GetComponent<MeshRenderer>();
            Material[] mats = renderer.materials;
            mats[0] = mat;
            renderer.materials = mats;

            // Crear el objeto de texto en la posición (x, z) pero con y = 2
            GameObject texto = new GameObject("Texto_" + cuerposCelestes[i].modelo);
            texto.transform.position = new Vector3(x, -0.9f, z);

            // Añadir un componente TextMeshPro (o TextMesh si no usas TMP)
            TextMeshPro textMeshPro = texto.AddComponent<TextMeshPro>();
            textMeshPro.text = cuerposCelestes[i].nombre; // Asignar el nombre del planeta
            textMeshPro.fontSize = 2; // Ajusta el tamaño de la fuente si es necesario
            textMeshPro.alignment = TextAlignmentOptions.Center; // Alineaci�n centrada

            // Hacer que la etiqueta mire hacia el origen (0, 0, 0)
            texto.transform.LookAt(Vector3.zero);
            texto.transform.Rotate(0, 180, 0);

            // Interacción con control
            ButtonController buttonController = cuerpo.GetComponent<ButtonController>();
            int index = i;

            buttonController.SetCallback(() =>
            {
                AppController.instance.cuerpoActual = cuerposCelestes[index];
                SceneController.instance.LoadScene("VistaDetallada");
            });
        }
    }
}
