using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float gravity = -9.81f;

    private CharacterController _controller;
    private Vector2 _moveInput;
    private float _verticalVelocity;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        // Converter input para direção no mundo (XZ)
        Vector3 move = new Vector3(_moveInput.x, 0f, _moveInput.y);

        if (move.sqrMagnitude > 0.001f)
        {
            // Rotacionar na direção do movimento
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        // Aplicar gravidade
        if (_controller.isGrounded)
        {
            _verticalVelocity = 0f;
        }
        _verticalVelocity += gravity * Time.deltaTime;

        Vector3 velocity = move * moveSpeed + Vector3.up * _verticalVelocity;

        _controller.Move(velocity * Time.deltaTime);
    }

    // ---- INPUT CALLBACKS ----

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
            ConsoleOverlay.Log("Interact acionado!");
    }

    public void OnAction1(InputAction.CallbackContext context)
    {
        if (context.performed)
            ConsoleOverlay.Log("Action1 acionado!");
    }
}


