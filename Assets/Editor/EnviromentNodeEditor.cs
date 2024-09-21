using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnviromentPoint))]
public class EnviromentNodeEditor : Editor 
{
    public override void OnInspectorGUI() {
        // Riferimento al nodo corrente
        EnviromentPoint node = (EnviromentPoint)target;

        // Disegna il campo di default per modificare la lista delle connessioni
        DrawDefaultInspector();

        if (GUILayout.Button("Update Bidirectional Connections")) {
            node.EnsureBidirectionalConnection();
        }

        // Forza Unity a salvare le modifiche quando si modifica l'Editor
        if (GUI.changed) {
            EditorUtility.SetDirty(node);
        }
    }
}
