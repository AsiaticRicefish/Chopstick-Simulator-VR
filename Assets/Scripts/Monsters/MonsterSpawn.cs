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

    private float spawnInterval = 5f;
    private float timer = 0f;
    private float elapsedTime = 0f;

    [SerializeField] private int maxMonsters = 10;
    [SerializeField] private float overflowCountdownTime = 10f;

    private List<GameObject> activeMonsters = new List<GameObject>();

    private bool isOverflow = false;
    private float overflowTimer = 0f;

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

        if (activeMonsters.Count > maxMonsters)
        {
            if (!isOverflow)
            {
                isOverflow = true;
                overflowTimer = overflowCountdownTime;
                Debug.Log("몬스터 수 초과");
            }
        }
        else if (isOverflow)
        {
            isOverflow = false;
            Debug.Log("몬스터 수 정상화");
        }

        if (isOverflow)
        {
            overflowTimer -= Time.deltaTime;
            if (overflowTimer <= 0f)
            {
                Debug.Log("패배! 몬스터 수를 줄이지 못했음");
            }
        }
        activeMonsters.RemoveAll(monster => monster == null);
    }

    private void SpawnMonster()
    {
        MonsterType typeToSpawn = availableMonsterTypes[Random.Range(0, availableMonsterTypes.Count)];
        GameObject prefab = monsterPrefabs.Find(x => x.type == typeToSpawn).prefab;

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        GameObject spawned = Instantiate(prefab, spawnPoint.position, Quaternion.identity);

        activeMonsters.Add(spawned);

        Monster monster = spawned.GetComponent<Monster>();
        if (monster != null)
        {
            monster.OnMonsterDie += () => activeMonsters.Remove(spawned);
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
