using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewWaveConfig", menuName = "Game/Wave Config")]
public class WaveConfig : ScriptableObject
{
    [Header("Wave Info")]
    public string waveName;
    public string startMessage;
    
    [Header("Enemies")]
    public List<WaveEnemyInfo> enemiesToSpawn; 

    [Header("Rewards")]
    public ActiveSkill skillUnlock;
}

[System.Serializable]
public struct WaveEnemyInfo
{
    public Enemy enemyPrefab;
    public int count;
}
