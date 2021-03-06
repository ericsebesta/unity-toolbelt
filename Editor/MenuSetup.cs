﻿using com.ericsebesta.toolbelt;
using UnityEditor;
using UnityEngine;

public class MenuSetup : MonoBehaviour
{
    [MenuItem("GameObject/Toolbelt/ExclusiveChooser", false, 20)]
    private static void CreateExclusiveChooser()
    {
        var go = new GameObject ("ExclusiveChooser");
        go.AddComponent<ExclusiveChooser>();
    }
}
