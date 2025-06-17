using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject leftRayInteractor;

    [SerializeField] private int monsterLimit = 10;
    [SerializeField] private float overflowCountdownTime = 10f;

    [Header("MonsterCount")]
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private TextMeshProUGUI currentmonsterCountText;
    [SerializeField] private TextMeshProUGUI countdownText;

    [Header("warningBGM")]
    [SerializeField] private AudioSource warningAudioSource;

    private bool hasPlayedWarningSound = false;

    [Header("GameOver")]
    [SerializeField] private GameObject gameOverPanel;

    [Header("BGM")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioClip normalBGMClip;
    [SerializeField] private AudioClip intenseBGMClip;

    private float survivalTime = 0f;
    [SerializeField] private TextMeshProUGUI survivalTimeText;

    private List<GameObject> activeMonsters = new List<GameObject>();
    private float overflowTimer = 0f;
    private bool isOverflow = false;
    [HideInInspector] public bool isGameOver = false;

    private int destroyedDefenseCount = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Time.timeScale = 1f;

        if (leftRayInteractor != null) leftRayInteractor.SetActive(false);

        if (bgmSource != null && normalBGMClip != null)
        {
            bgmSource.clip = normalBGMClip;
            bgmSource.Play();
        }
    }

    private void Update()
    {
        if (!isGameOver)
        {
            survivalTime += Time.deltaTime;
        }

        if (isOverflow)
        {
            overflowTimer -= Time.deltaTime;

            int displayTime = Mathf.CeilToInt(overflowTimer);

            if (countdownText != null)
            {
                countdownText.text = $"{displayTime}";
            }

            if (overflowTimer <= 0f)
            {
                GameOver();
            }
        }
        activeMonsters.RemoveAll(m => m == null);
    }

    public void OnMonsterSpawned(GameObject monster)
    {
        activeMonsters.Add(monster);
        CheckMonsterOverflow();
        UpdateMonsterCountUI();
    }

    public void OnMonsterDied(GameObject monster)
    {
        activeMonsters.Remove(monster);
        CheckMonsterOverflow();
        UpdateMonsterCountUI();
    }

    private void UpdateDangerUI(bool isDanger)
    {
        if (statusText == null) return;

        if (isDanger)
        {
            statusText.text = "위험! 몬스터 수를 초과하였습니다!";
            statusText.color = Color.red;
        }
        else
        {
            statusText.text = "몬스터 수가 안전 범위에 있습니다. ";
            statusText.color = Color.green;
        }
    }

    private void UpdateMonsterCountUI()
    {
        if (currentmonsterCountText != null)
        {
            currentmonsterCountText.text = $"현재 몬스터 수 : {activeMonsters.Count}";
        }
    }

    private void CheckMonsterOverflow()
    {
        if (isGameOver) return;

        if (activeMonsters.Count > monsterLimit)
        {
            if (!isOverflow)
            {
                isOverflow = true;
                overflowTimer = overflowCountdownTime;
                UpdateDangerUI(true);

                if (!hasPlayedWarningSound && warningAudioSource != null)
                {
                    warningAudioSource.Play();
                    hasPlayedWarningSound = true;
                }
            }
        }
        else if (isOverflow)
        {
            isOverflow = false;
            UpdateDangerUI(false);
            hasPlayedWarningSound = false;
        }
    }
    public void OnDefenseDestroyed()
    {
        destroyedDefenseCount++;

        if (destroyedDefenseCount == 1)
        {
            if (bgmSource != null && intenseBGMClip != null)
            {
                bgmSource.Stop();
                bgmSource.clip = intenseBGMClip;
                bgmSource.Play();
            }
        }

        if (destroyedDefenseCount >= 2 && !isGameOver)
        {
            GameOver();
        }
    }


    private void GameOver()
    {
        isGameOver = true;

        Time.timeScale = 0f;

        if (leftRayInteractor != null) leftRayInteractor.SetActive(true);
        if (gameOverPanel != null) gameOverPanel.SetActive(true);

        if (survivalTimeText != null)
        {
            int totalSeconds = Mathf.FloorToInt(survivalTime);
            int minutes = totalSeconds / 60;
            int seconds = totalSeconds % 60;

            survivalTimeText.text = $"당신의 생존 시간: {minutes:00}:{seconds:00}";
        }
    }
}
