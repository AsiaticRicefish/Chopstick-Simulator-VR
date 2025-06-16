using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public enum GunType
{
    Pistol,
    AK47,
    Sniper
}

public class Gun : MonoBehaviour
{

    [Header("General")]
    public GunType gunType;
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] AudioSource gunSound;
    [SerializeField] InputActionReference triggerAction;

    [Header("Ammo")]
    public int maxAmmo = 10;
    public int currentAmmo;

    [Header("Fire Control")]
    [SerializeField] float fireRate = 0.1f; // 연사 속도
    private float fireCooldown = 0f;

    [Header("Reload")]
    [SerializeField] float reloadTime = 2f;
    [SerializeField] InputActionReference reloadAction;
    [SerializeField] private AudioSource reloadSound;
    public bool isReloading = false;

    private void Start()
    {
        currentAmmo = maxAmmo;
    }

    private void Update()
    {
        fireCooldown -= Time.deltaTime;

        if (isReloading) return;

        if (reloadAction.action.WasPressedThisFrame() && currentAmmo < maxAmmo)
        {
            StartCoroutine(Reloud());
            return;
        }

        switch (gunType)
        {
            case GunType.Pistol:
                if (triggerAction.action.WasPressedThisFrame())
                {
                    CurrentFireCondition();
                }
                break;

            case GunType.AK47:
                if (triggerAction.action.IsPressed())
                {
                    CurrentFireCondition();
                }
                break;
            case GunType.Sniper:
                if (triggerAction.action.WasPressedThisFrame())
                {
                    CurrentFireCondition();
                }
                break;
        }
    }

    private IEnumerator Reloud()
    {
        isReloading = true;
        if (reloadSound != null) reloadSound.Play();
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        isReloading = false;
    }


    private void CurrentFireCondition() // 현재 총알을 발사할 수 있는 상태인가 확인
    {
        if (currentAmmo <= 0) return;
        if (fireCooldown > 0f) return;

        Fire();
        fireCooldown = fireRate;
    }

    private void Fire()
    {
        currentAmmo--;

        if (gunSound != null) gunSound.Play();

        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}
