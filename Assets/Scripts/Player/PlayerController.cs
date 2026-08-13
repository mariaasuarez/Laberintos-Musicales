using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public GridMovementController gridMovement;
    public MelodyManager melodyManager;
    
    [Header("Player Stats")]
    public int maxLives = 3;
    public int currentLives = 3;
    
    [Header("Interaction")]
    public LayerMask instrumentLayer;
    public LayerMask enemyLayer;
    
    void Start()
    {
        currentLives = maxLives;
    }
    
    void Update()
    {
        if (!gridMovement.IsPlayerMoving())
        {
            CheckForInteractions();
        }
    }
    
    void CheckForInteractions()
    {
        // Verificar si hay instrumentos en la celda actual
        Collider2D[] colliders = Physics2D.OverlapPointAll(transform.position, instrumentLayer);
        
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Instrument"))
            {
                InstrumentController instrument = collider.GetComponent<InstrumentController>();
                if (instrument != null && !instrument.IsActivated())
                {
                    instrument.Interact();
                    
                    // Verificar si la nota es correcta con el MelodyManager
                    if (melodyManager != null)
                    {
                        bool isCorrect = melodyManager.CheckNote(instrument.GetNoteName());
                        
                        if (isCorrect)
                        {
                            instrument.Activate();
                        }
                        else
                        {
                            // Aplicar penalización de tiempo
                            LevelManager levelManager = FindAnyObjectByType<LevelManager>();
                            if (levelManager != null)
                            {
                                levelManager.AddTimePenalty(10f);
                            }
                        }
                    }
                    
                    break; // Solo interactuar con un instrumento a la vez
                }
            }
        }
    }
    
    void CheckForEnemies()
    {
        Collider2D[] enemies = Physics2D.OverlapPointAll(transform.position, enemyLayer);
        
        foreach (var enemy in enemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                TakeDamage();
                break;
            }
        }
    }
    
    public void TakeDamage()
    {
        currentLives--;
        Debug.Log($"Player hit! Lives remaining: {currentLives}");
        
        if (currentLives <= 0)
        {
            GameOver();
        }
    }
    
    void GameOver()
    {
        Debug.Log("Game Over!");
        LevelManager levelManager = FindAnyObjectByType<LevelManager>();
        if (levelManager != null)
        {
            levelManager.GameOver();
        }
    }
    
    public int GetCurrentLives()
    {
        return currentLives;
    }
    
    public void ResetPlayer()
    {
        currentLives = maxLives;
        // Resetear posición según el nivel
    }
}