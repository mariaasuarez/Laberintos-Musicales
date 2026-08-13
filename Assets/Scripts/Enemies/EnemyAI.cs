using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("AI Settings")]
    public float chaseSpeed = 3f;
    public float patrolSpeed = 1.5f;
    public float chaseRange = 6f;
    public float losePlayerRange = 8f;
    
    private enum AIState { Patrol, Chase }
    private AIState currentState = AIState.Patrol;
    
    [Header("References")]
    public EnemyController enemyController;
    public Transform[] patrolPoints;
    private int currentPatrolIndex = 0;
    
    void Start()
    {
        if (enemyController == null)
            enemyController = GetComponent<EnemyController>();
    }
    
    public void UpdateBehavior(Transform player)
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        
        switch (currentState)
        {
            case AIState.Patrol:
                PatrolBehavior(distanceToPlayer);
                break;
                
            case AIState.Chase:
                ChaseBehavior(player, distanceToPlayer);
                break;
        }
    }
    
    void PatrolBehavior(float distanceToPlayer)
    {
        // Verificar si debe perseguir al jugador
        if (distanceToPlayer <= chaseRange)
        {
            currentState = AIState.Chase;
            enemyController.PlayChaseSound();
            return;
        }
        
        // Patrullar entre puntos
        if (patrolPoints != null && patrolPoints.Length > 0)
        {
            Transform targetPoint = patrolPoints[currentPatrolIndex];
            MoveTowards(targetPoint.position, patrolSpeed);
            
            if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
            {
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            }
        }
        else
        {
            // Si no hay puntos de patrulla, movimiento aleatorio
            RandomMovement();
        }
    }
    
    void ChaseBehavior(Transform player, float distanceToPlayer)
    {
        // Verificar si perdió al jugador
        if (distanceToPlayer > losePlayerRange)
        {
            currentState = AIState.Patrol;
            enemyController.PlayPatrolSound();
            return;
        }
        
        // Perseguir al jugador
        MoveTowards(player.position, chaseSpeed);
    }
    
    void MoveTowards(Vector3 target, float speed)
    {
        Vector3 direction = (target - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
        
        // Opcional: rotar hacia el objetivo
        if (direction != Vector3.zero)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
    
    void RandomMovement()
    {
        // Movimiento aleatorio simple
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        transform.position += (Vector3)randomDirection * patrolSpeed * Time.deltaTime;
    }
    
    public void SetPatrolPoints(Transform[] points)
    {
        patrolPoints = points;
    }
}