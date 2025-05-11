using UnityEngine;
using UnityEngine.UI;

public class UpdateLayout : MonoBehaviour
{
    void Start()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
    }
}
