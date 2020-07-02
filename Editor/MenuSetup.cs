using com.ericsebesta.unitytools;
using UnityEditor;
using UnityEngine;

public class MenuSetup : MonoBehaviour
{
    [MenuItem("GameObject/EASTools/ExclusiveChooser")]
    private static void CreateRedBlueGameObject()
    {
        var go = new GameObject ("ExclusiveChooser");
        go.AddComponent<ExclusiveChooser>();
    }
}
