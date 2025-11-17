using UnityEngine;
using System.Collections; 

public class Wave2Manager : MonoBehaviour
{
    public static Wave2Manager Instance;

    public TutorialUI tutorialUI;
    public GameObject enemyPrefab; 
    public Transform enemySpawnPoint; 

    private bool skillUsed = false;
    private bool enemySpawned = false;
    private Enemy spawnedEnemy;

    void Awake()
    {
        Instance = this;
    }

    public void BeginWave2Tutorial()
    {
        StartCoroutine(tutorialUI.ShowText("Press F to use FireBall Skill", 2f));
    }

    void Update()
    {
        if (enemySpawned && spawnedEnemy == null)
        {
            enemySpawned = false; 
            StartCoroutine(CompleteWave());
        }
    }

    public void OnUseSkill()
    {
        if (skillUsed) return;
        skillUsed = true;
        
        enemySpawned = true;
        SpawnEnemy();
        StartCoroutine(tutorialUI.ShowText("Good! Defeat the enemy!", 2f));
    }

    void SpawnEnemy()
    {
         if (enemyPrefab == null || enemySpawnPoint == null) return;
         GameObject enemyObj = Instantiate(enemyPrefab, enemySpawnPoint.position, Quaternion.identity);
         spawnedEnemy = enemyObj.GetComponent<Enemy>();
    }

    IEnumerator CompleteWave()
    {
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