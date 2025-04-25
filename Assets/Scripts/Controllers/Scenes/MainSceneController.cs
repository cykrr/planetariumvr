using UnityEngine;
using UnityEngine.UI;

public class MainSceneController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public ButtonController inicio, salir, ajustes;

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
        ajustes.callback = goToSettings;
        salir.callback = exitApp;
    }
}
