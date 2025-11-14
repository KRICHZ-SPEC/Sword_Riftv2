using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialUI : MonoBehaviour
{
    [Header("UI Components")]
    public Text uiText;                     
    public TextMeshProUGUI tmpText;         
    public CanvasGroup canvasGroup;        

    [Header("Settings")]
    public float fadeDuration = 0.5f;      
    public float defaultDisplayTime = 3f;  

    private Coroutine currentRoutine;

    void Awake()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup != null)
            canvasGroup.alpha = 0f;
    }

    public void Show(string message, float duration = -1f)
    {
        if (duration <= 0) duration = defaultDisplayTime;
        
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(ShowMessageRoutine(message, duration));
    }

    IEnumerator ShowMessageRoutine(string message, float duration)
    {
        SetText(message);
        
        yield return StartCoroutine(FadeCanvas(0, 1, fadeDuration));
        
        yield return new WaitForSeconds(duration);
        
        yield return StartCoroutine(FadeCanvas(1, 0, fadeDuration));
    }

    IEnumerator FadeCanvas(float from, float to, float duration)
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float normalized = t / duration;
            if (canvasGroup != null)
                canvasGroup.alpha = Mathf.Lerp(from, to, normalized);
            yield return null;
        }

        if (canvasGroup != null)
            canvasGroup.alpha = to;
    }

    private void SetText(string message)
    {
        if (uiText != null)
            uiText.text = message;
        if (tmpText != null)
            tmpText.text = message;
    }

    public void SetInstant(string message)
    {
        StopAllCoroutines();
        SetText(message);
        if (canvasGroup != null)
            canvasGroup.alpha = 1f;
    }

    public void HideInstant()
    {
        StopAllCoroutines();
        if (canvasGroup != null)
            canvasGroup.alpha = 0f;
    }
}