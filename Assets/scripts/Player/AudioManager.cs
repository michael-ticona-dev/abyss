using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Ambiental")]
    [SerializeField] private AudioSource audioSource;

    public void CambiarVolumenAmbiental(float valor)
    {
        if (audioSource == null)
            return;

        audioSource.volume = valor;

        Debug.Log("Volumen música: " + audioSource.volume);
    }
}