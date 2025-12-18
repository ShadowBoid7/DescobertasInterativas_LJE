using UnityEngine;
using UnityEngine.InputSystem;

public class SailsRigController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private WindSystem windSystem;
    [SerializeField] private Transform playerTransform;

    [Header("Pivot (mast axis)")]
    [SerializeField] private Transform sailsPivot; // <-- arrasta o SailsPivot aqui

    [Header("Parts to rotate (sails + yards)")]
    [SerializeField] private Transform[] sailParts;

    [Header("Rotation")]
    [SerializeField, Range(0f, 360f)] private float sailDirection = 0f;
    [SerializeField] private float windInfluenceSpeed = 20f;
    [SerializeField] private float playerRotateSpeed = 60f;

    [Header("Interaction")]
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private float alignTolerance = 10f;

    private bool _isAligned;

    // Guardar estado base relativo ao pivot
    private Vector3[] _baseLocalPos;
    private Quaternion[] _baseLocalRot;

    private void Awake()
    {
        CacheBaseState();
    }

    private void CacheBaseState()
    {
        if (sailsPivot == null || sailParts == null || sailParts.Length == 0)
            return;

        _baseLocalPos = new Vector3[sailParts.Length];
        _baseLocalRot = new Quaternion[sailParts.Length];

        for (int i = 0; i < sailParts.Length; i++)
        {
            if (sailParts[i] == null) continue;

            // Guardar posição/rotação no espaço do pivot (isto é o segredo)
            _baseLocalPos[i] = sailsPivot.InverseTransformPoint(sailParts[i].position);
            _baseLocalRot[i] = Quaternion.Inverse(sailsPivot.rotation) * sailParts[i].rotation;
        }
    }

    private bool _wasInRange;

    private void Update()
    {
        if (windSystem == null || sailsPivot == null) return;

        float target = windSystem.windDirection;

        // vento empurra em direção ao vento
        float windDelta = Mathf.DeltaAngle(sailDirection, target);
        sailDirection += Mathf.Sign(windDelta) * windInfluenceSpeed * Time.deltaTime;

        // input do jogador (Q/E) só perto
        if (IsPlayerInRange())
        {
            var kb = Keyboard.current;
            if (kb != null)
            {
                if (kb.qKey.isPressed) sailDirection -= playerRotateSpeed * Time.deltaTime;
                if (kb.eKey.isPressed) sailDirection += playerRotateSpeed * Time.deltaTime;
            }
        }

        sailDirection = (sailDirection + 360f) % 360f;

        ApplyRotationAroundPivot(sailDirection);

        // alinhamento
        float diff = Mathf.Abs(Mathf.DeltaAngle(sailDirection, target));
        _isAligned = diff <= alignTolerance;

        bool inRange = IsPlayerInRange();
        if (inRange && !_wasInRange) ConsoleOverlay.Log("Perto das velas: usa Q/E.");
        _wasInRange = inRange;

        if (!inRange) return;

        // Q/E aqui
    }

    private void ApplyRotationAroundPivot(float yDegrees)
    {
        if (_baseLocalPos == null || _baseLocalRot == null) CacheBaseState();
        if (_baseLocalPos == null || _baseLocalRot == null) return;

        // rotação em world space em torno do eixo do mastro (up do pivot)
        Quaternion deltaWorld = Quaternion.AngleAxis(yDegrees, sailsPivot.up);

        for (int i = 0; i < sailParts.Length; i++)
        {
            var part = sailParts[i];
            if (part == null) continue;

            // posição: pivot + rotação aplicada ao vetor base
            Vector3 worldPos = sailsPivot.TransformPoint(deltaWorld * _baseLocalPos[i]);
            part.position = worldPos;

            // rotação: rot do pivot + delta + base rot
            Quaternion worldRot = deltaWorld * (sailsPivot.rotation * _baseLocalRot[i]);
            part.rotation = worldRot;
        }
    }

    private bool IsPlayerInRange()
    {
        if (playerTransform == null) return true; // para teste
        return Vector3.Distance(playerTransform.position, sailsPivot.position) <= interactionDistance;
    }

    public bool AreSailsAligned() => _isAligned;

    private void OnDrawGizmosSelected()
    {
        if (sailsPivot == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(sailsPivot.position, interactionDistance);
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(sailsPivot.position, sailsPivot.position + sailsPivot.up * 2f);
    }
}



