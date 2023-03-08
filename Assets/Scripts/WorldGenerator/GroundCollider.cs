using UnityEngine;

public class GroundCollider : MonoBehaviour
{
    private WorldGenerator worldGenerator;
    private BoxCollider boxCollider;

    private void Awake()
    {
        worldGenerator = WorldGenerator.Instance;
        boxCollider = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        if(worldGenerator.PlayerController.transform.position.z > transform.position.z + boxCollider.size.z / 2.0f + 2.0f)
        {
            worldGenerator.GroundColliderPool.Release(gameObject);
        }
    }
}
