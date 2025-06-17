using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterType
{
    Normal,
    Speed,
    Suicide,
    Heavy
}

[System.Serializable]
public class MonsterSpawnData
{
    public MonsterType type;
    public GameObject prefab;
}

public class MonsterSpawn : MonoBehaviour
{
    [SerializeField] private List<MonsterSpawnData> monsterPrefabs;
    [SerializeField] private Transform[] spawnPoints;

    private float spawnInterval = 3f;
    private float timer = 0f;
    private float elapsedTime = 0f;

    private List<MonsterType> availableMonsterTypes = new List<MonsterType> { MonsterType.Normal };

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnMonster();
        }

        UpdateMonster();
    }

    private void SpawnMonster()
    {
        MonsterType typeToSpawn = availableMonsterTypes[Random.Range(0, availableMonsterTypes.Count)];
        GameObject prefab = monsterPrefabs.Find(x => x.type == typeToSpawn).prefab;

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        GameObject spawned = Instantiate(prefab, spawnPoint.position, Quaternion.identity);

        GameManager.Instance.OnMonsterSpawned(spawned);

        Monster monster = spawned.GetComponent<Monster>();
        if (monster != null)
        {
            monster.OnMonsterDie += () => GameManager.Instance.OnMonsterDied(spawned);
        }
    }

    private void UpdateMonster()
    {
        if (elapsedTime > 30f && !availableMonsterTypes.Contains(MonsterType.Speed))
            availableMonsterTypes.Add(MonsterType.Speed);

        if (elapsedTime > 40f && !availableMonsterTypes.Contains(MonsterType.Suicide))
            availableMonsterTypes.Add(MonsterType.Suicide);

        if (elapsedTime > 50f && !availableMonsterTypes.Contains(MonsterType.Heavy))
            availableMonsterTypes.Add(MonsterType.Heavy);
    }
}
