using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMonster : Monster
{
    [SerializeField] float attackRange = 1f;
    [SerializeField] float attackDamage = 10f;
    [SerializeField] float attackInterval = 2f;
    private float attackTimer;

    protected override void Start()
    {
        base.Start();

        if (agent != null)
        {
            agent.stoppingDistance = attackRange;
        }
    }

    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);
    }

    protected override void MoveToTarget()
    {
        if (isDead || target == null || agent == null) return;

        float dist = Vector3.Distance(transform.position, target.position);

        if (dist <= attackRange)
        {
            agent.ResetPath();
            transform.LookAt(target.position);

            attackTimer += Time.deltaTime;

            if (attackTimer >= attackInterval)
            {
                attackTimer = 0f;
                animator.SetTrigger("Attack");

                if (target.TryGetComponent<IDamageable>(out var damageable))
                {
                    damageable.TakeDamage(attackDamage);
                }
            }
        }
        else
        {
            agent.SetDestination(target.position);
            attackTimer = 0f;
        }
    }
}