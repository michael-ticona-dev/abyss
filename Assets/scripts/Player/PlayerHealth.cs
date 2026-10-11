using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Vida")]
    [SerializeField] private int vidaMaxima = 10;
    [SerializeField] private int vidaActual = 10;

    [Header("UI Corazones")]
    [SerializeField] private Image[] corazones;

    [Header("Sprites de corazón")]
    [SerializeField] private Sprite corazonEntero;
    [SerializeField] private Sprite corazonMedio;
    [SerializeField] private Sprite corazonCasiDañado;
    [SerializeField] private Sprite corazonDañado;

    [Header("Vida por corazón")]
    [SerializeField] private int vidaPorCorazon = 2;

    private void Start()
    {
        vidaActual = vidaMaxima;
        ActualizarUI();
    }

    public void RecibirDaño(int daño)
    {
        if (daño <= 0)
            return;

        vidaActual -= daño;

        if (vidaActual < 0)
            vidaActual = 0;

        ActualizarUI();

        if (vidaActual <= 0)
        {
            Morir();
        }
    }

    public void Curar(int cantidad)
    {
        if (cantidad <= 0)
            return;

        vidaActual += cantidad;

        if (vidaActual > vidaMaxima)
            vidaActual = vidaMaxima;

        ActualizarUI();
    }

    private void ActualizarUI()
    {
        if (corazones == null)
            return;

        for (int i = 0; i < corazones.Length; i++)
        {
            if (corazones[i] == null)
                continue;

            int vidaCorazon =
                vidaActual - (i * vidaPorCorazon);

            if (vidaCorazon >= 2)
            {
                if (corazonEntero != null)
                    corazones[i].sprite = corazonEntero;
            }
            else if (vidaCorazon == 1)
            {
                if (corazonMedio != null)
                    corazones[i].sprite = corazonMedio;
            }
            else
            {
                if (corazonDañado != null)
                    corazones[i].sprite = corazonDañado;
            }
        }
    }

    private void Morir()
    {
        Debug.Log("El jugador ha muerto.");
    }

    public int VidaActual
    {
        get { return vidaActual; }
    }

    public int VidaMaxima
    {
        get { return vidaMaxima; }
    }
}