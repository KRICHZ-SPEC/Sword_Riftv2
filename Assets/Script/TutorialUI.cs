using System.Collections;
using UnityEngine;
using TMPro;

public class TutorialUI : MonoBehaviour
{
    public TextMeshProUGUI tmpText;
    public CanvasGroup canvasGroup;

    public float fadeDuration = 0.5f;

    void Awake()
    {
        canvasGroup.alpha = 0;
    }

    public IEnumerator ShowText(string message, float displayTime = 2f)
    {
        tmpText.text = message;
        
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }

        yield return new WaitForSeconds(displayTime);
        
        t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1, 0, t / fadeDuration);
            yield return null;
        }
    }
}