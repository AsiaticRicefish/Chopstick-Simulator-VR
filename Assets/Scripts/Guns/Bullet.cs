using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed = 20f;
    [SerializeField] float lifeTime = 3f;
    [SerializeField] float damage = 10f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        if (CheckHitByRaycast()) return;
        MoveBullet();
    }

    private void MoveBullet()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private bool CheckHitByRaycast()
    {
        float distance = speed * Time.deltaTime;
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, distance))
        {
            HandleHit(hit);
            return true;
        }

        return false;
    }

    private void HandleHit(RaycastHit hit)
    {
        if (hit.collider.GetComponentInParent<IDamageable>() is IDamageable target)
        {
            target.TakeDamage(damage);
        }

        if (hit.collider.CompareTag("Wall") || hit.collider.CompareTag("Ground") || hit.collider.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }    
    }
}
