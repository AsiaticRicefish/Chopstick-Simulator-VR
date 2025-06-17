using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
public class Knight : MonoBehaviour, IDamageable
{
    [SerializeField] private HealthBarUI healthBar;
    [SerializeField] private float health;
    private float currentHealth;

    [SerializeField] private float Power = 10f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackInterval = 1.5f;

    private float attackTimer = 0f;
    private Transform currentTarget;
    private NavMeshAgent agent;
    private Animator animator;

    private bool isDead = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        currentHealth = health;

        if (healthBar == null) healthBar = GetComponentInChildren<HealthBarUI>();

        if (healthBar != null) healthBar.SetHealth(currentHealth ,health);
    }

    private void Update()
    {
        if (isDead) return;

        attackTimer += Time.deltaTime;

        animator.SetFloat("Speed", agent.velocity.magnitude);

        if (currentTarget == null || !currentTarget.gameObject.activeInHierarchy)
        {
            currentTarget = FindClosestEnemy();
        }

        if (currentTarget != null)
        {
            float dist = Vector3.Distance(transform.position, currentTarget.position);

            if (dist > attackRange)
            {
                agent.SetDestination(currentTarget.position);
            }
            else
            {
                agent.ResetPath();
                transform.LookAt(currentTarget.position);

                if (attackTimer >= attackInterval)
                {
                    animator.SetTrigger("Attack");
                    Attack(currentTarget);
                    attackTimer = 0f;
                }
            }
        }
    }

    private Transform FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;
        Transform closest = null;

        foreach (GameObject enemy in enemies)
        {
            if (enemy == null) continue;

            float dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist < shortestDistance)
            {
                shortestDistance = dist;
                closest = enemy.transform;
            }
        }

        return closest;
    }

    private void Attack(Transform target)
    {
        if (isDead) return;

        if (target.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TakeDamage(Power);

            if (target.TryGetComponent<Monster>(out var monster))
            {
                monster.AggroTarget(transform);
            }
        }
      
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;

        if (healthBar != null) healthBar.SetHealth(currentHealth, health);

        if (currentHealth <= 0)
        {
            isDead = true;

            agent.isStopped = true;
            animator.SetTrigger("IsDead");

            Collider col = GetComponent<Collider>();
            if (col != null) col.enabled = false;

            if (healthBar != null) Destroy(healthBar.gameObject);

            StartCoroutine(DieAfterDelay(1f));
        }
    }

    private IEnumerator DieAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
