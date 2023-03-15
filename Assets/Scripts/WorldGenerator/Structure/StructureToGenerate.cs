using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StructureToGenerate
{
    private WorldGenerator worldGenerator;

    [SerializeField]
    private MapStructure mapStructure;
    private Vector3 structureGroundSpawnPosition;
    private float zTmp = 0;

    private int currentRowId = 0;
    private List<GameObject> colliders;
    private List<MapObject> structureObjects;

    public StructureToGenerate(
        WorldGenerator worldGenerator,
        MapStructure mapStructure)
    {
        this.worldGenerator = worldGenerator;
        this.mapStructure = mapStructure;

        if (mapStructure.RandomizePosition)
        {
            float offset = worldGenerator.GroundSize.x - mapStructure.Size.x;

            structureGroundSpawnPosition = new Vector3(
                worldGenerator.GeneratePosition.x + Mathf.Floor(UnityEngine.Random.Range(0.0f, offset + 1.0f)),
                worldGenerator.GeneratePosition.y + structureGroundSpawnPosition.y,
                structureGroundSpawnPosition.z);
        }
        else
        {
            structureGroundSpawnPosition = worldGenerator.GeneratePosition;
        }

        colliders = new List<GameObject>();
        structureObjects = new List<MapObject>();

        zTmp = worldGenerator.Z;
    }

    public void Generate()
    {
        if (currentRowId != mapStructure.Size.z)
        {
            if (currentRowId == 0)
            {
                foreach (Transform baseCollider in mapStructure.Colliders)
                {
                    GameObject collider = worldGenerator.ColliderPool.Get();
                    colliders.Add(collider);

                    collider.transform.position = new Vector3(
                       structureGroundSpawnPosition.x + baseCollider.position.x,
                       structureGroundSpawnPosition.y + baseCollider.position.y + 1.0f,
                       zTmp + baseCollider.position.z);

                    StructureTrigger baseStructureTrigger = baseCollider.GetComponent<StructureTrigger>();
                    StructureTrigger structureTrigger = collider.GetComponent<StructureTrigger>();

                    structureTrigger.StrategyType = baseStructureTrigger.StrategyType;
                    structureTrigger.SetStrategyType(structureTrigger.StrategyType);
                    structureTrigger.MovementSpeed = baseStructureTrigger.MovementSpeed;

                    BoxCollider baseBoxCollider = baseCollider.GetComponent<BoxCollider>();
                    BoxCollider boxCollider = collider.GetComponent<BoxCollider>();

                    boxCollider.isTrigger = baseBoxCollider.isTrigger;
                    boxCollider.size = baseBoxCollider.size;
                }
            }

            foreach (Transform structureObject in mapStructure.ObjectRows[currentRowId])
            {
                MapObject mapObject = worldGenerator.MapObjectPool.Get();
                structureObjects.Add(mapObject);

                mapObject.GetComponent<MeshRenderer>().materials = structureObject.GetComponent<Renderer>().sharedMaterials;
                mapObject.transform.position = new Vector3(
                    structureGroundSpawnPosition.x + structureObject.position.x,
                    structureGroundSpawnPosition.y - 4.0f,
                    zTmp + structureObject.position.z);

                float y = structureGroundSpawnPosition.y + structureObject.position.y + 1;

                if (mapStructure.CustomGround)
                {
                    y += worldGenerator.GroundSize.y *
                        Mathf.PerlinNoise(UnityEngine.Random.Range(-1.0f, 1.0f), UnityEngine.Random.Range(-1.0f, 1.0f));
                }

                if (worldGenerator.MaxGroundHeight < y)
                {
                    worldGenerator.MaxGroundHeight = y;
                }

                mapObject.BaseHeight = y;
                mapObject.RoundHeightToInt = structureObject.GetComponent<MapObject>().RoundHeightToInt;
                mapObject.transform.localScale = new Vector3(0.9999f, 0.9999f, 0.9999f);
            }

            currentRowId++;
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
            foreach (GameObject onStartMapObject in colliders)
            {
                worldGenerator.ColliderPool.Release(onStartMapObject);
            }

            foreach (MapObject structureObject in structureObjects)
            {
                worldGenerator.MapObjectPool.Release(structureObject);
            }

            worldGenerator.RemoveStructureToGenerate(this);
        }
    }
}
