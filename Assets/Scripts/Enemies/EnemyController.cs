using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy Settings")]
    public float moveSpeed = 2f;
    public int damage = 1;
    public float detectionRange = 5f;
    
    [Header("Patrol Settings")]
    public Transform[] patrolPoints;
    // private int currentPatrolIndex = 0; // Reservado para uso futuro
    // private bool isPatrolling = true; // Reservado para uso futuro
    
    [Header("References")]
    public Transform player;
    public EnemyAI enemyAI;
    
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip patrolSound;
    public AudioClip chaseSound;
    public AudioClip hitSound;
    
    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
            
        if (enemyAI == null)
            enemyAI = GetComponent<EnemyAI>();
            
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }
    
    void Update()
    {
        if (enemyAI != null)
        {
            enemyAI.UpdateBehavior(player);
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.TakeDamage();
                PlayHitSound();
            }
        }
    }
    
    void PlayHitSound()
    {
        if (audioSource != null && hitSound != null)
        {
            audioSource.PlayOneShot(hitSound);
        }
    }
    
    public void PlayPatrolSound()
    {
        if (audioSource != null && patrolSound != null)
        {
            audioSource.PlayOneShot(patrolSound);
        }
    }
    
    public void PlayChaseSound()
    {
        if (audioSource != null && chaseSound != null)
        {
            audioSource.PlayOneShot(chaseSound);
        }
    }
}