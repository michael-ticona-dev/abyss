using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    [SerializeField] private Transform jugador;

    [SerializeField] private float suavizado = 5f;

    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f);

    private void LateUpdate()
    {
        if (jugador == null)
            return;

        Vector3 posicionObjetivo = new Vector3(
            jugador.position.x + offset.x,
            transform.position.y,
            offset.z
        );

        transform.position = Vector3.Lerp(
            transform.position,
            posicionObjetivo,
            suavizado * Time.deltaTime
        );
    }
}