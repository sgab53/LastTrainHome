using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour
{
    private enum EnemyStatus {SEARCHING, DESTROY}

    [SerializeField] private EnemyStatus status = EnemyStatus.SEARCHING;

    [SerializeField] private EnviromentPoint currentEnviromentPoint; //dove sto andando
    [SerializeField] private EnviromentPoint oldEnviromentPoint; // dove stavo
    
    public float speed = 2f;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MoveTowardsTarget());
    }

    public void OnEnterRoom(EnviromentPoint currentEnviromentPoint, EnviromentPoint oldEnviromentPoint)
    {
        this.currentEnviromentPoint = currentEnviromentPoint;
        this.oldEnviromentPoint = oldEnviromentPoint;
        StartCoroutine(MoveTowardsTarget());
    }
    
    public void OnPointReached()
    {
        if (currentEnviromentPoint.Connections.Count == 1)
        {
            currentEnviromentPoint = oldEnviromentPoint;
            oldEnviromentPoint = currentEnviromentPoint;
            StartCoroutine(MoveTowardsTarget());
            return;
        }
        bool cantComeBack = Random.Range(0f, 1f) > 0.20f;
        while (true)
        {
            EnviromentPoint point = GetRandomElement(currentEnviromentPoint.Connections);
            if (cantComeBack)
            {
                if (point.Equals(oldEnviromentPoint))
                    continue;
            }

            oldEnviromentPoint = currentEnviromentPoint;
            currentEnviromentPoint = point;
            
            StartCoroutine(MoveTowardsTarget());
            break;
        }
        
    }
    
    T GetRandomElement<T>(IReadOnlyList<T> list)
    {
        int randomIndex = Random.Range(0, list.Count);
        return list[randomIndex];
    }
    
    
    private IEnumerator MoveTowardsTarget()
    {
        while (Vector3.Distance(transform.position, currentEnviromentPoint.GetPosition()) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, currentEnviromentPoint.GetPosition(), speed * Time.deltaTime);
            yield return null; // Aspetta il frame successivo
        }
        
        OnPointReached(); // Chiama la funzione quando raggiunge il target
    }
    
}
