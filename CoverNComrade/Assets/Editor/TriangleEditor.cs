using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Triangle))]
public class TriangleEditor : Editor {

    private void OnSceneGUI() {
        var forest = (Triangle)target;
        if (Event.current.commandName == "UndoRedoPerformed") {
            EditorUtility.SetDirty(forest);
        }

        Undo.RecordObject(forest, "Edit Forest");
        forest.Coordinates[0] = Handles.PositionHandle(forest.Coordinates[0], Quaternion.identity);
        forest.Coordinates[1] = Handles.PositionHandle(forest.Coordinates[1], Quaternion.identity);
        forest.Coordinates[2] = Handles.PositionHandle(forest.Coordinates[2], Quaternion.identity);

        Vector3[] points = new Vector3[4];
        for (int i = 0; i < 4; i++) {
            points[i] = forest.Coordinates[i % 3];
        }

        Handles.DrawPolyLine(points);

        if (GUI.changed) {
            EditorUtility.SetDirty(target);
        }
    }
}
