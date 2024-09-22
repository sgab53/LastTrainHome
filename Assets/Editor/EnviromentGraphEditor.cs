using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnviromentGraph))]
public class EnviromentGraphEditor : Editor
{
    public override void OnInspectorGUI() {
        // Riferimento al nodo corrente
        EnviromentGraph graph = (EnviromentGraph)target;

        // Disegna il campo di default per modificare la lista delle connessioni
        DrawDefaultInspector();

        if (GUILayout.Button("Update Points list")) {
            EnviromentPoint[] points = target.GetComponentsInChildren<EnviromentPoint>();
            graph.points.Clear();
            foreach (var p in points)
            {
                graph.points.Add(p);
            }
        }

        // Forza Unity a salvare le modifiche quando si modifica l'Editor
        if (GUI.changed) {
            EditorUtility.SetDirty(graph);
        }
    }
}
