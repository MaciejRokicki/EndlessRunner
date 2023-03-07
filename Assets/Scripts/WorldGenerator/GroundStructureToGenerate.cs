using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class GroundStructureToGenerate
{
    private WorldGenerator worldGenerator;

    [SerializeField]
    private MapStructure mapStructure;
    private Vector3 structureGroundSpawnPosition;
    private int z = 0;
    private int zTmp = 0;

    private List<Transform> currentRowObjects;
    private List<MapObject> structureObjects;

    public GroundStructureToGenerate(
        WorldGenerator worldGenerator,
        GroundStructure groundStructure)
    {
        this.worldGenerator = worldGenerator;
        if (groundStructure.RandomizePosition)
        {
            float offset = worldGenerator.GroundSize.x - groundStructure.Width;
            structureGroundSpawnPosition = new Vector3(
                structureGroundSpawnPosition.x,
                structureGroundSpawnPosition.y,
                Mathf.Floor(UnityEngine.Random.Range(0.0f, offset + 1.0f)));
        }
        else
        {
            structureGroundSpawnPosition = worldGenerator.GeneratePosition;
        }
        this.mapStructure = groundStructure;

        structureObjects = new List<MapObject>();

        zTmp = worldGenerator.Z;
    }

    public void Generate()
    {
        if (z <= mapStructure.Length)
        {
            currentRowObjects = mapStructure.StructureObjects
                .Where(x => x.position.z == z)
                .ToList();

            foreach (Transform structureObject in currentRowObjects)
            {
                MapObject mapObject = worldGenerator.StructureMapObjectPool.Get();

                structureObjects.Add(mapObject);

                mapObject.GetComponent<MeshRenderer>().materials = structureObject.GetComponent<Renderer>().sharedMaterials;
                mapObject.transform.position = new Vector3(
                    structureGroundSpawnPosition.x + structureObject.position.x,
                    structureGroundSpawnPosition.y - 4.0f,
                    zTmp + structureObject.position.z);
                mapObject.BaseHeight = structureGroundSpawnPosition.y + structureObject.position.y + 1.0f;
                mapObject.RoundHeightToInt = structureObject.GetComponent<MapObject>().RoundHeightToInt;
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
        if (structureObjects[structureObjects.Count - 1].transform.position.z < worldGenerator.PlayerController.transform.position.z - 2.0f)
        {
            foreach (MapObject structureObject in structureObjects)
            {
                worldGenerator.StructureMapObjectPool.Release(structureObject);
            }

            worldGenerator.RemoveGroundStructureToGenerate(this);
        }
    }
}
