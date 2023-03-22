using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Singletons
    private WorldGenerator worldGenerator;
    private GameManager gameManager;
    private CharacterController controller;
    #endregion

    //References
    [Header("References")]
    [SerializeField]
    private Camera playerCamera;

    private Vector3 playerVelocity;
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
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
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

        if (groundedPlayer && playerVelocity.y < 0.0f)
        {
            playerVelocity.y = 0.0f;
        }

        Vector3 move = transform.rotation * Vector3.forward;

        controller.Move(move * Time.deltaTime * playerSpeed);

        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}
