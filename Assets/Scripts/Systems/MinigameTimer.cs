using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class MinigameTimer : MonoBehaviour
{
    [SerializeField] private float duration = 20f;
    [SerializeField] private TMP_Text timerText;

    public UnityEvent OnTimeOver;
    public UnityEvent OnMinigameSuccess;

    private float _timeLeft;
    private bool _running;

    private void OnEnable()
    {
        _timeLeft = duration;
        _running = true;
    }

    private void Update()
    {
        if (!_running) return;

        _timeLeft -= Time.deltaTime;

        if (timerText != null)
            timerText.text = $"{_timeLeft:0.0}s";

        if (_timeLeft <= 0f)
        {
            _running = false;
            OnTimeOver?.Invoke();
        }
    }

    public void CompleteMinigame()
    {
        if (!_running) return;
        _running = false;
        OnMinigameSuccess?.Invoke();
    }
}

