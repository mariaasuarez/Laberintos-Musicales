using UnityEngine;
using System.Collections.Generic;

public class DoorController : MonoBehaviour
{
    [Header("Door Settings")]
    public bool isOpen = false;
    public bool requiresMelody = true;
    
    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip doorOpenSound;
    public AudioClip doorLockSound;
    
    [Header("Visual Settings")]
    public SpriteRenderer spriteRenderer;
    public Sprite closedSprite;
    public Sprite openSprite;
    public Color closedColor = Color.red;
    public Color openColor = Color.green;
    
    [Header("Melody Playback")]
    public float noteDelay = 0.5f;
    
    void Start()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
            
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
            
        UpdateDoorVisual();
    }
    
    public void PlayInitialMelody(List<string> melody)
    {
        StartCoroutine(PlayMelodyCoroutine(melody));
    }
    
    public void PlayPartialMelody(List<string> progress)
    {
        StartCoroutine(PlayMelodyCoroutine(progress));
    }
    
    public void PlayCompleteMelody(List<string> melody)
    {
        StartCoroutine(PlayMelodyCoroutine(melody));
    }
    
    System.Collections.IEnumerator PlayMelodyCoroutine(List<string> notes)
    {
        foreach (string note in notes)
        {
            PlayNoteSound(note);
            yield return new WaitForSeconds(noteDelay);
        }
    }
    
    void PlayNoteSound(string noteName)
    {
        // Aquí se conectaría con el sistema de audio procedural
        Debug.Log($"Playing note: {noteName}");
        // Por ahora, solo un log
    }
    
    public void EnableDoor()
    {
        isOpen = true;
        UpdateDoorVisual();
        
        if (audioSource != null && doorOpenSound != null)
        {
            audioSource.PlayOneShot(doorOpenSound);
        }
        
        Debug.Log("Door is now open!");
    }
    
    public void DisableDoor()
    {
        isOpen = false;
        UpdateDoorVisual();
        
        if (audioSource != null && doorLockSound != null)
        {
            audioSource.PlayOneShot(doorLockSound);
        }
    }
    
    void UpdateDoorVisual()
    {
        if (spriteRenderer != null)
        {
            if (isOpen)
            {
                if (openSprite != null)
                    spriteRenderer.sprite = openSprite;
                spriteRenderer.color = openColor;
            }
            else
            {
                if (closedSprite != null)
                    spriteRenderer.sprite = closedSprite;
                spriteRenderer.color = closedColor;
            }
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isOpen)
        {
            Debug.Log("Player reached the door! Level complete.");
            // Aquí se llamaría al LevelManager para avanzar de nivel
        }
        else if (other.CompareTag("Player") && !isOpen)
        {
            Debug.Log("Door is locked. Complete the melody first.");
        }
    }
}