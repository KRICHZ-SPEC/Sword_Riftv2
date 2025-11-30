using UnityEngine;
using UnityEngine.UI;

public class SkillCooldownUI : MonoBehaviour
{
    [Header("References")]
    public Player player;           
    public Image cooldownOverlay;   
    public GameObject skillIconRoot; 

    void Start() 
    {
        if (player == null)
        {
            player = FindObjectOfType<Player>();
        }
        
        if (skillIconRoot != null) skillIconRoot.SetActive(false);
    }

    void Update()
    {
        if (player == null) return;

        SkillInstance fireBall = player.FireBallSkillInstance;
        
        if (fireBall != null)
        {
            if (skillIconRoot != null && !skillIconRoot.activeSelf)
            {
                skillIconRoot.SetActive(true);
            }

            if (cooldownOverlay != null)
            {
                cooldownOverlay.fillAmount = fireBall.GetCooldownRatio();
            }
        }
        else
        {
            if (skillIconRoot != null && skillIconRoot.activeSelf)
            {
                skillIconRoot.SetActive(false);
            }
        }
    }
}