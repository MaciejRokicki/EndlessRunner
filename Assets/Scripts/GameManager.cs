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

    [Header("Timers")]
    [SerializeField]
    private float gameTimer = 0.0f;
    public delegate void GameTimerChangeCallback(float time);
    public event GameTimerChangeCallback OnGameTimerChange;
    [SerializeField]
    private float gameOverTimer = 1.0f;
    private float currentGameOverTimer = 0.0f;
    [SerializeField]
    private float speedUpTimer = 15.0f;
    private float currentSpeedUpTimer = 0.0f;
    [SerializeField]
    private float changeStructureTierChanceTimer = 60.0f;
    private float currentChangeStructureTierChanceTimer = 0.0f;

     //Other
    private Vector3 lastPosition;

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
    }

    private void Start()
    {
        playerController = Instantiate(playerPrefab,
            new Vector3(Mathf.Floor(worldGenerator.GroundSize.x / 2.0f), worldGenerator.GeneratePosition.y + worldGenerator.GroundSize.y, 1.0f),
            Quaternion.identity)
            .GetComponent<PlayerController>();

        worldGenerator.PlayerController = playerController;
        worldGenerator.SetGeneratingRowTimer(playerController.PlayerSpeed);

        lastPosition = playerController.transform.position;
    }

    private void Update()
    {
        if (!uiManager.IsPause && !uiManager.IsGameOver)
        {
            TimersHandler();
            CheckGameOver();
        }

        if (uiManager.PressAnyKeyToPlay && Input.anyKeyDown)
        {
            playerController.PlayerSpeed = startPlayerSpeed;
            uiManager.TogglePressAnyKey();
            uiManager.ToggleGameUI();
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
        if (lastPosition.z + 0.1f > playerController.transform.position.z)
        {
            currentGameOverTimer += Time.deltaTime;

            if (!uiManager.IsGameOver && currentGameOverTimer > gameOverTimer)
            {
                uiManager.ToggleGameOver();
            }
        }
        else
        {
            currentGameOverTimer = 0.0f;
        }

        lastPosition = playerController.transform.position;
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
