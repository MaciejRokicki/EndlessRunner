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
    private float speedUpTimer = 10.0f;
    private float currentSpeedUpTimer = 0.0f;
    [SerializeField]
    private float changeGameOverTimeTimer = 20.0f;
    private float currentChangeGameOverTimeTimer = 0.0f;

    [Header("Distances")]
    [SerializeField]
    private float changeStructureTierChanceDistance = 250.0f;
    private float lastChangeStructureTierChanceDistance = 0.0f;
    [SerializeField]
    private float changeMinStructureOffsetDistance = 300.0f;
    private float lastChangeMinStructureOffsetDistance = 0.0f;

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

        lastChangeStructureTierChanceDistance = worldGenerator.StartLength + changeStructureTierChanceDistance;
        lastChangeMinStructureOffsetDistance = worldGenerator.StartLength + changeMinStructureOffsetDistance;
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
            TimersAndDistanceHandler();
            CheckGameOver();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !PressAnyKeyToPlay && !IsGameOver && !pressAnyKeyStartTimer)
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

    private void TimersAndDistanceHandler()
    {
        gameTimer += Time.deltaTime;
        currentSpeedUpTimer += Time.deltaTime;
        currentChangeGameOverTimeTimer += Time.deltaTime;

        OnGameTimerChange(gameTimer);

        if (currentSpeedUpTimer > speedUpTimer)
        {
            SpeedUp();
            currentSpeedUpTimer = 0.0f;
        }

        if (currentChangeGameOverTimeTimer > changeGameOverTimeTimer)
        {
            GameOverTimer -= 0.1f;

            if(GameOverTimer <= 0.5f)
            {
                GameOverTimer = 0.5f;
            }

            currentChangeGameOverTimeTimer = 0.0f;
        }

        if(worldGenerator.Z > lastChangeStructureTierChanceDistance)
        {
            ChangeStructureTierChances();
            lastChangeStructureTierChanceDistance = worldGenerator.Z + changeStructureTierChanceDistance;
        }

        if (worldGenerator.Z > lastChangeMinStructureOffsetDistance)
        {
            worldGenerator.MinStructureOffset -= 1;

            if(worldGenerator.MinStructureOffset < 3)
            {
                worldGenerator.MinStructureOffset = 3;
            }

            lastChangeMinStructureOffsetDistance = worldGenerator.Z + changeMinStructureOffsetDistance;
        }
    }

    private void CheckGameOver()
    {
        if(playerController.transform.position.y < worldGenerator.GeneratePosition.y - 3.0f)
        {
            IsGameOver = !IsGameOver;
        }

        if (gameTimer > 1.0f && playerController.PlayerVelocity.z < 0.5f)
        {
            OnGameOverTimerChange(currentGameOverTimer);

            currentGameOverTimer += Time.deltaTime;

            if (!IsGameOver && currentGameOverTimer > GameOverTimer)
            {
                IsGameOver = !IsGameOver;
            }
        }
        else
        {
            OnGameOverTimerChange(currentGameOverTimer);

            currentGameOverTimer -= Time.deltaTime;

            if(currentGameOverTimer < 0.0f)
            {
                currentGameOverTimer = 0.0f;
            }
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
