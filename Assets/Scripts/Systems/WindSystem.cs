using UnityEngine;

public class WindSystem : MonoBehaviour
{
    [Header("Wind")]
    [Range(0f, 360f)]
    public float windDirection = 0f; // em graus
    public float changeInterval = 20f;
    public float randomChangeAmount = 45f;

    private float _timer;

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= changeInterval)
        {
            _timer = 0f;
            float delta = Random.Range(-randomChangeAmount, randomChangeAmount);
            windDirection = (windDirection + delta + 360f) % 360f;
            ConsoleOverlay.Log($"Direção do vento mudou para: {windDirection:0}°");
        }
    }
}

