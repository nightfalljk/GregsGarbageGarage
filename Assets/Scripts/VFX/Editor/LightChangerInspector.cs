using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LightChanger))]
public class LightChangerInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        LightChanger lc = (LightChanger)target;
        if(GUILayout.Button("Set to Day"))
        {
            lc.ApplySettings(0.5f);
        }
        if (GUILayout.Button("Set to Night"))
        {
            lc.ApplySettings(1.0f);
        }
    }
}
