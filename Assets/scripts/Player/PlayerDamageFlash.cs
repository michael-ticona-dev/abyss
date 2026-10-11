
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerDamageFlash : MonoBehaviour
{
    [SerializeField] private Image panelFlash;
    [SerializeField] private float duracionFlash = 0.1f;
    [SerializeField] private int cantidadFlashes = 3;

    private Coroutine efectoActual;

    private void Awake()
    {
        if (panelFlash != null)
        {
            Color color = panelFlash.color;
            color.a = 0f;
            panelFlash.color = color;
        }
    }

    public void MostrarFlash()
    {
        if (panelFlash == null) return;

        if (efectoActual != null)
            StopCoroutine(efectoActual);

        efectoActual = StartCoroutine(AnimarFlash());
    }

    private IEnumerator AnimarFlash()
    {
        Color color = panelFlash.color;

        for (int i = 0; i < cantidadFlashes; i++)
        {
            color.a = 0.5f;
            panelFlash.color = color;

            yield return new WaitForSeconds(duracionFlash);

            color.a = 0f;
            panelFlash.color = color;

            yield return new WaitForSeconds(duracionFlash);
        }

        efectoActual = null;
    }
}

