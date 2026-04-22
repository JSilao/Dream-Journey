using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum GameMode { Endless, Level }
    public GameMode gameMode = GameMode.Level;

    [Header("Level Settings")]
    public int currentLevel = 1;
    public float dayDistance = 500f;
    public float afternoonDistance = 500f;
    public float nightDistance = 500f;

    [Header("Difficulty Settings")]
    public float fallingPlatformChance = 0.2f;
    public int maxObstaclesPerGround = 2;
    public float playerMaxSpeed = 100f;

    [HideInInspector]
    public float distanceTraveled = 0f;

    public Player player;

    void Start()
    {
       SetupLevelDifficulty();
    }
    void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Update()
    {
        if (player == null)
        {
            player = GameObject.Find("Player")?.GetComponent<Player>();
            if (player == null) return;

            player.maxXVelocity = playerMaxSpeed;
        }

        distanceTraveled = player.distance;

        if (gameMode == GameMode.Level)
            HandleLevelMode();
        else
            HandleEndlessMode();
    }

    void HandleLevelMode()
{
      float totalDistance = dayDistance + afternoonDistance + nightDistance;
    // Level complete
    if(distanceTraveled >= totalDistance && !player.levelCompleted)
{
    player.levelCompleted = true;
    Debug.Log("Level Complete!");

    if (LevelProgressManager.Instance != null)
    {
        LevelProgressManager.Instance.UnlockNextLevel(currentLevel);
    }

    currentLevel++;
    distanceTraveled = 0;

    if(player != null) player.velocity = Vector2.zero;
}
}

    void HandleEndlessMode()
    {
        float d = distanceTraveled;

        if(d < dayDistance)
        {
            player.maxXVelocity = 50f;
            fallingPlatformChance = 0.1f;
            maxObstaclesPerGround = 1;
        }
        else if(d < dayDistance + afternoonDistance + nightDistance)
        {
            player.maxXVelocity = 70f;
            fallingPlatformChance = 0.25f;
            maxObstaclesPerGround = 2;
        }
        else
        {
            player.maxXVelocity = 90f;
            fallingPlatformChance = 0.4f;
            maxObstaclesPerGround = 3;
        }
    }

    void SetupLevelDifficulty()
    {
        switch(currentLevel)
        {
            case 1:
                playerMaxSpeed = 80f;
                fallingPlatformChance = 0.1f;
                maxObstaclesPerGround = 1;
                break;

            case 2:
                playerMaxSpeed = 100f;
                fallingPlatformChance = 0.25f;
                maxObstaclesPerGround = 2;
                break;

            case 3:
                playerMaxSpeed = 120f;
                fallingPlatformChance = 0.4f;
                maxObstaclesPerGround = 3;
                break;
        }
    }

    
}