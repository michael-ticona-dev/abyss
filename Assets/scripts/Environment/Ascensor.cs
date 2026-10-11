using UnityEngine;

public class Ascensor : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float distancia = 3f;
    [SerializeField] private float velocidad = 2f;

    private Vector3 posicionInicial;
    private Vector3 posicionFinal;

    private bool subiendo = true;

    private void Start()
    {
        posicionInicial = transform.position;

        posicionFinal = posicionInicial + Vector3.up * distancia;
    }

    private void Update()
    {
        Vector3 objetivo;

        if (subiendo)
        {
            objetivo = posicionFinal;
        }
        else
        {
            objetivo = posicionInicial;
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            objetivo,
            velocidad * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, objetivo) < 0.01f)
        {
            subiendo = !subiendo;
        }
    }
}