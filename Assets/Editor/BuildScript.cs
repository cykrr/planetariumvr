using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BuildScript 
{
    static void PerformBuild()
    {
        string[] defaultScene = { 
            "Assets/Scenes/Main Screen.unity",
            "Assets/Scenes/DragAndDrop.unity",
            "Assets/Scenes/SettingsScene.unity",
            "Assets/Scenes/VistaAnillo.unity",
            "Assets/Scenes/VistaDetallada.unity",
            "Assets/Scenes/Wiimote.unity",
            };

        BuildPipeline.BuildPlayer(defaultScene, "test.apk" ,
            BuildTarget.Android, BuildOptions.None);
    }

}
