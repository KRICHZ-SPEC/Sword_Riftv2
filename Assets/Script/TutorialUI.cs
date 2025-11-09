using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialUI : MonoBehaviour
{
    [Header("UI Components")]
    public Text uiText;                     // สำหรับ UI Text ปกติ
    public TextMeshProUGUI tmpText;         // สำหรับ TextMeshPro
    public CanvasGroup canvasGroup;         // ใช้ควบคุมความโปร่งใสของข้อความ

    [Header("Settings")]
    public float fadeDuration = 0.5f;       // ระยะเวลาการเฟดเข้า/ออก
    public float defaultDisplayTime = 3f;   // เวลาที่ข้อความค้างอยู่

    private Coroutine currentRoutine;

    void Awake()
    {
        // Auto setup
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup != null)
            canvasGroup.alpha = 0f;
    }

    /// <summary>
    /// แสดงข้อความพร้อม fade in/out อัตโนมัติ
    /// </summary>
    public void ShowMessage(string message, float duration = -1f)
    {
        if (duration <= 0) duration = defaultDisplayTime;

        // ถ้ามีข้อความก่อนหน้าอยู่ ให้หยุดก่อน
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(ShowMessageRoutine(message, duration));
    }

    IEnumerator ShowMessageRoutine(string message, float duration)
    {
        SetText(message);

        // Fade In
        yield return StartCoroutine(FadeCanvas(0, 1, fadeDuration));

        // Wait
        yield return new WaitForSeconds(duration);

        // Fade Out
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

    /// <summary>
    /// ใช้แสดงข้อความทันที (ไม่เฟด)
    /// </summary>
    public void SetInstant(string message)
    {
        StopAllCoroutines();
        SetText(message);
        if (canvasGroup != null)
            canvasGroup.alpha = 1f;
    }

    /// <summary>
    /// ซ่อนข้อความทันที
    /// </summary>
    public void HideInstant()
    {
        StopAllCoroutines();
        if (canvasGroup != null)
            canvasGroup.alpha = 0f;
    }
}