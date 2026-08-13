using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI melodyProgressText;
    
    [Header("Screens")]
    public GameObject gameOverScreen;
    public GameObject victoryScreen;
    public GameObject pauseScreen;
    
    [Header("Melody Display")]
    public GameObject[] melodyNoteIcons; // Array de iconos para mostrar las notas
    
    void Start()
    {
        HideAllScreens();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }
    
    public void SetLevel(int level)
    {
        if (levelText != null)
        {
            levelText.text = $"Nivel {level}";
        }
    }
    
    public void SetTime(float time)
    {
        if (timeText != null)
        {
            int minutes = Mathf.FloorToInt(time / 60);
            int seconds = Mathf.FloorToInt(time % 60);
            timeText.text = $"{minutes:00}:{seconds:00}";
        }
    }
    
    public void SetLives(int lives)
    {
        if (livesText != null)
        {
            livesText.text = $"Vidas: {lives}";
        }
    }
    
    public void SetMelodyProgress(int current, int total)
    {
        if (melodyProgressText != null)
        {
            melodyProgressText.text = $"Melodía: {current}/{total}";
        }
        
        // Actualizar iconos de notas si existen
        if (melodyNoteIcons != null && melodyNoteIcons.Length > 0)
        {
            for (int i = 0; i < melodyNoteIcons.Length; i++)
            {
                if (melodyNoteIcons[i] != null)
                {
                    melodyNoteIcons[i].SetActive(i < current);
                }
            }
        }
    }
    
    public void ShowGameOverScreen()
    {
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
            Time.timeScale = 0f; // Pausar el juego
        }
    }
    
    public void ShowVictoryScreen()
    {
        if (victoryScreen != null)
        {
            victoryScreen.SetActive(true);
            Time.timeScale = 0f; // Pausar el juego
        }
    }
    
    public void HideAllScreens()
    {
        if (gameOverScreen != null)
            gameOverScreen.SetActive(false);
        if (victoryScreen != null)
            victoryScreen.SetActive(false);
        if (pauseScreen != null)
            pauseScreen.SetActive(false);
        Time.timeScale = 1f; // Reanudar el juego
    }
    
    public void TogglePause()
    {
        if (pauseScreen != null)
        {
            bool isPaused = pauseScreen.activeSelf;
            pauseScreen.SetActive(!isPaused);
            Time.timeScale = isPaused ? 1f : 0f;
        }
    }
    
    public void OnRestartButton()
    {
        Time.timeScale = 1f;
        LevelManager levelManager = FindAnyObjectByType<LevelManager>();
        if (levelManager != null)
        {
            levelManager.RestartLevel();
        }
    }
    
    public void OnMainMenuButton()
    {
        Time.timeScale = 1f;
        LevelManager levelManager = FindAnyObjectByType<LevelManager>();
        if (levelManager != null)
        {
            levelManager.LoadMainMenu();
        }
    }
    
    public void OnResumeButton()
    {
        HideAllScreens();
    }
}