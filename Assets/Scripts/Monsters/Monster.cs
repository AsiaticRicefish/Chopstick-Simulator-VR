using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Monster : MonoBehaviour, IDamageable
{
    [SerializeField] public float health;

    protected Transform target;
    protected NavMeshAgent agent;

    protected bool isDead = false;

    protected virtual void Start()
    {
        GameObject monsterTarget = GameObject.FindWithTag("MonsterTarget");
        if (monsterTarget != null) target = monsterTarget.transform;

        agent = GetComponent<NavMeshAgent>();
    }

    protected virtual void Update()
    {
        if (isDead) return;

        if (target == null) FindNewTarget();
        if (target != null) MoveToTarget();
    }

    public virtual void TakeDamage(float amount)
    {
        if (isDead) return;

        health -= amount;

        if (health <= 0)
        {
            MonsterDie();
        }
    }

    protected virtual void MonsterDie()
    {
        isDead = true;
        if (agent != null) agent.isStopped = true;

        Destroy(gameObject);
    }

    protected virtual void FindNewTarget() // target을 파괴하면 다른 target을 찾으로 이동
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("MonsterTarget");

        float closestDistance = Mathf.Infinity;
        Transform closest = null;

        foreach (GameObject obj in targets)
        {
            if (obj == null) continue;

            float dist = Vector3.Distance(transform.position, obj.transform.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                closest = obj.transform;
            }
        }

        target = closest;
    }

    protected abstract void MoveToTarget();
}
