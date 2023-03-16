using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private GameManager gameManager;
    private EnvironmentManager environmentManager;

    private bool isNextRowSpawned = false;

    private void Awake()
    {
        gameManager = GameManager.Instance;
        environmentManager = EnvironmentManager.Instance;
    }

    private void OnEnable()
    {
        isNextRowSpawned = false;
    }

    private void FixedUpdate()
    {
        //if(gameManager.PlayerController.transform.position.z > transform.position.z + 2.0f)
        //{
        //    environmentManager.environmentRowPool.Release(gameObject);
        //}

        if (!isNextRowSpawned && gameManager.PlayerController.transform.position.z > transform.position.z + 1.0f)
        {
            environmentManager.GenerateNextEnvironmentRow();
            isNextRowSpawned = true;
        }
    }
}
