using UnityEngine;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{
    [Header("Canvas")]
    [SerializeField] private GameObject canvasMenu;
    [SerializeField] private GameObject canvasSonido;

    private bool menuAbierto = false;

    private void Start()
    {
        canvasMenu.SetActive(false);
        canvasSonido.SetActive(false);
    }

    private void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            AlternarMenu();
        }
    }

    private void AlternarMenu()
    {
        menuAbierto = !menuAbierto;

        canvasMenu.SetActive(menuAbierto);
        canvasSonido.SetActive(false);

        if (menuAbierto)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    public void AbrirSonido()
    {
        canvasMenu.SetActive(false);
        canvasSonido.SetActive(true);
    }

    public void VolverMenu()
    {
        canvasSonido.SetActive(false);
        canvasMenu.SetActive(true);
    }
}