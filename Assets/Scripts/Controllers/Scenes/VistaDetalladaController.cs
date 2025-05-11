using TMPro;
using UnityEngine;

public class VistaDetalladaController : MonoBehaviour
{
    private CuerpoCeleste cuerpoCeleste;

    public TextMeshPro descriptionText;
    public MeshRenderer modelRenderer;

    private void Awake()
    {
        cuerpoCeleste = AppController.instance.cuerpoActual;

        Material mat = Resources.Load<Material>("Materials/" + cuerpoCeleste.modelo);
        Material[] mats = modelRenderer.materials;
        mats[0] = mat;
        modelRenderer.materials = mats;

        descriptionText.SetText(cuerpoCeleste.descripcion);
    }

}
