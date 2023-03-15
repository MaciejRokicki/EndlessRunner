using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    { 
        get { return instance; } 
    }

    private WorldGenerator worldGenerator;
    private PlayerController playerController;
    public PlayerController PlayerController 
    { 
        get { return playerController; } 
        set { playerController = value; } 
    }

    [SerializeField]
    private GameObject playerPrefab;

    private void Awake()
    {
        if(instance != null && instance != this) 
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        worldGenerator = WorldGenerator.Instance;

        playerController = Instantiate(playerPrefab, 
            new Vector3(Mathf.Floor(worldGenerator.GroundSize.x / 2.0f), worldGenerator.GeneratePosition.y + worldGenerator.GroundSize.y, 1.0f), 
            Quaternion.identity)
            .GetComponent<PlayerController>();

        worldGenerator.PlayerController = playerController;
        worldGenerator.SetGeneratingrowTimer(playerController.PlayerSpeed);
    }
}
