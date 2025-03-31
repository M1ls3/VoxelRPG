using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AbstractDunngeonGenerator), true)]
public class RandomDungeonGeneratorEditor : Editor
{
    AbstractDunngeonGenerator generator;

    private void Awake()
    {
        generator = (AbstractDunngeonGenerator)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Create Dungeon"))
        {
            generator.GenerateDungeon();
        }
    }
}
