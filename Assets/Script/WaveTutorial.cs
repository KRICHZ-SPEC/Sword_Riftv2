using UnityEngine;
using UnityEngine.UI;

public class WaveTutorial : MonoBehaviour
{
    public Player player;
    public Enemy dummyEnemy;
    public Item tutorialItem;
    public Text tutorialText;
    public TutorialUI tutorialUI;
    private int step = 0;
    private bool isCompleted = false;

    void Start()
    {
        step = 0;
        ShowStepText();
    }

    void Update()
    {
        if (isCompleted) return;
        CheckProgress();
    }

    void CheckProgress()
    {
        switch (step)
        {
            case 0: 
                if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0)
                {
                    NextStep();
                }
                break;

            case 1:
                if (Input.GetButtonDown("Jump"))
                {
                    NextStep();
                }
                break;

            case 2: 
                if (dummyEnemy == null) 
                {
                    NextStep();
                }
                break;

            case 3: 
                if (tutorialItem == null) 
                {
                    NextStep();
                }
                break;

            case 4: 
                Enemy realEnemy = FindObjectOfType<Enemy>();
                if (realEnemy == null)
                {
                    CompleteTutorial();
                }
                break;
        }
    }

    void NextStep()
    {
        step++;
        ShowStepText();
    }

    void ShowStepText()
    {
        switch (step)
        {
            case 0:
                tutorialUI.ShowMessage("Use A / D to move");
                break;
            case 1:
                tutorialUI.ShowMessage("Press SPACE to jump");
                break;
            case 2:
                tutorialUI.ShowMessage("Press J to attack the training dummy");
                break;
            case 3:
                tutorialUI.ShowMessage("Walk to pick up the item on the ground");
                break;
            case 4:
                tutorialUI.ShowMessage("Defeat the real enemy that appears!");
                break;
            default:
                tutorialUI.ShowMessage(" Tutorial Complete! ");
                break;
        }
    }

    void CompleteTutorial()
    {
        isCompleted = true;
        tutorialUI.ShowMessage(" Tutorial Complete! ");
    }
}
