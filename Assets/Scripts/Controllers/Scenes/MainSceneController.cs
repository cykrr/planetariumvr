using UnityEngine;
using UnityEngine.UI;

public class MainSceneController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public ButtonController inicio, informacion, salir;
    public RayPointer rayPointer;


    void Start()
    {
        inicio.SetCallback(() =>
        {
            SceneController.instance.LoadScene("DragAndDrop");
        });

        informacion.SetCallback(() =>
        {
            SceneController.instance.LoadScene("VistaAnillo");
        });

        salir.SetCallback(() =>
        {
            Application.Quit();
        });
    }
}
