using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;

    // RUN
    public Sprite[] runLeft;
    public Sprite[] runRight;
    public Sprite[] runUp;
    public Sprite[] runDown;

    // IDLE
    public Sprite idleLeft;
    public Sprite idleRight;
    public Sprite idleUp;
    public Sprite idleDown;

    public float animationSpeed = 0.1f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private Vector2 movimiento;

    private float animationTimer;
    private int animationFrame;

    // Empezamos mirando hacia abajo
    private string ultimaDireccion = "down";

    // ATAQUE 1
    public Sprite[] attack1Up;
    public Sprite[] attack1Down;
    public Sprite[] attack1Left;
    public Sprite[] attack1Right;

    // ATAQUE 2
    public Sprite[] attack2Up;
    public Sprite[] attack2Down;
    public Sprite[] attack2Left;
    public Sprite[] attack2Right;

    public float attackSpeed = 0.15f;
    public float attackMoveSpeed = 10f;

    private bool atacando = false;
    private int attackFrame = 0;
    private float attackTimer = 0f;
    private int tipoAtaque = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // =====================================================
        // ATAQUES
        // =====================================================

        if (Mouse.current.leftButton.wasPressedThisFrame && !atacando)
        {
            atacando = true;
            tipoAtaque = 1;
            attackFrame = 0;
            attackTimer = 0f;
        }

        if (Mouse.current.rightButton.wasPressedThisFrame && !atacando)
        {
            atacando = true;
            tipoAtaque = 2;
            attackFrame = 0;
            attackTimer = 0f;
        }

        // =====================================================
        // SI ESTÁ ATACANDO
        // =====================================================

        if (atacando)
        {
            movimiento = Vector2.zero;

            if (Keyboard.current.wKey.isPressed)
                movimiento.y += 1;

            if (Keyboard.current.sKey.isPressed)
                movimiento.y -= 1;

            if (Keyboard.current.aKey.isPressed)
                movimiento.x -= 1;

            if (Keyboard.current.dKey.isPressed)
                movimiento.x += 1;

            movimiento = movimiento.normalized;

            Sprite[] ataqueActual = null;

            // ATAQUE 1
            if (tipoAtaque == 1)
            {
                if (ultimaDireccion == "up")
                    ataqueActual = attack1Up;

                else if (ultimaDireccion == "down")
                    ataqueActual = attack1Down;

                else if (ultimaDireccion == "left")
                    ataqueActual = attack1Left;

                else if (ultimaDireccion == "right")
                    ataqueActual = attack1Right;
            }

            // ATAQUE 2
            else if (tipoAtaque == 2)
            {
                if (ultimaDireccion == "up")
                    ataqueActual = attack2Up;

                else if (ultimaDireccion == "down")
                    ataqueActual = attack2Down;

                else if (ultimaDireccion == "left")
                    ataqueActual = attack2Left;

                else if (ultimaDireccion == "right")
                    ataqueActual = attack2Right;
            }

            // REPRODUCIR ATAQUE
            if (ataqueActual != null && ataqueActual.Length > 0)
            {
                attackTimer += Time.deltaTime;

                if (attackTimer >= attackSpeed)
                {
                    attackTimer = 0f;

                    if (attackFrame < ataqueActual.Length)
                    {
                        spriteRenderer.sprite = ataqueActual[attackFrame];

                        attackFrame++;
                    }

                    // TERMINÓ
                    if (attackFrame >= ataqueActual.Length)
                    {
                        atacando = false;
                        attackFrame = 0;
                        attackTimer = 0f;
                    }
                }
            }

            // MUY IMPORTANTE:
            // No ejecutar RUN ni IDLE mientras ataca
            return;
        }

        // =====================================================
        // MOVIMIENTO
        // =====================================================

        movimiento = Vector2.zero;

        if (Keyboard.current.wKey.isPressed)
            movimiento.y += 1;

        if (Keyboard.current.sKey.isPressed)
            movimiento.y -= 1;

        if (Keyboard.current.aKey.isPressed)
            movimiento.x -= 1;

        if (Keyboard.current.dKey.isPressed)
            movimiento.x += 1;

        movimiento = movimiento.normalized;

        // =====================================================
        // ANIMACIÓN IZQUIERDA
        // =====================================================

        if (Keyboard.current.aKey.isPressed)
        {
            if (ultimaDireccion != "left")
            {
                animationFrame = 0;
                animationTimer = 0f;
                ultimaDireccion = "left";
            }

            animationTimer += Time.deltaTime;

            if (animationTimer >= animationSpeed)
            {
                animationTimer = 0f;

                animationFrame++;

                if (animationFrame >= runLeft.Length)
                    animationFrame = 0;

                spriteRenderer.sprite = runLeft[animationFrame];
            }
        }

        // =====================================================
        // ANIMACIÓN DERECHA
        // =====================================================

        if (Keyboard.current.dKey.isPressed)
        {
            if (ultimaDireccion != "right")
            {
                animationFrame = 0;
                animationTimer = 0f;
                ultimaDireccion = "right";
            }

            animationTimer += Time.deltaTime;

            if (animationTimer >= animationSpeed)
            {
                animationTimer = 0f;

                animationFrame++;

                if (animationFrame >= runRight.Length)
                    animationFrame = 0;

                spriteRenderer.sprite = runRight[animationFrame];
            }
        }

        // =====================================================
        // ANIMACIÓN ARRIBA
        // =====================================================

        if (Keyboard.current.wKey.isPressed)
        {
            if (ultimaDireccion != "up")
            {
                animationFrame = 0;
                animationTimer = 0f;
                ultimaDireccion = "up";
            }

            animationTimer += Time.deltaTime;

            if (animationTimer >= animationSpeed)
            {
                animationTimer = 0f;

                animationFrame++;

                if (animationFrame >= runUp.Length)
                    animationFrame = 0;

                spriteRenderer.sprite = runUp[animationFrame];
            }
        }

        // =====================================================
        // ANIMACIÓN ABAJO
        // =====================================================

        if (Keyboard.current.sKey.isPressed)
        {
            if (ultimaDireccion != "down")
            {
                animationFrame = 0;
                animationTimer = 0f;
                ultimaDireccion = "down";
            }

            animationTimer += Time.deltaTime;

            if (animationTimer >= animationSpeed)
            {
                animationTimer = 0f;

                animationFrame++;

                if (animationFrame >= runDown.Length)
                    animationFrame = 0;

                spriteRenderer.sprite = runDown[animationFrame];
            }
        }

        // =====================================================
        // IDLE
        // =====================================================

        if (!Keyboard.current.wKey.isPressed &&
            !Keyboard.current.sKey.isPressed &&
            !Keyboard.current.aKey.isPressed &&
            !Keyboard.current.dKey.isPressed)
        {
            animationTimer = 0f;
            animationFrame = 0;

            if (ultimaDireccion == "up")
                spriteRenderer.sprite = idleUp;

            else if (ultimaDireccion == "down")
                spriteRenderer.sprite = idleDown;

            else if (ultimaDireccion == "left")
                spriteRenderer.sprite = idleLeft;

            else if (ultimaDireccion == "right")
                spriteRenderer.sprite = idleRight;
        }
    }

    void FixedUpdate()
    {
        if (atacando)
        {
            rb.linearVelocity = movimiento * attackMoveSpeed;
        }
        else
        {
            rb.linearVelocity = movimiento * speed;
        }
    }
}
