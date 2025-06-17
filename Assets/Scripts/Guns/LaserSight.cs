using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSight : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private float laserDistance = 50f;
    [SerializeField] private LayerMask hitMask;
    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (firePoint == null) return;

        Vector3 start = firePoint.position;
        Vector3 direction = firePoint.forward;
        Vector3 end = start + direction * laserDistance;

        if (Physics.Raycast(start, direction, out RaycastHit hit, laserDistance, hitMask))
        {
            end = hit.point;
        }

        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }
}
