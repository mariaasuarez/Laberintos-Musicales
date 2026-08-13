using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Level Settings")]
    public int currentLevel = 1;
    public int maxLevels = 2;
    
    [Header("Time Settings")]
    public float level1Time = 120f; // 2 minutos
    public float level2Time = 180f; // 3 minutos
    private float currentTime;
    
    [Header("References")]
    public MelodyManager melodyManager;
    public PlayerController playerController;
    public UIManager uiManager;
    
    [Header("Level Configurations")]
    public Vector2Int level1GridSize = new Vector2Int(12, 12);
    public Vector2Int level2GridSize = new Vector2Int(18, 18);
    
    void Start()
    {
        SetupLevel(currentLevel);
    }
    
    void SetupLevel(int level)
    {
        currentLevel = level;
        
        switch (level)
        {
            case 1:
                currentTime = level1Time;
                if (melodyManager != null)
                {
                    melodyManager.levelNumber = 1;
                    melodyManager.notesCount = 3;
                }
                Debug.Log("Setting up Level 1: 12x12 grid, 3 notes");
                break;
                
            case 2:
                currentTime = level2Time;
                if (melodyManager != null)
                {
                    melodyManager.levelNumber = 2;
                    melodyManager.notesCount = 4;
                }
                Debug.Log("Setting up Level 2: 18x18 grid, 4 notes");
                break;
        }
        
        if (uiManager != null)
        {
            uiManager.SetLevel(level);
            uiManager.SetTime(currentTime);
        }
    }
    
    void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            
            if (uiManager != null)
            {
                uiManager.SetTime(currentTime);
            }
            
            if (currentTime <= 0)
            {
                GameOver();
            }
        }
    }
    
    public void LevelComplete()
    {
        Debug.Log($"Level {currentLevel} complete!");
        
        if (currentLevel < maxLevels)
        {
            LoadNextLevel();
        }
        else
        {
            GameComplete();
        }
    }
    
    void LoadNextLevel()
    {
        currentLevel++;
        SceneManager.LoadScene($"Level{currentLevel}");
    }
    
    void GameComplete()
    {
        Debug.Log("Congratulations! All levels complete!");
        if (uiManager != null)
        {
            uiManager.ShowVictoryScreen();
        }
    }
    
    public void GameOver()
    {
        Debug.Log("Game Over!");
        if (uiManager != null)
        {
            uiManager.ShowGameOverScreen();
        }
    }
    
    public void RestartLevel()
    {
        SceneManager.LoadScene($"Level{currentLevel}");
    }
    
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
    public void AddTimePenalty(float penalty)
    {
        currentTime -= penalty;
        if (currentTime < 0)
            currentTime = 0;
            
        Debug.Log($"Time penalty applied: -{penalty} seconds");
    }
}