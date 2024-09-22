using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Riferimento al player (assegnabile nell'editor o via script)
    public Vector3 offset;   // Offset per mantenere la distanza fissa

    void Start()
    {
        transform.position = player.position;
        transform.position += new Vector3(0,0,-10);
        // Imposta un offset iniziale se non è già stato fatto nell'editor
        if (offset == Vector3.zero)
        {
            offset = transform.position - player.position;
        }
    }

    void LateUpdate()
    {
        // Aggiorna la posizione della camera in base a quella del player
        transform.position = player.position + offset;
    }
}
