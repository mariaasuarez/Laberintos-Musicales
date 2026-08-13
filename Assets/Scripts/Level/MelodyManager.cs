using UnityEngine;
using System.Collections.Generic;

public class MelodyManager : MonoBehaviour
{
    [Header("Melody Settings")]
    public int levelNumber = 1;
    public int notesCount = 3; // Nivel 1: 3, Nivel 2: 4
    
    private List<string> availableNotes = new List<string> { "Do", "Re", "Mi", "Fa", "Sol" };
    private List<string> targetMelody = new List<string>();
    private List<string> playerProgress = new List<string>();
    private int currentNoteIndex = 0;
    
    [Header("References")]
    public DoorController doorController;
    
    void Start()
    {
        GenerateMelody();
    }
    
    void GenerateMelody()
    {
        targetMelody.Clear();
        playerProgress.Clear();
        currentNoteIndex = 0;
        
        // Generar melodía aleatoria según el nivel
        for (int i = 0; i < notesCount; i++)
        {
            int randomIndex = Random.Range(0, availableNotes.Count);
            targetMelody.Add(availableNotes[randomIndex]);
        }
        
        Debug.Log($"Level {levelNumber} Melody: {string.Join(" - ", targetMelody)}");
        
        // Reproducir melodía inicial a través de la puerta
        if (doorController != null)
        {
            doorController.PlayInitialMelody(targetMelody);
        }
    }
    
    public bool CheckNote(string noteName)
    {
        if (currentNoteIndex >= targetMelody.Count)
            return false;
        
        string expectedNote = targetMelody[currentNoteIndex];
        
        if (noteName == expectedNote)
        {
            // Nota correcta
            playerProgress.Add(noteName);
            currentNoteIndex++;
            
            Debug.Log($"Correct note! Progress: {string.Join(" - ", playerProgress)}");
            
            // Reproducir progreso parcial
            if (doorController != null)
            {
                doorController.PlayPartialMelody(playerProgress);
            }
            
            // Verificar si completó la melodía
            if (currentNoteIndex >= targetMelody.Count)
            {
                OnMelodyComplete();
            }
            
            return true;
        }
        else
        {
            // Nota incorrecta
            Debug.Log($"Wrong note! Expected: {expectedNote}, Got: {noteName}");
            return false;
        }
    }
    
    void OnMelodyComplete()
    {
        Debug.Log("Melody completed!");
        
        // Reproducir melodía completa
        if (doorController != null)
        {
            doorController.PlayCompleteMelody(targetMelody);
            doorController.EnableDoor();
        }
    }
    
    public List<string> GetTargetMelody()
    {
        return targetMelody;
    }
    
    public List<string> GetPlayerProgress()
    {
        return playerProgress;
    }
    
    public bool IsMelodyComplete()
    {
        return currentNoteIndex >= targetMelody.Count;
    }
    
    public void ResetMelody()
    {
        GenerateMelody();
    }
}