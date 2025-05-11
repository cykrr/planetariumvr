using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;
    private static List<string> sceneHistory = new List<string>();

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
        sceneHistory.Add(name);
    }

    public void PreviousScene()
    {
        if (sceneHistory.Count >= 2)
        {
            sceneHistory.RemoveAt(sceneHistory.Count - 1);
            SceneManager.LoadScene(sceneHistory[sceneHistory.Count - 1]);
        }
        else if (sceneHistory.Count == 1)
        {
            sceneHistory.RemoveAt(sceneHistory.Count - 1);
            SceneManager.LoadScene("Main Screen");
        }
        else
        {
            Debug.Log("No previous scene in history");
        }
    }

    private void Update()
    {
        if (WiimoteReceiver.instance.ButtonBClick())
        {
            PreviousScene();
        }
    }

}
