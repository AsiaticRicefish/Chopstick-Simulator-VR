using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private GameObject leftRayInteractor;

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
        if (leftRayInteractor != null) leftRayInteractor.SetActive(true);
    }

    private void Update()
    {
        if (leftRayInteractor != null)
        {
            Ray ray = new Ray(leftRayInteractor.transform.position, leftRayInteractor.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, 20f))
            {
                Debug.Log("Ray가 충돌한 오브젝트: " + hit.collider.name);
            }
        }


        if (isOverflow)
        {
            overflowTimer -= Time.deltaTime;

            int displayTime = Mathf.CeilToInt(overflowTimer); // 정수형으로 표시

            if (countdownText != null)
            {
                countdownText.text = $"{displayTime}";
                countdownText.color = Color.red;
                countdownText.color = new Color(1f, 0f, 0f, Mathf.PingPong(Time.time * 2f, 1f));
            }
                
            if (overflowTimer <= 0f)
            {
                GameOver();
            }
            else
            {
                if (countdownText != null)
                {
                    countdownText.text = "";
                }
                    
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
        if (destroyedDefenseCount >= 2 && !isGameOver)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        isGameOver = true;

        Time.timeScale = 0f;

        gameOverPanel.SetActive(true);
    }
}
