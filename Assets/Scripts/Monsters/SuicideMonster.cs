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

        // ���� �Ÿ� ���̸� ����
        if (dist <= explodeRange)
        {
            isExploding = true;
            StartCoroutine(ExplodeAfterDelay(explodeDelay));
            return;
        }

        // Ÿ���� ���� �̵�
        agent.SetDestination(target.position);
    }

    private IEnumerator ExplodeAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (target == null)
        {
            MonsterDie();
            yield break;
        }

        if (target.TryGetComponent<IDamageable>(out var damageable))
        {
            animator.SetTrigger("Attack");
            damageable.TakeDamage(explodeDamage);
        }

        MonsterDie();
    }
}
