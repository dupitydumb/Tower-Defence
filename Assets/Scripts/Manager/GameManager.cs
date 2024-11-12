using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public InventoryManager inventoryManager;
    public UpgradeBuildingUI upgradeBuildingUI;
    public EnemySpawner enemySpawner;
    public Action OnResearchAdded;
    public Action OnDayChange;
    public GameState gameState = GameState.Day;
    public static GameManager instance;
    public List<ResearchItems> researchedItems = new List<ResearchItems>();
    public int currentDay = 1;
    public TMP_Text clockText; // Reference to the UI Text component for displaying the clock
    public TMP_Text dayText; // Reference to the UI Text component for displaying the day
    public TMP_Text enemyLeftText; // Reference to the UI Text component for displaying the number of enemies left
    private float timer = 0.0f;
    private const float dayDuration = 300.0f; // 5 minutes in seconds
    public GameObject researchUI;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
        enemySpawner = FindObjectOfType<EnemySpawner>();
        UpdateEnemyLeftText();
        enemySpawner.OnEnemyDeath += UpdateEnemyLeftText;
    }
    void OnDisable()
    {
        ResetResearchItems();
    }

    void ResetResearchItems()
    {
        foreach (var item in researchedItems)
        {
            item.isUnlocked = false;
            item.isResearched = false;
            item.isOnProgress = false;
        }
        researchedItems.Clear();
    }
    public void SpeedTime(int speed)
    {
        Time.timeScale = speed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private List<TurretData> pendingItems = new List<TurretData>();
    public void AddResearch(TurretData item)
    {
        if (item.isUnlocked)
        {
            return;
        }
        pendingItems.Add(item);
        item.isResearched = true;
        OnResearchAdded?.Invoke();
    }
    void UpdateClockText()
    {
        int minutes = Mathf.FloorToInt(timer / 60);
        int seconds = Mathf.FloorToInt(timer % 60);
        clockText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void UpdateEnemyLeftText()
    {
        enemyLeftText.text = enemySpawner.enemiesRemaining.ToString();
        if (enemySpawner.enemiesRemaining <= 0)
        {
            enemyLeftText.text = "0";
        }
    }
    public Action OnStateChange;
    public void NextDay()
    {
        if (gameState == GameState.Day)
        {
            gameState = GameState.Night;
            OnStateChange?.Invoke();
            enemySpawner.StartWave(currentDay - 1);
            CameraFollow cam = FindObjectOfType<CameraFollow>();
            cam.MoveToEnemySpawn();
            timer = 0.0f;
        }
        else
        {
            if (enemySpawner.enemiesRemaining > 0)
            {
                return;
            }
            enemySpawner.enemiesRemaining = 0;
            enemySpawner.OnEnemyDeath?.Invoke();
            gameState = GameState.Day;
            currentDay++;
            dayText.text = "Day " + currentDay;
            foreach (var item in pendingItems)
            {
                item.isUnlocked = true;
            }
            timer = 0.0f;
            OnDayChange?.Invoke();
            pendingItems.Clear();
        }
    }

    public GameObject gameOverUI;
    public TMP_Text gameOverStats;
    public void ShowGameOver()
    {
        researchUI.SetActive(false);
        upgradeBuildingUI.gameObject.SetActive(false);
        Time.timeScale = 0;
        gameOverUI.SetActive(true);
        gameOverStats.text = "You survived " + currentDay + " days";

    }

    public void ResetGame()
    {

    }
    public GameObject tutorialUI;
    public void CloseTutorial()
    {
        tutorialUI.SetActive(false);
    }
}

public enum GameState
{
    Day,
    Night
}
