using UnityEngine;

public class InstrumentController : MonoBehaviour
{
    [Header("Instrument Settings")]
    public string noteName = "Do";
    public int noteIndex = 0; // 0=Do, 1=Re, 2=Mi, 3=Fa, 4=Sol
    public bool isActivated = false;
    
    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip noteSound;
    
    [Header("Visual Feedback")]
    public SpriteRenderer spriteRenderer;
    public Color normalColor = Color.white;
    public Color activatedColor = Color.green;
    
    void Start()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
            
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }
    
    public void Interact()
    {
        if (!isActivated)
        {
            PlayNote();
            // Aquí se comunicará con el MelodyManager para verificar si es la nota correcta
            Debug.Log($"Instrument played: {noteName}");
        }
    }
    
    void PlayNote()
    {
        if (audioSource != null && noteSound != null)
        {
            audioSource.PlayOneShot(noteSound);
        }
    }
    
    public void Activate()
    {
        isActivated = true;
        if (spriteRenderer != null)
        {
            spriteRenderer.color = activatedColor;
        }
    }
    
    public void ResetInstrument()
    {
        isActivated = false;
        if (spriteRenderer != null)
        {
            spriteRenderer.color = normalColor;
        }
    }
    
    public bool IsActivated()
    {
        return isActivated;
    }
    
    public string GetNoteName()
    {
        return noteName;
    }
    
    public int GetNoteIndex()
    {
        return noteIndex;
    }
}