using System.Collections.Generic;
using UnityEngine;

public class MapRow : MonoBehaviour
{
    private WorldGenerator worldGenerator;
    private List<MapObject> rowObjects;

    private void Awake()
    {
        worldGenerator = WorldGenerator.Instance;

        rowObjects = new List<MapObject>();
    }

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        if (transform.position.z < worldGenerator.PlayerController.transform.position.z - 2.0f)
        {
            for(int i = 0; i < rowObjects.Count; i++)
            {
                worldGenerator.MapObjectPool.Release(rowObjects[i]);
            }

            rowObjects.Clear();
            worldGenerator.MapRowPool.Release(this);
        }
    }

    public void Initialize(bool animate = true)
    {
        for (int x = 0; x < worldGenerator.GroundSize.x; x++)
        {
            float y = worldGenerator.GeneratePosition.y + worldGenerator.GroundSize.y * Mathf.PerlinNoise(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));

            if (worldGenerator.MaxGroundHeight < y)
            {
                worldGenerator.MaxGroundHeight = y;
            }

            MapObject mapObject = worldGenerator.MapObjectPool.Get();
            mapObject.transform.position = new Vector3(
                worldGenerator.GeneratePosition.x + x, 
                worldGenerator.GeneratePosition.y + (animate ? y - worldGenerator.HeightOffset : y), 
                worldGenerator.Z);
            mapObject.BaseHeight = y;
            rowObjects.Add(mapObject);
        }
    }
}
