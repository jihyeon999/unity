using UnityEngine;
using TMPro;
using System.Collections;

public class GameMessageUI : MonoBehaviour
{
    public static GameMessageUI Instance;

    public TextMeshProUGUI messageText;
    public float showTime = 2f;

    private Coroutine messageCoroutine;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (messageText != null)
        {
            messageText.gameObject.SetActive(false);
        }
    }

    public void ShowMessage(string message)
    {
        if (messageText == null) return;

        if (messageCoroutine != null)
        {
            StopCoroutine(messageCoroutine);
        }

        messageCoroutine = StartCoroutine(ShowMessageRoutine(message));
    }

    IEnumerator ShowMessageRoutine(string message)
    {
        messageText.text = message;
        messageText.gameObject.SetActive(true);

        yield return new WaitForSeconds(showTime);

        messageText.gameObject.SetActive(false);
    }
}