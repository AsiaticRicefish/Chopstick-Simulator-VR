using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightSpawn : MonoBehaviour
{
    [SerializeField] private GameObject knightPrefab; 
    [SerializeField] private Transform[] spawnPoints;

    [SerializeField] private float spawnInterval = 5f;

    [SerializeField] private int maxKnights = 10;

    private float timer = 0f;

    private List<GameObject> activeKnights = new List<GameObject>();

    private void Update()
    {
        timer += Time.deltaTime;

        activeKnights.RemoveAll(knight => knight == null);

        if (timer >= spawnInterval && activeKnights.Count < maxKnights)
        {
            SpawnKnight();
            timer = 0f;
        }
    }

    private void SpawnKnight()
    {
        if (knightPrefab == null || spawnPoints.Length == 0) return;

        int index = Random.Range(0, spawnPoints.Length);
        GameObject knight = Instantiate(knightPrefab, spawnPoints[index].position, spawnPoints[index].rotation);
        activeKnights.Add(knight);
    }
}