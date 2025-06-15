using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;


public class PlayerTeleport : MonoBehaviour
{
    [System.Serializable]
    public class TeleportPoint
    {
        public string pointName;
        public Transform positionTransform;
    }

    [SerializeField] TeleportPoint[] teleportPoints;

    [SerializeField] InputActionReference teleportA;
    [SerializeField] InputActionReference teleportB;
    [SerializeField] InputActionReference teleportC;

    [SerializeField] float teleportCooldown = 5f;
    private float lastTeleportTime = -Mathf.Infinity;

    private XROrigin xrOrigin;
   

    private void Start()
    {
        xrOrigin = GetComponent<XROrigin>();
    }

    private void Update()
    {
        if (Time.time - lastTeleportTime < teleportCooldown) return;

        if (teleportA.action.WasPressedThisFrame())
        {
            TryTeleport("A");
        }
        else if (teleportB.action.WasPressedThisFrame())
        {
            TryTeleport("B");
        }
        else if (teleportC.action.WasPressedThisFrame())
        {
            TryTeleport("C");
        }
    }

    private void TryTeleport(string pointName)
    {
        foreach (var point in teleportPoints)
        {
            if (point.pointName == pointName)
            {
                xrOrigin.MoveCameraToWorldLocation(point.positionTransform.position);

                Vector3 forward = point.positionTransform.forward;
                forward.y = 0;
                Quaternion targetRotation = Quaternion.LookRotation(forward);
                xrOrigin.transform.rotation = targetRotation;

                lastTeleportTime = Time.time;
                return;
            }
        }
    }
}
