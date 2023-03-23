using UnityEngine;

public class EnvironmentRow : MonoBehaviour
{
    #region Singletons
    private GameManager gameManager;
    private EnvironmentManager environmentManager;
    #endregion

    public Vector3? destinationPosition = null;

    private void Awake()
    {
        gameManager = GameManager.Instance;
        environmentManager = EnvironmentManager.Instance;
    }

    private void Update()
    {
        if (destinationPosition != null)
        {
            transform.position = Vector3.Lerp(transform.position, destinationPosition.Value, gameManager.PlayerController.PlayerSpeed * Time.deltaTime);

            if (transform.position.z > destinationPosition.Value.z)
            {
                destinationPosition = null;
            }
        }
    }

    private void FixedUpdate()
    {
        if (gameManager.PlayerController.transform.position.z > transform.position.z + 10.0f)
        {
            environmentManager.GenerateNextEnvironmentRow();
        }
    }
}
