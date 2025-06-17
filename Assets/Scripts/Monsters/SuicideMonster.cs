using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SuicideMonster : Monster
{
    [SerializeField] float explodeRange = 1f;
    [SerializeField] float explodeDamage = 50f;
    [SerializeField] float explodeDelay = 1f;

    private bool isExploding = false;

    protected override void MoveToTarget()
    {
        if (isDead || target == null || isExploding) return;

        float dist = Vector3.Distance(transform.position, target.position);

        if (dist <= explodeRange)
        {
            isExploding = true;

            if (agent != null && agent.isOnNavMesh) agent.ResetPath();

            transform.LookAt(target.position);

            StartCoroutine(ExplodeAfterDelay(explodeDelay));
        }
        else
        {
            agent.SetDestination(target.position);
        }
    }

    private IEnumerator ExplodeAfterDelay(float delay)
    {
        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(delay);

        if (target != null && target.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TakeDamage(explodeDamage);
        }

        MonsterDie();
    }
}
