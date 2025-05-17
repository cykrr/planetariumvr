using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

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

    void OnMouseDown()
    {
        callback.Invoke();
    }
}
