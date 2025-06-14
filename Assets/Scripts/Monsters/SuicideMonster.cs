using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SuicideMonster : Monster
{
    [SerializeField] float explodeRange = 1f;
    [SerializeField] float explodeDamage = 50f;
    [SerializeField] float explodeDelay = 1f;

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

        // 타겟을 향해 이동
        agent.SetDestination(target.position);
    }

    private IEnumerator ExplodeAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (target.TryGetComponent<IDamageable>(out var damageable))
        {
            animator.SetTrigger("Attack");
            damageable.TakeDamage(explodeDamage);
        }

        MonsterDie();
    }
}
