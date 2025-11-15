using System.Collections;
using UnityEngine;

public class Wave1Manager : MonoBehaviour
{
    public static Wave1Manager Instance;

    [Header("References")]
    public TutorialUI tutorialUI;
    public GameObject enemyDummyPrefab;
    public Transform enemySpawnPoint;
    public GameObject pickupPrefab;
    public Transform pickupSpawnPoint;

    [Header("Tutorial Timings")]
    public float showDuration = 2f;

    bool moved = false;
    bool jumped = false;
    bool attacked = false;
    bool dummyDead = false;
    bool pickupCollected = false;

    GameObject currentDummy;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        StartCoroutine(RunTutorial());
    }

    IEnumerator RunTutorial()
    {
        yield return tutorialUI.ShowText("Wave 1: Control Training", showDuration);
        
        yield return tutorialUI.ShowText("Press A or D to move", showDuration);
        yield return StartCoroutine(WaitUntilMove());
        
        yield return tutorialUI.ShowText("Press SpaceBar to jump", showDuration);
        yield return StartCoroutine(WaitUntilJump());
        
        yield return tutorialUI.ShowText("Press J to attack", showDuration);
        yield return StartCoroutine(WaitUntilAttack());
        
        yield return tutorialUI.ShowText("Good! Destroy the training dummy!", showDuration);
        SpawnDummy();

        yield return new WaitUntil(() => dummyDead == true);
        
        yield return tutorialUI.ShowText("Collect items dropped by enemies", showDuration);
        yield return new WaitUntil(() => pickupCollected == true);

        yield return tutorialUI.ShowText("Wave 1 Complete!", showDuration);
    }

    IEnumerator WaitUntilMove()
    {
        moved = false;

        while (!moved)
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
            {
                moved = true;
                break;
            }
            yield return null;
        }
    }
    IEnumerator WaitUntilJump()
    {
        jumped = false;
        while (!jumped)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                jumped = true;
            yield return null;
        }
    }

    IEnumerator WaitUntilAttack()
    {
        attacked = false;

        while (!attacked)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                attacked = true;
                break;
            }
            yield return null;
        }
    }

    void SpawnDummy()
    {
        currentDummy = Instantiate(enemyDummyPrefab, enemySpawnPoint.position, Quaternion.identity);

        var dummy = currentDummy.GetComponent<EnemyDummy>();
        dummy.pickupPrefab = pickupPrefab;
        dummy.pickupSpawnPoint = pickupSpawnPoint;

        StartCoroutine(CheckDummyAlive());
    }

    IEnumerator CheckDummyAlive()
    {
        dummyDead = false;
        while (currentDummy != null)
            yield return null;

        dummyDead = true;
    }

    public void OnPickupCollected()
    {
        pickupCollected = true;
    }
}