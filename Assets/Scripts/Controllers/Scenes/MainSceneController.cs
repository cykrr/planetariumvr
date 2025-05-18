using UnityEngine;
using System;

public class MainSceneController : MonoBehaviour
{
    public ButtonController inicio, informacion, salir;
    public ConnectionManager connectionManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inicio.SetCallback(() =>
        {
            if (Convert.ToBoolean(PlayerPrefs.GetInt("isHost", 0)))
                connectionManager.StartHost();
            else
                connectionManager.ConnectClient();
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

    private void Update()
    {
        if (WiimoteReceiver.instance.ButtonHomeClick())
        {
            SceneController.instance.LoadScene("SettingsScene");
        }
    }
}
