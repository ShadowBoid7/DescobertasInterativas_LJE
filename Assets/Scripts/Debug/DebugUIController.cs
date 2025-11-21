using UnityEngine;
using UnityEngine.InputSystem;

public class DebugUIController : MonoBehaviour
{
    [SerializeField] private GameObject debugUIRoot;

    private bool _visible = true;

    private void Awake()
    {
        if (debugUIRoot != null)
            debugUIRoot.SetActive(_visible);
    }

    public void OnDebugToggle(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _visible = !_visible;
            debugUIRoot.SetActive(_visible);
            Debug.Log("Debug UI: " + (_visible ? "Mostrado" : "Escondido"));
        }
    }
}

