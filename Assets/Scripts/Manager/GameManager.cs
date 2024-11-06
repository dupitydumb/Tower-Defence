using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class GameManager : MonoBehaviour
{
    public EnemySpawner enemySpawner;
    public Action OnResearchAdded;
    public GameState gameState = GameState.Day;
    public static GameManager instance;
    public List<ResearchItems> researchedItems = new List<ResearchItems>();
    public int currentDay = 1;
    public TMP_Text clockText; // Reference to the UI Text component for displaying the clock
    public TMP_Text dayText; // Reference to the UI Text component for displaying the day
    public TMP_Text enemyLeftText; // Reference to the UI Text component for displaying the number of enemies left
    private float timer = 0.0f;
    private const float dayDuration = 60.0f; // 5 minutes in seconds
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
        enemySpawner = FindObjectOfType<EnemySpawner>();
        dayText.text = "Day " + currentDay;
        UpdateEnemyLeftText();
        enemySpawner.OnEnemyDeath += UpdateEnemyLeftText;
    }

    public void SpeedTime(int speed)
    {
        Time.timeScale = speed;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= dayDuration)
        {
            NextDay();
            timer = 0.0f;
        }

        UpdateClockText();
    }

    private List<ResearchItems> pendingItems = new List<ResearchItems>();
    public void AddResearch(ResearchItems item)
    {
        Debug.Log("Adding research : " + item.names);
        // Wait 1 day before adding the research
        pendingItems.Add(item);
        item.isOnProgress = true;
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
    }

    public void NextDay()
    {
        if (gameState == GameState.Day)
        {
            gameState = GameState.Night;
            enemySpawner.StartWave(currentDay - 1);
            CameraFollow cam = FindObjectOfType<CameraFollow>();
            cam.MoveToEnemySpawn();
        }
        else
        {
            if (enemySpawner.enemiesRemaining > 0)
            {
                return;
            }
            gameState = GameState.Day;
            currentDay++;
            foreach (var item in pendingItems)
            {
                researchedItems.Add(item);
            }
            pendingItems.Clear();
        }
    }
}

public enum GameState
{
    Day,
    Night
}
