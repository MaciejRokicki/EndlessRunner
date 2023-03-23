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
        transform.Rotate(new Vector3(0.0f, Input.GetAxis("Mouse X"), 0.0f) * Time.deltaTime * cameraSpeed);

        float cameraRotX = Input.GetAxis("Mouse Y") * Time.deltaTime * cameraSpeed;

        playerCamera.transform.Rotate(-cameraRotX, 0.0f, 0.0f);
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
