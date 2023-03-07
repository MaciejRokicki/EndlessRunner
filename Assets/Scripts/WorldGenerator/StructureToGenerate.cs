using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class StructureToGenerate
{
    private WorldGenerator worldGenerator;

    [SerializeField]
    private MapStructure mapStructure;
    private float structureGroundSpawnPositionX;
    private int z = 0;
    private int zTmp = 0;

    private List<Transform> currentRowObjects;
    private List<MapObject> structureObjects;

    public StructureToGenerate(
        WorldGenerator worldGenerator, 
        float structureGroundSpawnPositionX, 
        MapStructure mapStructure)
    {
        this.worldGenerator = worldGenerator;
        if(mapStructure.RandomizePosition)
        {
            float offset = worldGenerator.GroundWidth - mapStructure.Width;
            this.structureGroundSpawnPositionX = Mathf.Floor(UnityEngine.Random.Range(0.0f, offset + 1.0f));
        }
        else
        {
            this.structureGroundSpawnPositionX = structureGroundSpawnPositionX;
        }
        this.mapStructure = mapStructure;

        structureObjects = new List<MapObject>();

        zTmp = worldGenerator.Z;
    }

    public void Generate()
    {
        if(z <= mapStructure.Length)
        {
            currentRowObjects = mapStructure.StructureObjects
                .Where(x => x.position.z == z)
                .ToList();

            foreach (Transform structureObject in currentRowObjects)
            {
                MapObject mapObject = worldGenerator.StructureMapObjectPool.Get();

                structureObjects.Add(mapObject);

                mapObject.GetComponent<MeshRenderer>().materials = structureObject.GetComponent<Renderer>().sharedMaterials; 
                mapObject.transform.position = new Vector3(structureGroundSpawnPositionX + structureObject.position.x, -4.0f, zTmp + structureObject.position.z);
                mapObject.BaseHeight = structureObject.position.y + 1.0f;
                mapObject.ShouldRoundHeightToInt = structureObject.GetComponent<MapObject>().ShouldRoundHeightToInt;
            }

            z++;
        }
        else
        {
            CheckPoolRelease();
        }
    }

    private void CheckPoolRelease()
    {
        if(structureObjects[structureObjects.Count - 1].transform.position.z < worldGenerator.PlayerController.transform.position.z - 2.0f)
        {
            foreach(MapObject structureObject in structureObjects)
            {
                worldGenerator.StructureMapObjectPool.Release(structureObject);
            }

            worldGenerator.RemoveStructureToGenerate(this);
        }
    }
}
