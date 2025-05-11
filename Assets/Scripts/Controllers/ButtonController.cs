using UnityEngine;
using UnityEngine.Events;

public class ButtonController : MonoBehaviour
{
    public UnityAction callback = (() => { });

    public void SetCallback(UnityAction callback)
    {
        this.callback = callback;
    }

    public void OnPointerEnter() {
        Material glowMaterial = gameObject.GetComponent<Renderer>().materials[1];
        glowMaterial.SetFloat("_Scale", 1.05f);
    }

    public void OnPointerExit() {
        Material glowMaterial = gameObject.GetComponent<Renderer>().materials[1];
        glowMaterial.SetFloat("_Scale", 0f);
    }
}
