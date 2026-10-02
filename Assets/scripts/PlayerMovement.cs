using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float speed = 5f;

    [Header("Salto")]
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private int saltosMaximos = 2;

    [Header("Ataque")]
    [SerializeField] private float tiempoAtaque = 0.6f;

    [Header("Referencias")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform puntoInicio;
    [SerializeField] private GameObject efectoCaida;

    private PlayerInput playerInput;
    private PlayerAudio playerAudio;

    private bool estaEnElSuelo = false;
    private bool estaAtacando = false;

    private float temporizadorAtaque;
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
        Atacar();
        ActualizarAtaque();

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

        if (estaAtacando)
        {
            rb.linearVelocity = new Vector2(
                0f,
                rb.linearVelocity.y
            );

            return;
        }

        rb.linearVelocity = new Vector2(
            movimiento * speed,
            rb.linearVelocity.y
        );
    }

    private void Saltar()
    {
        if (estaAtacando)
            return;

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

    private void Atacar()
    {
        if (estaAtacando)
            return;

        if (!estaEnElSuelo)
            return;

        if (!playerInput.AtaquePresionado)
            return;

        estaAtacando = true;
        temporizadorAtaque = tiempoAtaque;

        if (animator != null)
        {
            animator.SetBool("isAttack", true);
            animator.SetBool("isRunning", false);
            animator.SetBool("isJump", false);
        }

        if (playerAudio != null)
        {
            playerAudio.DetenerCorrer();
            playerAudio.ReproducirAtaque();
        }
    }

    private void ActualizarAtaque()
    {
        if (!estaAtacando)
            return;

        temporizadorAtaque -= Time.deltaTime;

        if (temporizadorAtaque <= 0f)
        {
            estaAtacando = false;

            if (animator != null)
                animator.SetBool("isAttack", false);
        }
    }

    private void Animaciones()
    {
        if (animator == null || playerInput == null)
            return;

        if (estaAtacando)
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isJump", false);
            return;
        }

        if (!estaEnElSuelo)
        {
            animator.SetBool("isJump", true);
            animator.SetBool("isRunning", false);
            return;
        }

        animator.SetBool("isJump", false);

        bool estaCorriendo =
            Mathf.Abs(playerInput.Movimiento) > 0.01f;

        animator.SetBool("isRunning", estaCorriendo);
    }

    private void Flip()
    {
        if (spriteRenderer == null || playerInput == null)
            return;

        if (playerInput.Movimiento > 0.01f)
        {
            spriteRenderer.flipX = true;
        }
        else if (playerInput.Movimiento < -0.01f)
        {
            spriteRenderer.flipX = false;
        }
    }

    private void AudioCorrer()
    {
        if (playerAudio == null || playerInput == null)
            return;

        if (estaAtacando || !estaEnElSuelo)
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

    private void AudioCaer()
    {
        if (rb == null)
            return;

        if (estaEnElSuelo)
            return;

        if (rb.linearVelocity.y > 0.1f)
        {
            estabaSubiendo = true;
            return;
        }

        if (estabaSubiendo && rb.linearVelocity.y < -0.1f)
        {
            estabaSubiendo = false;
            estabaCayendo = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        bool tocoSuelo = false;

        foreach (ContactPoint2D contacto in collision.contacts)
        {
            // Solo una superficie que apunta hacia arriba
            // puede considerarse suelo.
            if (contacto.normal.y > 0.5f)
            {
                tocoSuelo = true;
                break;
            }
        }

        if (!tocoSuelo)
            return;

        // Si estaba cayendo y acaba de tocar el suelo
        if (!estaEnElSuelo)
    {
        if (estabaCayendo && playerAudio != null)
            playerAudio.ReproducirCaer();

        MostrarEfectoCaida();
    }

        estaEnElSuelo = true;

        estabaSubiendo = false;
        estabaCayendo = false;

        saltosRealizados = 0;

        if (animator != null)
            animator.SetBool("isJump", false);
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

    private void MostrarEfectoCaida()
    {
        if (efectoCaida == null)
            return;

        efectoCaida.SetActive(true);

        CancelInvoke(nameof(OcultarEfectoCaida));
        Invoke(nameof(OcultarEfectoCaida), 0.3f);
    }

    private void OcultarEfectoCaida()
    {
        if (efectoCaida != null)
            efectoCaida.SetActive(false);
    }
}