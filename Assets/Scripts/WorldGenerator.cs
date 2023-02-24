using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public int startSize = 25;
    [SerializeField]
    private int rowSize = 5;
    [SerializeField]
    private float height = 1.0f;
    [SerializeField]
    private float heightOffset = 5.0f;
    [HideInInspector]
    public float maxGroundHeight = 0.0f;
    public int z = 0;

    [SerializeField]
    private GameObject world;

    [SerializeField]
    private GameObject ground;

    private void Start()
    {
        for(int i = 0; i < startSize; i++)
        {
            GenerateRow();
        }

        z = startSize;
    }

    public void GenerateRow()
    {
        z++;

        for (int x = 0; x < rowSize; x++)
        {
            float y = height * Mathf.PerlinNoise(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));

            if(maxGroundHeight < y)
            {
                maxGroundHeight = y;
            }

            Instantiate(ground, new Vector3(x, y - heightOffset, z), Quaternion.identity, world.transform).GetComponent<MapObject>().BaseHeight = y;

            world.GetComponent<BoxCollider>().size = new Vector3(rowSize, 1.0f, z);
            world.GetComponent<BoxCollider>().center = new Vector3(2.0f, maxGroundHeight * 0.7f, z / 2.0f);
        }
    }
}
