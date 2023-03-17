using UnityEngine;

public class EnvironmentRow : MonoBehaviour
{
    private GameManager gameManager;
    private EnvironmentManager environmentManager;

    public Vector3? dest = null;

    private void Awake()
    {
        gameManager = GameManager.Instance;
        environmentManager = EnvironmentManager.Instance;
    }

    private void FixedUpdate()
    {
        if(dest != null)
        {
            transform.position += dest.Value * Time.fixedDeltaTime;

            if(transform.position.z > dest.Value.z)
            {
                dest = null;
            }
        }

        if (gameManager.PlayerController.transform.position.z > transform.position.z + 10.0f)
        {
            environmentManager.environmentRowPool.Release(gameObject);
            environmentManager.GenerateNextEnvironmentRow();
        }
    }
}
