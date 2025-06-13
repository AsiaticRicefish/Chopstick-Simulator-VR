using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject bulletPrefab;

    [SerializeField] AudioSource gunSound;

    [SerializeField] InputActionReference triggerAction;

    [SerializeField] Transform rightHandTransform;

    private void Start()
    {
        transform.SetParent(rightHandTransform);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    private void Update()
    {
        if (triggerAction.action.WasPressedThisFrame())
        {
            Fire();
        }
    }

    private void Fire()
    {
        if (gunSound != null)  gunSound.Play();

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}
