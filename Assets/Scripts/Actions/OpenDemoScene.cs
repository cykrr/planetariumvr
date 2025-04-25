using UnityEngine;

public class OpenDemoScene : MonoBehaviour, IClickable
{
    public void OnClick()
    {
        SceneController.instance.LoadScene("HelloCardboard");
    }
}
