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
    private float centerPathPosition;
    private int z = 0;
    private int zTmp = 0;

    private List<StructureObject> currentRowObjects;
    private List<MapObject> structureObjects;

    public StructureToGenerate(
        WorldGenerator worldGenerator, 
        float centerPathPosition, 
        MapStructure mapStructure)
    {
        this.worldGenerator = worldGenerator;
        this.centerPathPosition = centerPathPosition;
        this.mapStructure = mapStructure;

        structureObjects = new List<MapObject>();

        zTmp = worldGenerator.Z;
    }

    public void Generate()
    {
        if(z <= mapStructure.Length)
        {
            currentRowObjects = mapStructure.StructureObjects
                .Where(x => x.Position.z == z)
                .ToList();

            foreach(StructureObject structureObject in currentRowObjects)
            {
                MapObject mapObject = worldGenerator.StructureMapObjectPool.Get();

                structureObjects.Add(mapObject);

                mapObject.transform.position = new Vector3(centerPathPosition + structureObject.Position.x, -4.0f, zTmp + structureObject.Position.z);
                mapObject.BaseHeight = structureObject.Position.y + 1.0f;
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
