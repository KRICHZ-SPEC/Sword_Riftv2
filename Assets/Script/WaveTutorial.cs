using UnityEngine;
using UnityEngine.UI;

public class WaveTutorial : MonoBehaviour
{
    public Player player;
    public Enemy dummyEnemy;
    public Item tutorialItem;
    public Text tutorialText;

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
            case 0: // Move
                if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0)
                {
                    NextStep();
                }
                break;

            case 1: // Jump
                if (Input.GetButtonDown("Jump"))
                {
                    NextStep();
                }
                break;

            case 2: // Attack dummy
                if (dummyEnemy == null) // หมายถึง dummy ถูกทำลาย
                {
                    NextStep();
                }
                break;

            case 3: // Collect item
                if (tutorialItem == null) // หมายถึงเก็บ item แล้ว
                {
                    NextStep();
                }
                break;

            case 4: // Defeat enemy
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
                tutorialText.text = "ใช้ปุ่ม A / D เพื่อเคลื่อนไหว";
                break;
            case 1:
                tutorialText.text = "กด SPACE เพื่อกระโดด";
                break;
            case 2:
                tutorialText.text = "กด J เพื่อโจมตีหุ่นฝึก";
                break;
            case 3:
                tutorialText.text = "เดินไปเก็บไอเท็มบนพื้น";
                break;
            case 4:
                tutorialText.text = "กำจัดศัตรูตัวจริงที่โผล่มา!";
                break;
            default:
                tutorialText.text = "";
                break;
        }
    }

    void CompleteTutorial()
    {
        isCompleted = true;
        tutorialText.text = "🎉 Tutorial Complete! 🎉";
        // สามารถเรียก WaveManager.NextWave() ได้หลังจากนี้
    }
}
