using UnityEngine;
using UnityEngine.UI;

public class MainSceneController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public ButtonController inicio, salir;
    public Button vistaInformativa;

    void startDemo () {
        SceneController.instance.LoadScene("DetailedViewScene");
    }

    void goToSettings () {
        SceneController.instance.LoadScene("DetailedViewScene");
    }

    void exitApp () {
        Application.Quit();
    }


    void Start()
    {
        inicio.callback = startDemo;
        salir.callback = exitApp;

        vistaInformativa.onClick.AddListener(() =>
        {
            SceneController.instance.LoadScene("VistaAnillo");
        });
    }
}
