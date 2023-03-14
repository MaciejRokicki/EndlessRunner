using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private WorldGenerator worldGenerator;

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    [SerializeField]
    private Camera playerCamera;

    [SerializeField]
    private float playerSpeed;
    public float PlayerSpeed 
    { 
        get { return playerSpeed; } 
        set 
        { 
            playerSpeed = value; 
            timer = 1 / (playerSpeed + worldGenerator.StartLength);
        } 
    }
    [SerializeField]
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;
    [SerializeField]
    private float cameraSpeed = 250.0f;

    private float timer = 0.0f;
    private float currentTimer = 0.0f;

    private Vector3 lastPosition;
    //TODO: przeniesc do GameManager'a
    private float gameOverTimer = 1.0f;
    private float currentGameOverTimer = 0.0f;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        worldGenerator = WorldGenerator.Instance;

        lastPosition = transform.position;
        PlayerSpeed = 8.0f;
    }

    void Update()
    {
        RotateCharacter();
        MoveCharacter();
        GenerateMap();

        //TODO: przeniesc do GameManager'a
        if(lastPosition.z + 0.1f > transform.position.z)
        {
            currentGameOverTimer += Time.deltaTime;

            if(currentGameOverTimer > gameOverTimer)
            {
                Debug.Log("Game Over");
            }
        }
        else
        {
            currentGameOverTimer = 0.0f;
        }

        lastPosition = transform.position;
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

    private void GenerateMap()
    {
        if(currentTimer > timer)
        {
            if (worldGenerator.Z - transform.position.z < worldGenerator.StartLength)
            {
                worldGenerator.GenerateRow();
            }

            currentTimer = 0.0f;
        }

        currentTimer += Time.deltaTime;
    }
}
