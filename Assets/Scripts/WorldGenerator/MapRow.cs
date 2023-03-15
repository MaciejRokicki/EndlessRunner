using System.Collections.Generic;
using UnityEngine;

public class MapRow : MonoBehaviour
{
    private WorldGenerator worldGenerator;
    [SerializeField]
    private List<MapObject> rowObjects;

    private void Awake()
    {
        worldGenerator = WorldGenerator.Instance;

        rowObjects = new List<MapObject>();
    }

    private void Update()
    {
        if (worldGenerator.PlayerController.transform.position.z - 2.0f > transform.position.z)
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
            mapObject.GetComponent<MeshRenderer>().materials = worldGenerator.mapObjectPrefab.GetComponent<Renderer>().sharedMaterials;
            mapObject.transform.position = new Vector3(
                worldGenerator.GeneratePosition.x + x, 
                worldGenerator.GeneratePosition.y + (animate ? y - worldGenerator.HeightOffset : y), 
                worldGenerator.Z);
            mapObject.BaseHeight = y;
            mapObject.transform.localScale = Vector3.one;
            rowObjects.Add(mapObject);
        }
    }
}
