using System.Collections.Generic;
using UnityEngine;

public class MapRow : MonoBehaviour
{
    private WorldGenerator worldGenerator;

    private List<MapObject> mapObjects;

    private void Awake()
    {
        worldGenerator = WorldGenerator.Instance;

        mapObjects = new List<MapObject>();
    }

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        if (transform.position.z < worldGenerator.PlayerController.transform.position.z - 2.0f)
        {
            worldGenerator.MapRowPool.Release(this);

            for(int i = 0; i < mapObjects.Count; i++)
            {
                worldGenerator.MapObjectPool.Release(mapObjects[i]);
                mapObjects.RemoveAt(i);
            }
        }
    }

    public void Initialize(bool animate = true)
    {
        for (int x = 0; x < worldGenerator.GroundWidth; x++)
        {
            float y = worldGenerator.Height * Mathf.PerlinNoise(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));

            if (worldGenerator.MaxGroundHeight < y)
            {
                worldGenerator.MaxGroundHeight = y;
            }

            MapObject mapObject = worldGenerator.MapObjectPool.Get();
            mapObject.transform.position = new Vector3(x, animate ? y - worldGenerator.HeightOffset : y, worldGenerator.Z);
            mapObject.BaseHeight = y;
            mapObjects.Add(mapObject);
        }
    }
}
