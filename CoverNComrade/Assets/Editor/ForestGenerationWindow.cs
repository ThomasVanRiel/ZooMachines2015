using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using System.Collections;
using System.Collections.Generic;

public class ForestGenerationWindow : EditorWindow {

    private Triangle _triangle;
    private GameObject _prefab;
    private int _amount;
    private int _index = 0;
    private AnimBool _showGeneration;


    [MenuItem("Window/ForestGeneration")]
    public static void ShowWindow() {
        var window = EditorWindow.GetWindow<ForestGenerationWindow>();
        window.name = "Forest Generator";
    }

    void OnEnable() {
        _showGeneration = new AnimBool(false);
        _showGeneration.valueChanged.AddListener(Repaint);
    }

    private void OnGUI() {
        GUILayout.Label("Forest generator", EditorStyles.boldLabel);
        GUILayout.Space(20);

        _triangle = EditorGUILayout.ObjectField("Forest Outline", _triangle, typeof(Triangle), true) as Triangle;
        _prefab = EditorGUILayout.ObjectField("Tree Prefab", _prefab, typeof(GameObject), true) as GameObject;

        GUILayout.Space(10);

        if (_triangle && _prefab)
            _showGeneration = new AnimBool(true);
        else
            _showGeneration = new AnimBool(false);

        if (EditorGUILayout.BeginFadeGroup(_showGeneration.faded)) {
            _amount = EditorGUILayout.IntField("Amount", _amount);

            GUILayout.Space(20);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("GENERATE", GUILayout.ExpandWidth(false)))
                GenerateObjects();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        EditorGUILayout.EndFadeGroup();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Cleanup", GUILayout.ExpandWidth(false)))
            CleanUp();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

    }

    void GenerateObjects() {
        Undo.IncrementCurrentGroup();
        for (int i = 0; i < _amount; i++) {

            float r1 = Random.Range(0.0f, 1.0f);
            float r2 = Random.Range(0.0f, 1.0f);

            Vector3 pos = (1 - Mathf.Sqrt(r1)) * _triangle.Coordinates[0] + (Mathf.Sqrt(r1) * (1 - r2)) * _triangle.Coordinates[1] + (Mathf.Sqrt(r1) * r2) * _triangle.Coordinates[2];

            Quaternion rot = Quaternion.Euler(0, Random.Range(0.0f, 360.0f), 0);

            GameObject obj = (Instantiate(_prefab, pos, rot) as GameObject);
            obj.name = _prefab.name + "_" + _index++;
            obj.transform.parent = _triangle.transform;
            Undo.RegisterCreatedObjectUndo(obj, "instantiated prefab");
        }

        Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
    }

    void CleanUp() {
        var gameObject =_triangle.gameObject;
        DestroyImmediate(gameObject.GetComponent<Triangle>());
    }

}
