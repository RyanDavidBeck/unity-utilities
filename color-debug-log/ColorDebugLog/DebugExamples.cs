using ColorDebugLog;
using UnityEngine;

public class DebugExamples : MonoBehaviour
{
    private void Start()
    {
        ColorDebug.Log("Test", DebugType.Default);
        ColorDebug.Log("Test", DebugType.Inventory);
        ColorDebug.Log("Test", DebugType.Dialog);
        ColorDebug.Log("Test", DebugType.Test);
    }

}
