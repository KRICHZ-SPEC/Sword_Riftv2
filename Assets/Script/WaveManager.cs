using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;

    [Header("Wave Configs")]
    public List<WaveConfig> allWaves; 
    public Transform[] spawnPoints;
    
    [Header("Special Spawns")]
    public Transform tutorialSpawnPoint;
    public Transform bossSpawnPoint;
    
    [Header("References")]
    public Player player;
    public TutorialUI tutorialUI;

    private int currentWaveIndex = 0;
    private int enemiesRemaining = 0;
    private bool isWaveActive = false;
    
    private bool tutorialMoved = false;
    private bool tutorialJumped = false;
    private bool tutorialAttacked = false;
    public bool pickupCollected = false; 

    void Awake() { Instance = this; }
    void OnEnable() { Enemy.OnEnemyDied += HandleEnemyDeath; }
    void OnDisable() { Enemy.OnEnemyDied -= HandleEnemyDeath; }

    public void StartWave(int index) { StartCoroutine(StartWaveRoutine(index)); }
    public void OnPickupCollected() { pickupCollected = true; }

    void Start()
    {
        if (allWaves.Count > 0)
            StartCoroutine(StartWaveRoutine(0));
    }

    IEnumerator StartWaveRoutine(int index)
    {
        if (index >= allWaves.Count)
        {
            if (tutorialUI) yield return StartCoroutine(tutorialUI.ShowText("VICTORY!", 5f));
            if (GameUIManager.Instance != null) 
            {
                GameUIManager.Instance.TriggerVictory();
            }
            yield break;
        }

        currentWaveIndex = index;
        WaveConfig config = allWaves[currentWaveIndex];
        
        if (config.skillUnlock != null && player != null)
        {
            player.UnlockSkill(config.skillUnlock);
        }
        
        if (currentWaveIndex == 0)
        {
            yield return StartCoroutine(RunBasicTutorial());
        }
        else
        {
            if (tutorialUI != null && !string.IsNullOrEmpty(config.startMessage))
            {
                string[] messages = config.startMessage.Split('|'); 
                
                foreach (string msg in messages)
                {
                    if (!string.IsNullOrWhiteSpace(msg))
                    {
                        yield return StartCoroutine(tutorialUI.ShowText(msg.Trim(), 2.5f));
                    }
                }
            }
        }

        SpawnEnemies(config);
    }

    IEnumerator RunBasicTutorial()
    {
        yield return tutorialUI.ShowText("Wave 1: Basic Training", 2f);
        yield return tutorialUI.ShowText("Press A / D to Move", 2f);
        tutorialMoved = false;
        while (!tutorialMoved) { if (Input.GetAxisRaw("Horizontal") != 0) tutorialMoved = true; yield return null; }

        yield return tutorialUI.ShowText("Press SpaceBar to Jump", 2f);
        tutorialJumped = false;
        while (!tutorialJumped) { if (Input.GetKeyDown(KeyCode.Space)) tutorialJumped = true; yield return null; }

        yield return tutorialUI.ShowText("Press J to Attack", 2f);
        tutorialAttacked = false;
        while (!tutorialAttacked) { if (Input.GetKeyDown(KeyCode.J)) tutorialAttacked = true; yield return null; }

        yield return tutorialUI.ShowText("Destroy the Dummy!", 2f);
    }
    
    void SpawnEnemies(WaveConfig config)
    {
        enemiesRemaining = 0;
        isWaveActive = true;

        foreach (var info in config.enemiesToSpawn)
        {
            for (int i = 0; i < info.count; i++)
            {
                Vector3 spawnPos;
                
                if (currentWaveIndex == 0 && tutorialSpawnPoint != null)
                {
                    spawnPos = tutorialSpawnPoint.position;
                }
                else if (currentWaveIndex == 2 && bossSpawnPoint != null)
                {
                    spawnPos = bossSpawnPoint.position;
                }
                else
                {
                    Transform randomPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                    spawnPos = randomPoint.position;
                }
                
                ObjectPoolManager.Instance.SpawnFromPool(
                    info.enemyPrefab.gameObject, 
                    spawnPos, 
                    Quaternion.identity
                );
                
                enemiesRemaining++;
            }
        }
    }

    void HandleEnemyDeath(Enemy enemy)
    {
        if (!isWaveActive) return;
        enemiesRemaining--;

        if (enemiesRemaining <= 0)
        {
            if (currentWaveIndex == 0) StartCoroutine(WaitForPickup());
            else EndWave();
        }
    }

    IEnumerator WaitForPickup()
    {
        yield return tutorialUI.ShowText("Pick up the Item!", 2f);
        pickupCollected = false;
        yield return new WaitUntil(() => pickupCollected);
        EndWave();
    }

    void EndWave()
    {
        isWaveActive = false;
        if (tutorialUI) StartCoroutine(tutorialUI.ShowText("Wave Cleared!", 2f));
        Invoke(nameof(NextWave), 3f);
    }

    void NextWave()
    {
        StartCoroutine(StartWaveRoutine(currentWaveIndex + 1));
    }
    public void AddEnemy()
    {
        enemiesRemaining++;
    }
}