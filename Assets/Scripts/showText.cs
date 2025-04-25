using System;
using TMPro;
using UnityEngine;

public class showText : MonoBehaviour
{

    public TextMeshPro fpsTextRef;
    void Start() {

    }
        
    void Update()
    {
        fpsTextRef.text = (1/Time.deltaTime).ToString("0.00");
    }
}
