using UnityEngine;
using UnityEngine.InputSystem;

public class SailsController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private WindSystem windSystem;
    [SerializeField] private Transform playerTransform;

    [Header("Settings")]
    [SerializeField] private float sailDirection = 0f;          // ângulo atual da vela
    [SerializeField] private float windInfluenceSpeed = 20f;    // velocidade com que a vela vira sozinha em direção ao vento
    [SerializeField] private float playerRotateSpeed = 60f;     // velocidade girada pelo jogador
    [SerializeField] private float alignTolerance = 10f;
    [SerializeField] private float interactionDistance = 3f;    // distância máxima para o jogador poder mexer

    private bool _isAligned;

    private void Update()
    {
        if (windSystem == null) return;

        float target = windSystem.windDirection;

        // 1️⃣ Vento tenta alinhar a vela
        float windDelta = Mathf.DeltaAngle(sailDirection, target);
        sailDirection += Mathf.Sign(windDelta) * windInfluenceSpeed * Time.deltaTime;

        // 2️⃣ Só deixa o jogador mexer se estiver perto
        bool canControl = IsPlayerInRange();

        if (canControl)
        {
            var keyboard = Keyboard.current;
            if (keyboard != null)
            {
                if (keyboard.qKey.isPressed)
                    sailDirection -= playerRotateSpeed * Time.deltaTime;

                if (keyboard.eKey.isPressed)
                    sailDirection += playerRotateSpeed * Time.deltaTime;
            }
        }

        // Normalizar ângulo
        sailDirection = (sailDirection + 360f) % 360f;

        // 3️⃣ Aplicar rotação visual
        transform.rotation = Quaternion.Euler(0f, sailDirection, 0f);

        // 4️⃣ Verificar alinhamento
        float diff = Mathf.Abs(Mathf.DeltaAngle(sailDirection, target));
        bool wasAligned = _isAligned;
        _isAligned = diff <= alignTolerance;

        if (_isAligned && !wasAligned)
        {
            ConsoleOverlay.Log("Velas alinhadas com o vento!");
        }

        // Mensagem de debug opcional
        if (canControl)
        {
            ConsoleOverlay.Log("Estás perto da vela. Usa Q/E para ajustar.");
        }
    }

    private bool IsPlayerInRange()
    {
        if (playerTransform == null) return false;
        float dist = Vector3.Distance(playerTransform.position, transform.position);
        return dist <= interactionDistance;
    }

    public bool AreSailsAligned()
    {
        return _isAligned;
    }

    // Gizmo para veres o raio de interação na cena
    private void OnDrawGizmosSelected()
    {
        if (playerTransform == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionDistance);
    }
}



