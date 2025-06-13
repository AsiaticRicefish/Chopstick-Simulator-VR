using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.Rendering.DebugUI.Table;

public class TeleportMonster : Monster
{
    [SerializeField] float teleportInterval = 6f;

    [SerializeField] float explodeRange = 1f;

    [SerializeField] float explodeDamage = 50f;

    [SerializeField] float explodeDelay = 1f;
    [SerializeField] GameObject warningEffectPrefab;

    private float teleportTimer;

    private bool hasTeleported = false;

    protected override void MoveToTarget()
    {
        if (isDead || target == null) return;

        float dist = Vector3.Distance(transform.position, target.position);

        // 자폭 거리 안이면 자폭
        if (dist <= explodeRange)
        {
            StartCoroutine(ExplodeAfterDelay(explodeDelay));
            return;
        }

        agent.SetDestination(target.position);

        // 순간이동
        if (!hasTeleported)
        {
            teleportTimer += Time.deltaTime;
            if (teleportTimer >= teleportInterval)
            {
                teleportTimer = 0f;
                hasTeleported = true;

                // NavMesh 위에 있는 랜덤 위치 찾기 시도
                Vector3 randomPos = RandomPointOnNavMesh(target.position, 10f, 15f);
                if (randomPos != Vector3.zero)
                {
                    // 경고 이펙트
                    Vector3 effectPos = randomPos + Vector3.up * 0.01f;
                    Quaternion rot = Quaternion.Euler(90f, 0f, 0f);
                    if (warningEffectPrefab != null)
                    {
                        GameObject warning = Instantiate(warningEffectPrefab, effectPos, rot);
                        Destroy(warning, 1f);
                    }

                    StartCoroutine(TeleportAfterDelay(0.5f, randomPos));
                }
            }
        }
    }

    private Vector3 RandomPointOnNavMesh(Vector3 center, float minRadius, float maxRadius)
    {
        for (int i = 0; i < 10; i++)
        {
            Vector2 randomCircle = Random.insideUnitCircle.normalized * Random.Range(minRadius, maxRadius);
            Vector3 samplePos = center + new Vector3(randomCircle.x, 0, randomCircle.y);

            if (NavMesh.SamplePosition(samplePos, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }
        return Vector3.zero;
    }

    private IEnumerator TeleportAfterDelay(float delay, Vector3 targetPosition)
    {
        yield return new WaitForSeconds(delay);

        if (agent != null)
        {
            agent.Warp(targetPosition);
            agent.SetDestination(target.position);
        }
    }

    private IEnumerator ExplodeAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        MonsterTarget monsterTarget = target.GetComponent<MonsterTarget>();
        if (monsterTarget != null)
        {
            monsterTarget.TakeDamage(explodeDamage);
        }

        MonsterDie();
    }
}

