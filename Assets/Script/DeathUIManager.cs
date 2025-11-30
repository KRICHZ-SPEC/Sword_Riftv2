using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeathUIManager : MonoBehaviour
{
    public static DeathUIManager Instance;

    [Header("UI References")]
    public CanvasGroup youDiedCanvas;      
    public CanvasGroup menuCanvas;         
    public float fadeDuration = 1.2f;

    private void Awake()
    {
        Instance = this;
        
        youDiedCanvas.alpha = 0f;
        menuCanvas.alpha = 0f;

        youDiedCanvas.gameObject.SetActive(false);
        menuCanvas.gameObject.SetActive(false);
    }
    
    public void TriggerDeath()
    {
        StartCoroutine(DeathSequence());
    }

    private IEnumerator DeathSequence()
    {
        youDiedCanvas.gameObject.SetActive(true);
        youDiedCanvas.alpha = 1;

        yield return new WaitForSeconds(2);

        menuCanvas.gameObject.SetActive(true);
        menuCanvas.alpha = 1;
    }

    private void ShowMenu()
    {
        menuCanvas.gameObject.SetActive(true);

        StartCoroutine(FadeInMenu());
    }

    private IEnumerator FadeInMenu()
    {
        float t = 0;
        float duration = 1f;

        while (t < duration)
        {
            t += Time.deltaTime;
            menuCanvas.alpha = Mathf.Lerp(0f, 1f, t / duration);
            yield return null;
        }
    }
}