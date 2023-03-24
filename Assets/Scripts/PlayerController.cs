using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Singletons
    private WorldGenerator worldGenerator;
    private GameManager gameManager;
    #endregion

    //References
    [Header("References")]
    private CharacterController controller;
    [SerializeField]
    private Camera playerCamera;

    private Vector3 playerVelocityTmp;
    private Vector3 playerVelocity;
    public Vector3 PlayerVelocity
    {
        get { return playerVelocity; }
        private set { playerVelocity = value; }
    }
    private bool groundedPlayer;

    //Player settings
    [Header("Player settings")]
    [SerializeField]
    private float playerSpeed;
    public float PlayerSpeed 
    { 
        get { return playerSpeed; } 
        set 
        { 
            playerSpeed = value;
            playerSpeed = Mathf.Clamp(playerSpeed, 5.0f, 25.0f);

            worldGenerator.SetGeneratingRowTimer(playerSpeed);
        } 
    }
    [SerializeField]
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;
    [SerializeField]
    private float cameraSpeed = 250.0f;
    private float playerRotY = 0.0f;
    [SerializeField]
    private float minPlayerRotY = -60.0f;
    [SerializeField]
    private float maxPlayerRotY = 60.0f;
    private float cameraRotX = 0.0f;
    [SerializeField]
    private float minCameraRotX = -50.0f;
    [SerializeField]
    private float maxCameraRotX = 50.0f;

    private void Awake()
    {
        worldGenerator = WorldGenerator.Instance;
        gameManager = GameManager.Instance;
        controller = GetComponent<CharacterController>();
    }

    private void Start()
    {
        playerVelocityTmp.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocityTmp * Time.deltaTime);
    }

    void Update()
    {
        if(!gameManager.IsPause && !gameManager.IsGameOver)
        {
            RotateCharacter();
            MoveCharacter();
        }
    }

    private void RotateCharacter()
    {
        playerRotY += Input.GetAxis("Mouse X") * Time.deltaTime * cameraSpeed;
        playerRotY = Mathf.Clamp(playerRotY, minPlayerRotY, maxPlayerRotY);

        transform.rotation = Quaternion.Euler(0.0f, playerRotY, 0.0f);

        cameraRotX -= Input.GetAxis("Mouse Y") * Time.deltaTime * cameraSpeed;
        cameraRotX = Mathf.Clamp(cameraRotX, minCameraRotX, maxCameraRotX);

        playerCamera.transform.rotation = Quaternion.Euler(cameraRotX, playerRotY, 0.0f);
    }

    private void MoveCharacter()
    {
        groundedPlayer = controller.isGrounded;

        if (groundedPlayer && playerVelocityTmp.y < 0.0f)
        {
            playerVelocityTmp.y = 0.0f;
        }

        Vector3 move = transform.rotation * Vector3.forward * playerSpeed;

        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocityTmp.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocityTmp.y += gravityValue * Time.deltaTime;
        controller.Move((move + playerVelocityTmp) * Time.deltaTime);
        PlayerVelocity = controller.velocity;
    }
}
