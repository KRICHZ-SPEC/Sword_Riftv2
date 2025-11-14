using UnityEngine;

public class Wave1Manager : MonoBehaviour
{
    public TutorialUI tutorialUI;
    public Player player;

    public EnemyDummy dummyEnemy;         // หุ่นฝึก
    public GameObject pickUpItem;         // ไอเท็มที่ต้องเก็บ
    public Enemy realEnemy;               // ศัตรูจริงตัวสุดท้าย

    private int step = 0;
    private bool itemPicked = false;
    private bool dummyKilled = false;
    private bool realEnemyKilled = false;

    void Start()
    {
        Debug.Log("Wave1Manager Start()");
        step = 0;
        ShowStepMessage();
    }

    void Update()
    {
        switch (step)
        {
            // 0 : Move
            case 0:
                if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
                    NextStep();
                break;

            // 1 : Jump
            case 1:
                if (Input.GetKeyDown(KeyCode.Space))
                    NextStep();
                break;

            // 2 : Attack dummy
            case 2:
                if (dummyEnemy == null || dummyEnemy.maxHP <= 0)
                {
                    dummyKilled = true;
                    NextStep();
                }
                break;

            // 3 : Pick item
            case 3:
                if (itemPicked)
                    NextStep();
                break;

            // 4 : Defeat real enemy
            case 4:
                if (realEnemy == null || realEnemy.hp <= 0)
                {
                    realEnemyKilled = true;
                    NextStep();
                }
                break;

            // 5 : Tutorial Complete
            case 5:
                // จบแล้วไม่ต้องทำอะไร
                break;
        }
    }

    // เรียกจาก PickupItem.cs
    public void OnItemPicked()
    {
        itemPicked = true;
    }

    // เปลี่ยน Step
    void NextStep()
    {
        step++;
        ShowStepMessage();
    }
    public void OnEnemyKilled()
    {
        if (step == 4)
        {
            NextStep();
        }
    }

    // ข้อความแต่ละ Step
    void ShowStepMessage()
    {
        Debug.Log("Show message step: " + step);
        switch (step)
        {
            case 0:
                tutorialUI.Show("Use A / D to move");
                break;
            case 1:
                tutorialUI.Show("Press SPACE to jump");
                break;
            case 2:
                tutorialUI.Show("Press J to attack the training dummy");
                break;
            case 3:
                tutorialUI.Show("Walk to pick up the item on the ground");
                break;
            case 4:
                tutorialUI.Show("Defeat the real enemy that appears!");
                break;
            case 5:
                tutorialUI.Show("Tutorial Complete!");
                break;
        }
    }
}