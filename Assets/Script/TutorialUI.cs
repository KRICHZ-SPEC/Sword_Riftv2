using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    public TextMeshProUGUI tmpText;      
    public CanvasGroup canvasGroup;      
    public float fadeTime = 0.35f;

    void Reset()
    {
        // optional: try to auto-find
        tmpText = GetComponentInChildren<TextMeshProUGUI>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void Awake()
    {
        if (canvasGroup == null)
            canvasGroup = gameObject.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
        if (tmpText) tmpText.text = "";
    }

    public void ShowTextImmediate(string text)
    {
        if (tmpText) tmpText.text = text;
        canvasGroup.alpha = 1f;
    }

    public void HideImmediate()
    {
        if (tmpText) tmpText.text = "";
        canvasGroup.alpha = 0f;
    }

    public Coroutine ShowText(string text, float displaySeconds)
    {
        return StartCoroutine(DoShowText(text, displaySeconds));
    }

    IEnumerator DoShowText(string text, float displaySeconds)
    {
        if (tmpText) tmpText.text = text;
        yield return StartCoroutine(Fade(0f, 1f, fadeTime));
        yield return new WaitForSeconds(displaySeconds);
        yield return StartCoroutine(Fade(1f, 0f, fadeTime));
        if (tmpText) tmpText.text = "";
    }

    IEnumerator Fade(float from, float to, float time)
    {
        float t = 0f;
        while (t < time)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(from, to, t / time);
            canvasGroup.alpha = a;
            yield return null;
        }
        canvasGroup.alpha = to;
    }
}