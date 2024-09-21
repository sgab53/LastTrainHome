using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviromentPoint : MonoBehaviour
{
    public IReadOnlyList<EnviromentPoint> Connections => _connections;
    [SerializeField] List<EnviromentPoint> _connections = new List<EnviromentPoint>();

    public Vector2 GetPosition()
    {
        return transform.position;
    }
    
    public void EnsureBidirectionalConnection() {
        foreach (EnviromentPoint connectedNode in _connections) {
            if (!connectedNode._connections.Contains(this)) {
                connectedNode.AddConnection(this);
            }
        }
    }
    
    private void AddConnection(EnviromentPoint node) {
        if (!_connections.Contains(node)) {
            _connections.Add(node);
        }
    }
    
    private void OnDrawGizmos() {
        // Imposta il colore del Gizmo a verde
        Gizmos.color = Color.green;

        // Disegna un pallino nel centro del nodo
        Gizmos.DrawSphere(transform.position, 0.2f);

        // Disegna le connessioni (linee) verso altri nodi
        Gizmos.color = Color.blue;  // Linee blu per le connessioni
        foreach (EnviromentPoint connectedNode in _connections) {
            if (connectedNode != null) {
                Gizmos.DrawLine(transform.position, connectedNode.transform.position);
            }
        }
    }
}
