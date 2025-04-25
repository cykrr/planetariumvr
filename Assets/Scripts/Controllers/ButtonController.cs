using System;
using Unity.Burst;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public delegate void WhateverType();
    public WhateverType callback;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OnPointerClick() {
        callback();
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
