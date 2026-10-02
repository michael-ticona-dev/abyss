using UnityEngine;

public class GameOverFueraZona : MonoBehaviour
{
    [SerializeField] private GameObject gameOver;
    [SerializeField] private float tiempoGameOver = 2f;

    private Vector3 posicionInicial;
    private bool reiniciando = false;

    private void Start()
    {
        GameObject jugador = GameObject.FindGameObjectWithTag("Player");

        if (jugador != null)
        {
            posicionInicial = jugador.transform.position;
        }

        if (gameOver != null)
            gameOver.SetActive(false);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (reiniciando)
            return;

        if (!other.CompareTag("Player"))
            return;

        reiniciando = true;

        if (gameOver != null)
            gameOver.SetActive(true);

        Invoke(nameof(ReaparecerJugador), tiempoGameOver);
    }

    private void ReaparecerJugador()
    {
        GameObject jugador = GameObject.FindGameObjectWithTag("Player");

        if (jugador != null)
        {
            jugador.transform.position = posicionInicial;

            Rigidbody2D rb = jugador.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
            }
        }

        if (gameOver != null)
            gameOver.SetActive(false);

        reiniciando = false;
    }
}