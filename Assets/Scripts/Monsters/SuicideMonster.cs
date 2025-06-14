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

        // ���� �Ÿ� ���̸� ����
        if (dist <= explodeRange)
        {
            StartCoroutine(ExplodeAfterDelay(explodeDelay));
            return;
        }

        // Ÿ���� ���� �̵�
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
