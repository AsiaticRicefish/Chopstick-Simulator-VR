using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRUICamera : MonoBehaviour
{
    [SerializeField] private Transform xrCamera;
    [SerializeField] private float distanceFromCamera = 2f;
    [SerializeField] private float heightOffset = 0.5f;

    private void LateUpdate()
    {
        if (xrCamera == null) return;

        Vector3 targetPos = xrCamera.position + xrCamera.forward * distanceFromCamera;
        targetPos.y += heightOffset;

        transform.position = targetPos;

        Vector3 lookDir = targetPos - xrCamera.position;
        lookDir.y = 0f;
        transform.rotation = Quaternion.LookRotation(lookDir);
    }
}
