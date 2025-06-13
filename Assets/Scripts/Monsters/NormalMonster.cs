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

        agent.SetDestination(target.position);

        float dist = Vector3.Distance(transform.position, target.position);

        if (dist <= attackRange)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackInterval)
            {
                attackTimer = 0f;
                MonsterTarget monsterTarget = target.GetComponent<MonsterTarget>();
                if (monsterTarget != null)
                {
                    monsterTarget.TakeDamage(attackDamage);
                }
            }
        }
        else
        {
            attackTimer = 0f;
        }
    }
}