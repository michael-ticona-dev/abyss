using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float speed = 5f;

    [Header("Salto")]
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private int saltosMaximos = 2;

    [Header("Referencias")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform puntoInicio;
    [SerializeField] private GameObject efectoCaida;

    private PlayerInput playerInput;
    private PlayerAudio playerAudio;

    private bool estaEnElSuelo = false;

    private int saltosRealizados;

    private bool estabaSubiendo = false;
    private bool estabaCayendo = false;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerAudio = GetComponent<PlayerAudio>();

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        if (animator == null)
            animator = GetComponent<Animator>();

        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (puntoInicio == null)
        {
            GameObject nuevoPunto = new GameObject("PuntoInicio");

            nuevoPunto.transform.position = transform.position;

            puntoInicio = nuevoPunto.transform;
        }

        saltosRealizados = 0;
    }

    private void Update()
    {
        Saltar();

        Animaciones();
        Flip();

        AudioCorrer();
        AudioCaer();
    }

    private void FixedUpdate()
    {
        if (rb == null || playerInput == null)
            return;

        float movimiento = playerInput.Movimiento;

        rb.linearVelocity = new Vector2(
            movimiento * speed,
            rb.linearVelocity.y
        );
    }

    // =========================
    // SALTO
    // =========================

    private void Saltar()
    {
        if (!playerInput.SaltoPresionado)
            return;

        if (saltosRealizados >= saltosMaximos)
            return;

        rb.linearVelocity = new Vector2(
            rb.linearVelocity.x,
            jumpForce
        );

        saltosRealizados++;

        estaEnElSuelo = false;

        estabaSubiendo = true;
        estabaCayendo = false;

        if (animator != null)
        {
            animator.SetBool("isJump", true);
            animator.SetBool("isRunning", false);
        }

        if (playerAudio != null)
        {
            playerAudio.DetenerCorrer();
            playerAudio.ReproducirSalto();
        }
    }

    // =========================
    // ANIMACIONES
    // =========================

    private void Animaciones()
    {
        if (animator == null || playerInput == null)
            return;

        // Está en el aire
        if (!estaEnElSuelo)
        {
            animator.SetBool("isJump", true);
            animator.SetBool("isRunning", false);

            return;
        }

        // Está en el suelo
        animator.SetBool("isJump", false);

        bool estaCorriendo =
            Mathf.Abs(playerInput.Movimiento) > 0.01f;

        animator.SetBool(
            "isRunning",
            estaCorriendo
        );
    }

    // =========================
    // GIRAR PERSONAJE
    // =========================

    private void Flip()
    {
        if (spriteRenderer == null || playerInput == null)
            return;

        float movimiento = playerInput.Movimiento;

        if (movimiento > 0.01f)
        {
            spriteRenderer.flipX = true;
        }
        else if (movimiento < -0.01f)
        {
            spriteRenderer.flipX = false;
        }
    }

    // =========================
    // AUDIO DE CORRER
    // =========================

    private void AudioCorrer()
    {
        if (playerAudio == null || playerInput == null)
            return;

        if (!estaEnElSuelo)
        {
            playerAudio.DetenerCorrer();
            return;
        }

        if (Mathf.Abs(playerInput.Movimiento) > 0.01f)
        {
            playerAudio.ReproducirCorrer();
        }
        else
        {
            playerAudio.DetenerCorrer();
        }
    }

    // =========================
    // DETECTAR CAÍDA
    // =========================

    private void AudioCaer()
    {
        if (rb == null)
            return;

        if (estaEnElSuelo)
            return;

        // Está subiendo
        if (rb.linearVelocity.y > 0.1f)
        {
            estabaSubiendo = true;
            estabaCayendo = false;

            return;
        }

        // Empieza a caer
        if (estabaSubiendo && rb.linearVelocity.y < -0.1f)
        {
            estabaSubiendo = false;
            estabaCayendo = true;
        }
    }

    // =========================
    // COLISIÓN CON EL SUELO
    // =========================

    private void OnCollisionEnter2D(Collision2D collision)
    {
        bool tocoSuelo = false;

        foreach (ContactPoint2D contacto in collision.contacts)
        {
            if (contacto.normal.y > 0.5f)
            {
                tocoSuelo = true;
                break;
            }
        }

        if (!tocoSuelo)
            return;

        // Solo reproducir caída si realmente estaba cayendo
        if (!estaEnElSuelo && estabaCayendo)
        {
            if (playerAudio != null)
            {
                playerAudio.ReproducirCaer();
            }

            MostrarEfectoCaida();
        }

        estaEnElSuelo = true;

        estabaSubiendo = false;
        estabaCayendo = false;

        saltosRealizados = 0;

        if (animator != null)
        {
            animator.SetBool("isJump", false);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        bool estaSobreSuelo = false;

        foreach (ContactPoint2D contacto in collision.contacts)
        {
            if (contacto.normal.y > 0.5f)
            {
                estaSobreSuelo = true;
                break;
            }
        }

        if (estaSobreSuelo)
        {
            estaEnElSuelo = true;
        }
        else
        {
            estaEnElSuelo = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        estaEnElSuelo = false;
    }

    // =========================
    // EFECTO DE CAÍDA
    // =========================

    private void MostrarEfectoCaida()
    {
        if (efectoCaida == null)
            return;

        efectoCaida.SetActive(true);

        CancelInvoke(nameof(OcultarEfectoCaida));

        Invoke(
            nameof(OcultarEfectoCaida),
            0.3f
        );
    }

    private void OcultarEfectoCaida()
    {
        if (efectoCaida != null)
        {
            efectoCaida.SetActive(false);
        }
    }
}