using UnityEngine;
using UnityEditor;
using System.Reflection;

[CustomEditor(typeof(MonoBehaviour), true)]
public class ButtonDrawer : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MonoBehaviour mono = (MonoBehaviour)target;
        var methods = mono.GetType()
            .GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

        foreach (var method in methods)
        {
            if (method.GetCustomAttribute(typeof(ButtonAttribute)) != null && method.GetParameters().Length == 0)
            {
                if (GUILayout.Button(method.Name))
                {
                    method.Invoke(mono, null);
                }
            }
        }
    }
}
