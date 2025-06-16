using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterTarget : MonoBehaviour, IDamageable
{
    [SerializeField] public float maxHealth = 1000f;
    private float currentHealth;

    [Header("UI")]
    [SerializeField] private Slider healthBar;
    private Image fillImage;
    [SerializeField] private TextMeshProUGUI destroyedText;

    private bool isUnderAttack = false;

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateUI();

        currentHealth = maxHealth;
        UpdateUI();

        if (healthBar != null && healthBar.fillRect != null)
        {
            fillImage = healthBar.fillRect.GetComponent<Image>();
        }
    }

    private void Update()
    {
        if (isUnderAttack && fillImage != null)
        {
            float alpha = Mathf.PingPong(Time.time * 4f, 1f);
            fillImage.color = new Color(1f, 0f, 0f, alpha);
        }
    }

    public void TakeDamage(float amount)
    {
        if (currentHealth <= 0f) return;

        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0f);
        UpdateUI();

        isUnderAttack = true;
        Invoke(nameof(StopBlinking), 1.5f);

        if (currentHealth <= 0) TargetDestroy();
    }

    private void StopBlinking()
    {
        isUnderAttack = false;

        if (fillImage != null) fillImage.color = Color.red;
    }

    private void UpdateUI()
    {
        if (healthBar != null)
        {
            healthBar.value = currentHealth / maxHealth;
        }
    }

    private void TargetDestroy()
    {
        GameManager.Instance.OnDefenseDestroyed();

        if (healthBar != null)
            healthBar.gameObject.SetActive(false);

        if (destroyedText != null)
        {
            destroyedText.text = "ÆÄ±«µÊ";
            destroyedText.color = Color.red;
            destroyedText.gameObject.SetActive(true);
        }

        gameObject.tag = "Untagged";
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;
    }

}
