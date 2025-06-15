using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] List<GameObject> weapons;
    [SerializeField] Transform rightHandTransform;

    [Header("Weapon Switch")]
    [SerializeField] InputActionReference rightThumbstick;
    [SerializeField] float switchCooldown = 0.5f;

    private GameObject currentWeapon;
    private float lastSwitchTime = 0f;
  

    private void Start()
    {
        EquipWeapon(0);
    }

    private void Update()
    {
        HandleThumbstickInput();
    }

    private void HandleThumbstickInput()
    {
        if (Time.time - lastSwitchTime < switchCooldown) return;

        Vector2 input = rightThumbstick.action.ReadValue<Vector2>();

        if (input.y > 0.7f)
        {
            TryEquipWeapon(0); // 위: 권총
        }
        else if (input.x < -0.7f)
        {
            TryEquipWeapon(1); // 왼쪽: AK47
        }
        else if (input.x > 0.7f)
        {
            TryEquipWeapon(2); // 오른쪽: 스나이퍼
        }
    }

    private void TryEquipWeapon(int index)
    {
        if (index < 0 || index >= weapons.Count) return;

        EquipWeapon(index);
        lastSwitchTime = Time.time;
    }

    private void EquipWeapon(int index)
    {
        if (currentWeapon != null)
        {
            Destroy(currentWeapon);
        }

        GameObject newWeapon = Instantiate(weapons[index]);

        newWeapon.transform.SetParent(rightHandTransform);

        Transform grip = newWeapon.transform.Find("Grip");
        if (grip != null)
        {
            Vector3 offsetPos = -grip.localPosition;
            Quaternion offsetRot = Quaternion.Inverse(grip.localRotation);

            newWeapon.transform.localPosition = offsetPos;
            newWeapon.transform.localRotation = offsetRot;
        }
        else
        {
            newWeapon.transform.localPosition = Vector3.zero;
            newWeapon.transform.localRotation = Quaternion.identity;
        }

        currentWeapon = newWeapon;

        Debug.Log($"무기 : {weapons[index].name}");
    }
}
