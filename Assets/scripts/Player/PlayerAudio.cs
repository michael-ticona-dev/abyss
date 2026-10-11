using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;

    [Header("Clips")]
    [SerializeField] private AudioClip sonidoSalto;
    [SerializeField] private AudioClip sonidoCorrer;
    [SerializeField] private AudioClip sonidoCaer;
    [SerializeField] private AudioClip sonidoAtaque;
    [SerializeField] private AudioClip sonidoDaño;
    [SerializeField] private AudioClip sonidoMuerte;

    [Header("Configuración")]
    [SerializeField] private float velocidadSonidoCorrer = 1.5f;

    private void Awake()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    public void ReproducirSalto()
    {
        Reproducir(sonidoSalto);
    }

    public void ReproducirCorrer()
    {
        if (audioSource == null || sonidoCorrer == null)
            return;

        if (audioSource.isPlaying && audioSource.clip == sonidoCorrer)
            return;

        audioSource.Stop();

        audioSource.clip = sonidoCorrer;
        audioSource.loop = true;
        audioSource.pitch = velocidadSonidoCorrer;

        audioSource.Play();
    }

    public void DetenerCorrer()
    {
        if (audioSource == null)
            return;

        if (audioSource.clip == sonidoCorrer)
        {
            audioSource.Stop();
            audioSource.clip = null;
            audioSource.loop = false;
            audioSource.pitch = 1f;
        }
    }

    public void ReproducirCaer()
    {
        Reproducir(sonidoCaer);
    }

    public void ReproducirAtaque()
    {
        Reproducir(sonidoAtaque);
    }

    public void ReproducirDaño()
    {
        Reproducir(sonidoDaño);
    }

    public void ReproducirMuerte()
    {
        Reproducir(sonidoMuerte);
    }

    private void Reproducir(AudioClip clip)
    {
        if (audioSource == null || clip == null)
            return;

        audioSource.PlayOneShot(clip);
    }

    public void CambiarVolumen(float volumen)
    {
        if (audioSource != null)
            audioSource.volume = volumen;
    }
    
}