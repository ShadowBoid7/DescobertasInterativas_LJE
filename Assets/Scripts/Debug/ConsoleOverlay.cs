using TMPro;
using UnityEngine;
using System.Text;

public class ConsoleOverlay : MonoBehaviour
{
    [SerializeField] private TMP_Text logText;

    private static ConsoleOverlay instance;
    private readonly StringBuilder builder = new StringBuilder();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(transform.root.gameObject);

    }

    public static void Log(string msg)
    {
        if (instance == null) return;
        instance.Show(msg);
    }

    private void Show(string msg)
    {
        builder.Clear();
        builder.Append(msg);

        if (logText != null)
            logText.text = builder.ToString();
    }
}



