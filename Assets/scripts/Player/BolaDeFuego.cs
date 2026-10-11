
using UnityEngine;

public class BolaDeFuego : MonoBehaviour
{
    [Header("Configuración")]
    private Transform jugador;
    private GameObject canvasDanio;
    private float velocidad = 5f;

    private bool impacto = false;

    public void Configurar(
        Transform objetivo,
        GameObject canvas,
        float velocidadMovimiento)
    {
        jugador = objetivo;
        canvasDanio = canvas;
        velocidad = velocidadMovimiento;
    }

    private void Update()
    {
        if (impacto || jugador == null)
            return;

        // Seguir al jugador
        Vector2 direccion =
            ((Vector2)jugador.position -
             (Vector2)transform.position).normalized;

        transform.position += (Vector3)(
            direccion * velocidad * Time.deltaTime
        );

        // Girar la bola de fuego
        transform.Rotate(0f, 0f, 720f * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (impacto || !other.CompareTag("Player"))
            return;

        impacto = true;

        // Mostrar el efecto visual de daño
        if (canvasDanio != null)
        {
            canvasDanio.SetActive(true);

            PlayerDamageFlash flash =
                canvasDanio.GetComponentInChildren<PlayerDamageFlash>(true);

            if (flash != null)
            {
                flash.MostrarFlash();
            }
            else
            {
                Debug.LogWarning(
                    "No se encontró PlayerDamageFlash en el Canvas."
                );
            }
        }
        else
        {
            Debug.LogWarning(
                "No se asignó el Canvas de daño en EnemyMovement."
            );
        }

        // Destruir la bola después del impacto
        Destroy(gameObject);
    }
}
