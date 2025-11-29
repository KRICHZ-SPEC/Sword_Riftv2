using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossHealthBar : MonoBehaviour
{
    public static BossHealthBar Instance;

    [Header("UI References")]
    public GameObject barObject;     
    public Slider slider;            
    public TextMeshProUGUI nameText; 

    void Awake()
    {
        Instance = this;
        HideBar();
    }
    
    public void InitializeBar(string bossName, float maxHealth)
    {
        barObject.SetActive(true);
        
        if (nameText != null) 
            nameText.text = bossName;

        slider.maxValue = maxHealth;
        slider.value = maxHealth;
    }
    
    public void UpdateHealth(float currentHealth)
    {
        slider.value = currentHealth;
    }
    
    public void HideBar()
    {
        barObject.SetActive(false);
    }
}
