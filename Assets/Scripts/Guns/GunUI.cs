using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GunUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI gunText;
    private Gun currentGun;

    public void SetGun(Gun newGun)
    {
        currentGun = newGun;
    }

    private void Update()
    {
        if (currentGun == null) return;
 
        string gunName = currentGun.gunType.ToString();
        int currentAmmo = currentGun.currentAmmo;
        int maxAmmo = currentGun.maxAmmo;
        bool isReloading = currentGun.isReloading;
        string status;

        if (isReloading)
        {
            status = "재장전 중";
        }
        else
        {
            status = "";
        }

        gunText.text = $"{gunName} : {currentAmmo}\n{status}";
    }
}
