using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGenerator))]
public class custonInspector : Editor
{
    public override void OnInspectorGUI()
    {

        DrawDefaultInspector();

        MapGenerator gen = (MapGenerator)target;
        if (GUILayout.Button("New Seed")) {
            gen.NewSeed();
            gen.GenerateMap();
        }

        if (GUILayout.Button("Generate Map")) {
            gen.GenerateMap();
        }

        if (GUILayout.Button("Clear Map")) {
            gen.ClearMap();
        }
    }
}
