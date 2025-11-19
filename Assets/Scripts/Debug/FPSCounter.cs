using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
    [SerializeField] private Text fpsText;
    [SerializeField] private float updateInterval = 0.5f;

    private float accumulated;
    private int frames;
    private float timeLeft;

    private void Start()
    {
        timeLeft = updateInterval;
    }

    private void Update()
    {
        timeLeft -= Time.deltaTime;
        accumulated += Time.timeScale / Time.deltaTime;
        frames++;

        if (timeLeft <= 0.0f)
        {
            float fps = accumulated / frames;
            if (fpsText != null)
            {
                fpsText.text = $"FPS: {fps:0}";
            }

            timeLeft = updateInterval;
            accumulated = 0f;
            frames = 0;
        }
    }
}

