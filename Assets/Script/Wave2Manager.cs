using UnityEngine;
using System.Collections;

public class Wave2Manager : MonoBehaviour
{
    public static Wave2Manager Instance;

    [Header("References")]
    public TutorialUI tutorialUI;
    public GameObject enemyPrefab;
    public Transform enemySpawnPoint;

    [Header("Wave State")]
    private bool tutorialStarted = false;
    private bool enemySpawned = false;
    private bool waveCompleted = false; 
    private Enemy spawnedEnemy;

    void Awake()
    {
        Instance = this;
    }

    public void BeginWave2Tutorial()
    {
        StartCoroutine(RunWave2Tutorial());
    }

    IEnumerator RunWave2Tutorial()
    {
        yield return StartCoroutine(tutorialUI.ShowText("Wave 2: Skill Practice", 2f));
        yield return StartCoroutine(tutorialUI.ShowText("Press F to use FireBall Skill", 2f));
        tutorialStarted = true;
    }

    void Update()
    {
        if (enemySpawned && spawnedEnemy == null && !waveCompleted)
        {
            waveCompleted = true; 
            StartCoroutine(CompleteWave());
        }
    }

    public void OnUseSkill()
    {
        if (!tutorialStarted || enemySpawned || waveCompleted) return; 
        
        enemySpawned = true; 
        SpawnEnemy();
    }

    void SpawnEnemy()
    {
         if (enemyPrefab == null || enemySpawnPoint == null) return;
         GameObject enemyObj = Instantiate(enemyPrefab, enemySpawnPoint.position, Quaternion.identity);
         spawnedEnemy = enemyObj.GetComponent<Enemy>();
    }

    IEnumerator CompleteWave()
    {
        yield return tutorialUI.ShowText("Good! Enemy Defeated!", 2f); 
        yield return tutorialUI.ShowText("Wave 2 Complete!", 2f);
        Debug.Log("WAVE 2 DONE");
        
        WaveManager mainManager = GetComponent<WaveManager>();
        if (mainManager != null)
        {
            mainManager.enabled = true;
            mainManager.StartWave(0);
        }
        this.enabled = false;
    }
}