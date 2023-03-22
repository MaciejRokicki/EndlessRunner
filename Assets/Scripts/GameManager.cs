using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singletons
    private static GameManager instance;
    public static GameManager Instance
    {
        get { return instance; }
    }

    private WorldGenerator worldGenerator;
    private StructureManager structureManager;
    private UIManager uiManager;
    #endregion

    //References
    private PlayerController playerController;
    public PlayerController PlayerController
    {
        get { return playerController; }
        set { playerController = value; }
    }

    [Header("Prefabs")]
    [SerializeField]
    private GameObject playerPrefab;

    [Header("Game settings")]
    [SerializeField]
    private float startPlayerSpeed = 8.0f;

    [SerializeField]
    private bool pressAnyKeyToPlay;
    public bool PressAnyKeyToPlay
    {
        get { return pressAnyKeyToPlay; }
        private set
        {
            pressAnyKeyToPlay = value;  

            uiManager.TogglePressAnyKey();
        }
    }
    [SerializeField]
    private bool isPause;
    public bool IsPause
    {
        get { return isPause; }
        set
        {
            isPause = value;

            if(!pressAnyKeyToPlay)
            {
                uiManager.ToggleGameUI();
                uiManager.TogglePause();
            }
        }
    }

    [SerializeField]
    private bool isGameOver;
    public bool IsGameOver
    {
        get { return isGameOver; }
        set
        {
            isGameOver = value;

            if(isGameOver)
            {
                uiManager.ToggleGameUI();
                uiManager.ToggleGameOver();
            }
        }
    }

    [Header("Timers")]
    [SerializeField]
    private bool pressAnyKeyStartTimer = false;
    [SerializeField]
    private float pressAnyKeyTimer = 3.0f;
    private float currentPressAnyKeyTimer = 0.0f;
    public delegate void PressAnyKeyTimerCallback(float time);
    public event PressAnyKeyTimerCallback OnPressAnyKeyTimerChange;
    private float gameTimer = 0.0f;
    public delegate void GameTimerChangeCallback(float time);
    public event GameTimerChangeCallback OnGameTimerChange;
    [SerializeField]
    public float GameOverTimer = 1.0f;
    private float currentGameOverTimer = 0.0f;
    public delegate void GameOverTimerChangeCallback(float time);
    public event GameOverTimerChangeCallback OnGameOverTimerChange;
    [SerializeField]
    private float speedUpTimer = 15.0f;
    private float currentSpeedUpTimer = 0.0f;
    [SerializeField]
    private float changeStructureTierChanceTimer = 60.0f;
    private float currentChangeStructureTierChanceTimer = 0.0f;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        worldGenerator = WorldGenerator.Instance;
        structureManager = StructureManager.Instance;
        uiManager = UIManager.Instance;
        uiManager.SetGameManager(instance);

        PressAnyKeyToPlay = true;
        IsPause = true;
        IsGameOver = false;
    }

    private void Start()
    {
        playerController = Instantiate(playerPrefab,
            new Vector3(Mathf.Floor(worldGenerator.GroundSize.x / 2.0f), worldGenerator.GeneratePosition.y + worldGenerator.GroundSize.y, 1.0f),
            Quaternion.identity)
            .GetComponent<PlayerController>();

        worldGenerator.PlayerController = playerController;
        worldGenerator.SetGeneratingRowTimer(playerController.PlayerSpeed);
    }

    private void Update()
    {
        if(pressAnyKeyStartTimer)
        {
            currentPressAnyKeyTimer += Time.deltaTime;

            OnPressAnyKeyTimerChange(Mathf.CeilToInt(pressAnyKeyTimer - currentPressAnyKeyTimer));

            if(currentPressAnyKeyTimer > pressAnyKeyTimer)
            {
                playerController.PlayerSpeed = startPlayerSpeed;
                pressAnyKeyStartTimer = false;
                isPause = false;
                uiManager.ToggleGameUI();
            }
        }

        if (!IsPause && !IsGameOver)
        {
            TimersHandler();
            CheckGameOver();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !IsGameOver && !pressAnyKeyStartTimer)
        {
            IsPause = !IsPause;
        }

        if (PressAnyKeyToPlay && Input.anyKeyDown)
        {
            pressAnyKeyStartTimer = true;
            uiManager.TogglePressAnyKeyTimerLabel();
            PressAnyKeyToPlay = !PressAnyKeyToPlay;
        }   
    }

    private void TimersHandler()
    {
        gameTimer += Time.deltaTime;
        currentSpeedUpTimer += Time.deltaTime;
        currentChangeStructureTierChanceTimer += Time.deltaTime;

        OnGameTimerChange(gameTimer);

        if (currentSpeedUpTimer > speedUpTimer)
        {
            SpeedUp();
            currentSpeedUpTimer = 0.0f;
        }

        if (currentChangeStructureTierChanceTimer > changeStructureTierChanceTimer)
        {
            ChangeStructureTierChances();
            currentChangeStructureTierChanceTimer = 0.0f;
        }
    }

    private void CheckGameOver()
    {
        if(playerController.transform.position.y < worldGenerator.GeneratePosition.y - 3.0f)
        {
            IsGameOver = !IsGameOver;
        }

        if (playerController.PlayerVelocity.z == 0.0f)
        {
            currentGameOverTimer += Time.deltaTime;

            OnGameOverTimerChange(currentGameOverTimer);

            if (!IsGameOver && currentGameOverTimer > GameOverTimer)
            {
                IsGameOver = !IsGameOver;
            }
        }
        else
        {
            currentGameOverTimer -= Time.deltaTime;
            currentGameOverTimer = Mathf.Clamp(currentGameOverTimer, 0.0f, GameOverTimer);

            OnGameOverTimerChange(currentGameOverTimer);
        }
    }

    private void SpeedUp()
    {
        playerController.PlayerSpeed += 0.5f;
    }

    private void ChangeStructureTierChances()
    {
        structureManager.ChangeStructureTierChance("Easy", -5);
        structureManager.ChangeStructureTierChance("Medium", 10);
        structureManager.ChangeStructureTierChance("Hard", 5);
    }
}
