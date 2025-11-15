using UnityEngine;

public class Wave2Manager : MonoBehaviour
{
    public static Wave2Manager Instance;

    public TutorialUI tutorialUI;

    private bool skillUsed = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        tutorialUI.ShowText("Press F to use FireBall Skill");
    }

    public void OnUseSkill()
    {
        if (skillUsed) return;

        skillUsed = true;

        tutorialUI.ShowText("Good! Skill Activated!");
        Invoke("CompleteWave", 1.5f);
    }

    void CompleteWave()
    {
        tutorialUI.ShowText("Wave 2 Complete!");
    }
}
