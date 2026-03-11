using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    [Header("Backgrounds")]
    public GameObject dayBackground;
    public GameObject afternoonBackground;
    public GameObject nightBackground;

    private Player player;

    public enum Mode { Day, Afternoon, Night }

    // Fix so first SetMode always runs
    public Mode currentMode = (Mode)(-1);

    public float dayLength = 500f;
    public float afternoonLength = 500f;
    public float nightLength = 500f;

    [Header("Afternoon Effects")]
    public ScreenFlash screenFlash;

    [Range(0f, 1f)]
    public float flashChancePerSecond = 0.05f;

    public static EnvironmentManager Instance;

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    void Start()
    {
        player = GameObject.Find("Player")?.GetComponent<Player>();

        // Start in day mode
        SetMode(Mode.Day);
    }

    void Update()
    {
        if (player == null) return;

        float d = player.distance;

        if (GameManager.Instance.gameMode == GameManager.GameMode.Level)
        {
            if (d < GameManager.Instance.dayDistance)
                SetMode(Mode.Day);
            else if (d < GameManager.Instance.dayDistance + GameManager.Instance.afternoonDistance)
                SetMode(Mode.Afternoon);
            else if (d < GameManager.Instance.dayDistance + GameManager.Instance.afternoonDistance + GameManager.Instance.nightDistance)
                SetMode(Mode.Night);
        }
        else
        {
            float cycle = d % (dayLength + afternoonLength + nightLength);

            if (cycle < dayLength)
                SetMode(Mode.Day);
            else if (cycle < dayLength + afternoonLength)
                SetMode(Mode.Afternoon);
            else
                SetMode(Mode.Night);
        }

        // Afternoon flash effect
        if (currentMode == Mode.Afternoon && screenFlash != null)
        {
            if (Random.value < flashChancePerSecond * Time.deltaTime)
            {
                bool bright = Random.value < 0.5f;
                screenFlash.Flash(bright);
            }
        }
    }

    void SetMode(Mode mode)
    {
        if (currentMode == mode) return;

        currentMode = mode;

        dayBackground.SetActive(mode == Mode.Day);
        afternoonBackground.SetActive(mode == Mode.Afternoon);
        nightBackground.SetActive(mode == Mode.Night);
    }
}