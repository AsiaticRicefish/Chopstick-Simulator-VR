using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Transform target; 
    [SerializeField] private Vector3 offset = new Vector3(0, 0, 0);

    private void Update()
    {
        if (target == null) return;

        transform.position = target.position + offset;

        Vector3 camDir = transform.position - Camera.main.transform.position;
        camDir.y = 0;
        transform.rotation = Quaternion.LookRotation(camDir);
    }

    public void SetHealth(float current, float max)
    {
        slider.value = current / max;
    }
}
