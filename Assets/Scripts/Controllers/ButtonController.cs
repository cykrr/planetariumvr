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

    void OnClick() {
        callback();
    }
}
