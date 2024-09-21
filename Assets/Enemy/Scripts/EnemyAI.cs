using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour
{
    private enum EnemyStatus
    {
        SEARCHING,
        DESTROY
    }
    [SerializeField] private bool aiIsActive = true;
    [SerializeField] private Transform player;
    [SerializeField] private EnemyStatus status = EnemyStatus.SEARCHING;

    [SerializeField] private EnviromentPoint currentEnviromentPoint; //dove sto andando
    [SerializeField] private EnviromentPoint oldEnviromentPoint; // dove stavo
    
    public float viewRadius = 5f;  // Raggio del cono visivo
    public float viewAngle = 45f;
    
    public float speed = 2f;
    public float destroySpeed = 3f;
    
    private Coroutine movementCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        movementCoroutine = StartCoroutine(MoveTowardsTarget());
    }
    
    void Update() {

        if (status == EnemyStatus.SEARCHING && IsPlayerInFieldOfView()) {
            StopCoroutine(movementCoroutine);
            movementCoroutine = null;
            status = EnemyStatus.DESTROY;
            movementCoroutine = StartCoroutine(MoveTowardsPlayer());
        }
    }
    
    bool IsPlayerInFieldOfView()
    {
        // Calcola la direzione dal nemico al giocatore
        Vector3 directionToPlayer = player.position - transform.position;
        
        // Verifica se il giocatore è nel raggio
        if (directionToPlayer.magnitude <= viewRadius)
        {
            // Normalizza la direzione per ottenere un vettore di direzione unitario
            directionToPlayer.Normalize();

            // Calcola l'angolo tra la direzione dell'entità e la direzione verso il giocatore
            float angleBetweenEntityAndPlayer = Vector3.Angle(transform.right, directionToPlayer);

            // Controlla se il giocatore è entro l'angolo del cono visivo
            if (angleBetweenEntityAndPlayer < viewAngle)
            {
                // Facoltativo: aggiungi un raycast per verificare se c'è un ostacolo tra l'entità e il giocatore
                RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, viewRadius);
                if (hit.collider != null && hit.collider.transform == player)
                {
                    return true;  // Il giocatore è visibile e non ci sono ostacoli
                }
            }
        }

        return false;  // Il giocatore è fuori dal cono visivo
    }

    public void OnEnterRoom(EnviromentPoint currentEnviromentPoint, EnviromentPoint oldEnviromentPoint)
    {
        this.currentEnviromentPoint = currentEnviromentPoint;
        this.oldEnviromentPoint = oldEnviromentPoint;
        movementCoroutine = StartCoroutine(MoveTowardsTarget());
    }
    
    public void OnPointReached()
    {
        if (currentEnviromentPoint.Connections.Count == 1)
        {
            SetNewTarget(oldEnviromentPoint);
            movementCoroutine = StartCoroutine(MoveTowardsTarget());
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
            SetNewTarget(point);
            movementCoroutine = StartCoroutine(MoveTowardsTarget());
            break;
        }
    }

    private void SetNewTarget(EnviromentPoint newEnviromentPoint)
    {
        oldEnviromentPoint = currentEnviromentPoint;
        currentEnviromentPoint = newEnviromentPoint;
        Vector3 direction = currentEnviromentPoint.transform.position - transform.position;
        transform.position = Vector3.MoveTowards(transform.position, currentEnviromentPoint.transform.position, speed * Time.deltaTime);
        // Ruota l'entità verso il target

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
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
    
    private IEnumerator MoveTowardsPlayer()
    {
        while (Vector3.Distance(transform.position, player.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, destroySpeed * Time.deltaTime);
            yield return null; // Aspetta il frame successivo
        }
        
        Debug.Log("Player destroyed");
    }
    
}
