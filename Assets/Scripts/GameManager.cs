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

    //Pause
    private bool pressAnyKeyToPlay = true;
    [HideInInspector]
    public bool Pause = true;

    [Header("Timers")]
    [SerializeField]
    private float gameTimer = 0.0f;
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
        if (!Pause)
        {
            TimersHandler();
            CheckGameOver();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause = !Pause;
        }

        if (pressAnyKeyToPlay && Input.anyKeyDown)
        {
            pressAnyKeyToPlay = false;
            Pause = false;
            playerController.PlayerSpeed = startPlayerSpeed;
        }
    }

    private void TimersHandler()
    {
        gameTimer += Time.deltaTime;
        currentSpeedUpTimer += Time.deltaTime;
        currentChangeStructureTierChanceTimer += Time.deltaTime;

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

            if (currentGameOverTimer > gameOverTimer)
            {
                GameOver();
            }
        }
        else
        {
            currentGameOverTimer = 0.0f;
        }

        lastPosition = playerController.transform.position;
    }

    private void GameOver()
    {
        Debug.Log("Game Over");
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
