using UnityEngine;
using UnityEngine.InputSystem;

public class CameraZoom2D : MonoBehaviour
{
    [Header("Configuración de Zoom")]
    public float zoomSpeed = 5f;
    public float minZoom = 2f;
    public float maxZoom = 10f;

    private Camera cam;
    private float targetZoom;

    void Start()
    {
        cam = GetComponent<Camera>();

        // Zoom inicial
        targetZoom = 8f;
        cam.orthographicSize = targetZoom;
    }

    void Update()
    {
        if (Keyboard.current != null)
        {
            // Acercar (+)
            if (Keyboard.current.equalsKey.isPressed ||
                Keyboard.current.numpadPlusKey.isPressed)
            {
                targetZoom -= zoomSpeed * Time.deltaTime;
            }

            // Alejar (-)
            if (Keyboard.current.minusKey.isPressed ||
                Keyboard.current.numpadMinusKey.isPressed)
            {
                targetZoom += zoomSpeed * Time.deltaTime;
            }
        }

        if (Mouse.current != null)
        {
            float scroll = Mouse.current.scroll.ReadValue().y;

            if (scroll != 0)
            {
                targetZoom -= Mathf.Sign(scroll) * zoomSpeed * 0.5f;
            }
        }

        targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);

        cam.orthographicSize = Mathf.Lerp(
            cam.orthographicSize,
            targetZoom,
            Time.deltaTime * 10f
        );
    }
}