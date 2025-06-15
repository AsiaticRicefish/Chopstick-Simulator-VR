using System;
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

    public event Action OnMonsterDie;
    protected Animator animator;

    [SerializeField] private float targetRefreshInterval = 1f;
    private float targetRefreshTimer = 0f;

    protected virtual void Start()
    {
        FindNewTarget();

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        if (isDead) return;

        targetRefreshTimer += Time.deltaTime;
        if (targetRefreshTimer >= targetRefreshInterval)
        {
            targetRefreshTimer = 0f;
            FindNewTarget();
        }

        if (target != null)
        {
            MoveToTarget();

            if (agent != null && animator != null)
            {
                animator.SetFloat("Speed", agent.velocity.magnitude);
            }
        }
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
        if (agent != null && agent.isOnNavMesh) agent.isStopped = true;

        if (animator != null)
        {
            animator.SetBool("IsDead", true);
        }

        OnMonsterDie?.Invoke();

        Destroy(gameObject);
    }

    protected virtual void FindNewTarget() // target을 파괴하면 다른 target을 찾으로 이동
    {
        List<GameObject> targets = new List<GameObject>();
        targets.AddRange(GameObject.FindGameObjectsWithTag("MonsterTarget"));
        targets.AddRange(GameObject.FindGameObjectsWithTag("Knight"));


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
