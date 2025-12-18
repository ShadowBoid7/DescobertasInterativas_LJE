using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cameraTransform; // arrasta aqui a Main Camera

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 12f;
    [SerializeField] private float gravity = -9.81f;

    private CharacterController _controller;
    private Vector2 _moveInput;
    private float _verticalVelocity;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();

        // fallback: se não ligares no inspector, tenta buscar a Main Camera
        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        // --- 1) calcular direção relativa à câmara ---
        Vector3 moveWorld = Vector3.zero;

        if (cameraTransform != null)
        {
            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;

            // ignorar inclinação vertical
            camForward.y = 0f;
            camRight.y = 0f;

            camForward.Normalize();
            camRight.Normalize();

            moveWorld = camForward * _moveInput.y + camRight * _moveInput.x;
        }
        else
        {
            // fallback se não houver camera
            moveWorld = new Vector3(_moveInput.x, 0f, _moveInput.y);
        }

        // --- 2) rodar player na direção do movimento ---
        if (moveWorld.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveWorld);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        }

        // --- 3) gravidade ---
        if (_controller.isGrounded && _verticalVelocity < 0f)
            _verticalVelocity = -2f; // pequeno "stick to ground" (melhor que 0)

        _verticalVelocity += gravity * Time.deltaTime;

        // --- 4) mover ---
        Vector3 velocity = moveWorld * moveSpeed;
        velocity.y = _verticalVelocity;

        _controller.Move(velocity * Time.deltaTime);
    }

    // INPUT CALLBACKS (PlayerInput -> Invoke Unity Events)
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




