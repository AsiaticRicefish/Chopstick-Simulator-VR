using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Knight : MonoBehaviour, IDamageable
{
    [SerializeField] private float health = 100f;
    [SerializeField] private float Power = 10f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackInterval = 1.5f;

    private float attackTimer = 0f;
    private Transform currentTarget;
    private NavMeshAgent agent;
    private Animator animator;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        attackTimer += Time.deltaTime;

        animator.SetFloat("Speed", agent.velocity.magnitude);

        if (currentTarget == null)
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
        if (target.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TakeDamage(Power);
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            animator.SetTrigger("IsDead");
            StartCoroutine(DieAfterDelay(1f));
        }
    }

    private IEnumerator DieAfterDelay(float delay)
    {
        agent.isStopped = true;
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
