using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossHealthBar : MonoBehaviour
{
    public static BossHealthBar Instance;

    [Header("UI Components")]
    public GameObject panel;       
    public Slider healthSlider;     
    public TextMeshProUGUI nameText; 

    void Awake()
    {
        Instance = this;
        Hide();
    }

    public void Show(string bossName, float maxHp)
    {
        panel.SetActive(true);
        if (nameText != null) nameText.text = bossName;
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHp;
            healthSlider.value = maxHp;
        }
    }

    public void UpdateHealth(float currentHp)
    {
        if (healthSlider != null) healthSlider.value = currentHp;
    }

    public void Hide()
    {
        panel.SetActive(false);
    }
}
