using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public float Movimiento { get; private set; }

    private bool saltoPresionado;
    private bool ataquePresionado;

    public bool SaltoPresionado
    {
        get
        {
            if (saltoPresionado)
            {
                saltoPresionado = false;
                return true;
            }

            return false;
        }
    }

    public bool AtaquePresionado
    {
        get
        {
            if (ataquePresionado)
            {
                ataquePresionado = false;
                return true;
            }

            return false;
        }
    }

    private void Update()
    {
        Movimiento = 0f;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed ||
                Keyboard.current.leftArrowKey.isPressed)
            {
                Movimiento = -1f;
            }

            if (Keyboard.current.dKey.isPressed ||
                Keyboard.current.rightArrowKey.isPressed)
            {
                Movimiento = 1f;
            }

            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                saltoPresionado = true;
            }
        }

        if (Mouse.current != null)
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                ataquePresionado = true;
            }
        }
    }
}