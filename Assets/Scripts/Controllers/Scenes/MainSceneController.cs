using UnityEngine;
using UnityEngine.UI;

public class MainSceneController : MonoBehaviour
{
    public ButtonController inicio, informacion, salir;
    public ConnectionManager connectionManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inicio.SetCallback(() =>
        {
            if (Application.platform == RuntimePlatform.Android)
                connectionManager.ConnectClient();
            else
                connectionManager.StartHost();
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
