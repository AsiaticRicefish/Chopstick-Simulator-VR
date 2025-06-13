using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTarget : MonoBehaviour, IDamageable
{
    [SerializeField] public float targetHealth = 100f;

    public void TakeDamage(float amount)
    {
        if (targetHealth < 0) return;

        targetHealth -= amount;
        targetHealth = Mathf.Max(targetHealth, 0);

        Debug.Log($"���� ü���� {targetHealth}");
        if (targetHealth <= 0) GameOver();
    }

    private void GameOver()
    {
        Debug.Log("���ӿ���");
        Destroy(gameObject);
    }

}
