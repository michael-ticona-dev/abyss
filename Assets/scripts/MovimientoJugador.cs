using UnityEngine;
using UnityEngine.InputSystem;

public class MovimientoJugador : MonoBehaviour
{
    public float speed = 5f;

    private Rigidbody2D rb;
    private Vector2 movimiento;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        movimiento = Vector2.zero;

        if (Keyboard.current.wKey.isPressed)
        {
            movimiento.y += 1;
        }
        if (Keyboard.current.sKey.isPressed)
        {
            movimiento.y -= 1;
        }
        if (Keyboard.current.aKey.isPressed)
        {
            movimiento.x -= 1;
        }
        if (Keyboard.current.dKey.isPressed)
        {
            movimiento.x += 1;
        }

        movimiento = movimiento.normalized;
    }

    void FixedUpdate() 
    {
        rb.MovePosition(
            rb.position + movimiento * speed * Time.fixedDeltaTime
        );
    }

    private GameObject objeto;

    void OnCollisionEnter2D(Collision2D collision)
    {
        objeto = collision.gameObject;
        Debug.Log("choque con algo" + objeto.name);
    }
}
