using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
[Header("Movimiento")]
[SerializeField] private float velocidad = 2f;
[SerializeField] private float distanciaPatrulla = 3f;

[Header("Jugador")]
[SerializeField] private Transform jugador;
[SerializeField] private float distanciaPerseguir = 5f;
[SerializeField] private float distanciaAtaque = 1.2f;

[Header("Sprites")]
[SerializeField] private Sprite[] spritesIdle;
[SerializeField] private Sprite[] spritesRun;
[SerializeField] private Sprite[] spritesAttack;

[Header("Animacion")]
[SerializeField] private float velocidadAnimacion = 0.1f;

[Header("Ataque cuerpo a cuerpo")]
[SerializeField] private float tiempoEntreAtaques = 1f;
[SerializeField] private float duracionAtaque = 0.5f;

[Header("Bola de fuego")]
[SerializeField] private GameObject bolaDeFuego;
[SerializeField] private Sprite spriteBolaDeFuego;
[SerializeField] private Transform puntoDisparo;
[SerializeField] private float distanciaAtaqueDistancia = 3f;
[SerializeField] private float tiempoEntreBolas = 2f;
[SerializeField] private float velocidadBola = 6f;
[SerializeField] private float tiempoVidaBola = 5f;

[Header("Canvas de dano")]
[SerializeField] private GameObject canvasDanio;

[Header("Referencias")]
[SerializeField] private SpriteRenderer spriteRenderer;
[SerializeField] private Rigidbody2D rb;

private Vector3 posicionInicial;
private bool moviendoDerecha = true;
private bool estaAtacando;
private bool persiguiendo;

private float temporizadorAtaque;
private float temporizadorBola;
private float temporizadorSprite;
private int indiceSprite;

private void Awake()
{
    if (spriteRenderer == null)
        spriteRenderer = GetComponent<SpriteRenderer>();

    if (rb == null)
        rb = GetComponent<Rigidbody2D>();
}

private void Start()
{
    posicionInicial = transform.position;

    if (canvasDanio != null)
        canvasDanio.SetActive(false);
}

private void Update()
{
    if (temporizadorAtaque > 0f)
        temporizadorAtaque -= Time.deltaTime;

    if (temporizadorBola > 0f)
        temporizadorBola -= Time.deltaTime;

    ComprobarJugador();
    ComprobarAtaqueDistancia();
    ActualizarSprites();
}

private void FixedUpdate()
{
    if (rb == null)
        return;

    if (estaAtacando)
    {
        DetenerMovimiento();
        return;
    }

    if (persiguiendo)
        PerseguirJugador();
    else
        Patrullar();
}

private void ComprobarJugador()
{
    if (jugador == null)
    {
        persiguiendo = false;
        return;
    }

    float distancia = Mathf.Abs(
        jugador.position.x - transform.position.x
    );

    if (distancia <= distanciaAtaque)
    {
        persiguiendo = true;
        DetenerMovimiento();
        MirarAlJugador();

        if (temporizadorAtaque <= 0f)
            Atacar();

        return;
    }

    persiguiendo = distancia <= distanciaPerseguir;
}

private void ComprobarAtaqueDistancia()
{
    if (jugador == null)
        return;

    float distancia = Mathf.Abs(
        jugador.position.x - transform.position.x
    );

    if (distancia <= distanciaAtaqueDistancia &&
        distancia > distanciaAtaque)
    {
        persiguiendo = true;
        DetenerMovimiento();
        MirarAlJugador();

        if (temporizadorBola <= 0f)
            LanzarBolaDeFuego();
    }
}

private void PerseguirJugador()
{
    if (jugador == null || rb == null)
        return;

    float distancia = Mathf.Abs(
        jugador.position.x - transform.position.x
    );

    if (distancia <= distanciaAtaqueDistancia)
    {
        DetenerMovimiento();
        MirarAlJugador();
        return;
    }

    float direccion =
        jugador.position.x - transform.position.x;

    float movimiento = Mathf.Sign(direccion);

    rb.linearVelocity = new Vector2(
        movimiento * velocidad,
        rb.linearVelocity.y
    );

    spriteRenderer.flipX = movimiento < 0f;
}

private void Patrullar()
{
    if (rb == null)
        return;

    float limiteIzquierdo =
        posicionInicial.x - distanciaPatrulla;

    float limiteDerecho =
        posicionInicial.x + distanciaPatrulla;

    float movimiento = moviendoDerecha ? 1f : -1f;

    rb.linearVelocity = new Vector2(
        movimiento * velocidad,
        rb.linearVelocity.y
    );

    if (spriteRenderer != null)
        spriteRenderer.flipX = !moviendoDerecha;

    if (moviendoDerecha &&
        transform.position.x >= limiteDerecho)
    {
        CambiarDireccion();
    }
    else if (!moviendoDerecha &&
             transform.position.x <= limiteIzquierdo)
    {
        CambiarDireccion();
    }
}

private void Atacar()
{
    if (estaAtacando)
        return;

    estaAtacando = true;
    temporizadorAtaque = tiempoEntreAtaques;
    indiceSprite = 0;
    temporizadorSprite = 0f;

    DetenerMovimiento();
    MirarAlJugador();

    Invoke(nameof(FinalizarAtaque), duracionAtaque);
}

private void FinalizarAtaque()
{
    estaAtacando = false;
    indiceSprite = 0;
    temporizadorSprite = 0f;
}

private void LanzarBolaDeFuego()
{
    if (bolaDeFuego == null || jugador == null)
        return;

    temporizadorBola = tiempoEntreBolas;

    Vector3 posicion = puntoDisparo != null
        ? puntoDisparo.position
        : transform.position;

    GameObject bola = Instantiate(
        bolaDeFuego,
        posicion,
        Quaternion.identity
    );

    SpriteRenderer rendererBola =
        bola.GetComponent<SpriteRenderer>();

    if (rendererBola != null && spriteBolaDeFuego != null)
        rendererBola.sprite = spriteBolaDeFuego;

    BolaDeFuego scriptBola =
        bola.GetComponent<BolaDeFuego>();

    if (scriptBola != null)
    {
        scriptBola.Configurar(
            jugador,
            canvasDanio,
            velocidadBola
        );
    }
    else
    {
        Debug.LogError(
            "El Prefab de la bola necesita BolaDeFuego.cs."
        );
    }

    Destroy(bola, tiempoVidaBola);
}

private void DetenerMovimiento()
{
    if (rb == null)
        return;

    rb.linearVelocity = new Vector2(
        0f,
        rb.linearVelocity.y
    );
}

private void MirarAlJugador()
{
    if (jugador == null || spriteRenderer == null)
        return;

    spriteRenderer.flipX =
        jugador.position.x < transform.position.x;
}

private void ActualizarSprites()
{
    if (spriteRenderer == null || rb == null)
        return;

    Sprite[] spritesActuales;

    if (estaAtacando)
        spritesActuales = spritesAttack;
    else if (Mathf.Abs(rb.linearVelocity.x) > 0.01f)
        spritesActuales = spritesRun;
    else
        spritesActuales = spritesIdle;

    if (spritesActuales == null ||
        spritesActuales.Length == 0)
        return;

    temporizadorSprite += Time.deltaTime;

    if (temporizadorSprite >= velocidadAnimacion)
    {
        temporizadorSprite = 0f;
        indiceSprite++;

        if (indiceSprite >= spritesActuales.Length)
            indiceSprite = 0;

        spriteRenderer.sprite =
            spritesActuales[indiceSprite];
    }
}

private void CambiarDireccion()
{
    moviendoDerecha = !moviendoDerecha;
    indiceSprite = 0;

    if (spriteRenderer != null)
        spriteRenderer.flipX = !moviendoDerecha;
}


}
