using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Monster : MonoBehaviour, IDamageable
{
    [SerializeField] private HealthBarUI healthBar;
    [SerializeField] public float health;
    private float currentHealth;

    protected Transform target;
    protected NavMeshAgent agent;

    protected bool isDead = false;

    public event Action OnMonsterDie;
    protected Animator animator;

    private float aggroHoldTime = 3f;
    private float aggroTimer = 0f;

    protected virtual void Start()
    {
        FindNewTarget();

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        currentHealth = health;

        if (healthBar == null) healthBar = GetComponentInChildren<HealthBarUI>();

        if (healthBar != null) healthBar.SetHealth(currentHealth ,health);
    }

    protected virtual void Update()
    {
        if (isDead) return;

        if (aggroTimer > 0f)
        {
            aggroTimer -= Time.deltaTime;
        }
        else
        {
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

        currentHealth -= amount;

        if (healthBar != null) healthBar.SetHealth(currentHealth, health);

        if (currentHealth <= 0)
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

        if (healthBar != null) Destroy(healthBar.gameObject);

        OnMonsterDie?.Invoke();

        StartCoroutine(DestroyAfterDelay(1f));
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    protected virtual void FindNewTarget()
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

    public void AggroTarget(Transform attacker)
    {
        if (!isDead && attacker != null)
        {
            target = attacker;
            aggroTimer = aggroHoldTime;
        }
    }

    protected abstract void MoveToTarget();
}
