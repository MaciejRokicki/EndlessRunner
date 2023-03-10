using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class WorldGenerator : MonoBehaviour
{
    private static WorldGenerator instance;
    public static WorldGenerator Instance
    {
        get { return instance; }
    }

    private PlayerController playerController;
    public PlayerController PlayerController
    {
        get { return playerController; }
        set { playerController = value; }
    }

    [SerializeField]
    private GameObject groundColliderPrefab;
    [SerializeField]
    private GameObject rowPrefab;
    [SerializeField]
    private GameObject colliderPrefab;
    [SerializeField]
    private GameObject mapObjectPrefab;

    [SerializeField]
    private GameObject groundColliderContainer;
    [SerializeField]
    private GameObject rowContainer;
    [SerializeField]
    private GameObject mapObjectContainer;
    [SerializeField]
    private GameObject colliderContainer;

    public int StartLength = 25;
    public Vector2 GroundSize = new Vector2(5.0f, 1.0f);
    public float HeightOffset = 5.0f;
    [HideInInspector]
    public float MaxGroundHeight = 0.0f;
    public float Z = 0;
    public Vector2 GeneratePosition;
    private float lastChangeGeneratePositionZ = 0.0f;

    private GameObject currentGroundCollider;

    public IObjectPool<GameObject> GroundColliderPool;
    public IObjectPool<MapRow> MapRowPool;
    public IObjectPool<MapObject> MapObjectPool;
    public IObjectPool<GameObject> StructureOnStartMapObjectPool;
    public IObjectPool<GameObject> ColliderPool;

    [SerializeField]
    private List<MapStructure> structures = new List<MapStructure>();
    [SerializeField]
    private List<StructureToGenerate> structuresToGenerate = new List<StructureToGenerate>();
    private int structureOffset;
    public int MinStructureOffset = 10;
    private float lastStructureZ = 0.0f;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        GroundColliderPool = new ObjectPool<GameObject>(OnCreateGroundCollider, OnGetGroundCollider, OnReturnedGroundCollider, OnDestroyGroundCollider);
        MapRowPool = new ObjectPool<MapRow>(OnCreateMapRow, OnGetMapRow, OnReturnedMapRow, OnDestroyMapRow);
        MapObjectPool = new ObjectPool<MapObject>(OnCreateMapObject, OnGetMapObject, OnReturnedMapObject, OnDestroyMapObject);
        StructureOnStartMapObjectPool = new ObjectPool<GameObject>(
            OnCreateStructureOnStartMapObject, 
            OnGetStructureOnStartMapObject, 
            OnReturnedStructureOnStartMapObject, 
            OnDestroyStructureOnStartMapObject);
        ColliderPool = new ObjectPool<GameObject>(
            OnCreateCollider,
            OnGetCollider,
            OnReturnedCollider,
            OnDestroyCollider);

        structureOffset = MinStructureOffset;

        for (int i = 0; i < StartLength; i++)
        {
            GenerateRow(false);
        }

        Z = StartLength;

        currentGroundCollider = GroundColliderPool.Get();

        currentGroundCollider.transform.position = new Vector3(
            GeneratePosition.x + 2.0f, 
            GeneratePosition.y, 
            StartLength / 2.0f);

        currentGroundCollider.GetComponent<BoxCollider>().size = new Vector3(GroundSize.x, 1.0f, StartLength + 1.0f);
    }

    public void GenerateRow(bool animate = true)
    {
        Z++;

        if (Z > StartLength)
        {
            if (Z - lastChangeGeneratePositionZ > 10.0f)
            {
                if(Random.Range(0, 100) > 90)
                {
                    GeneratePosition.x += Random.Range(0, 100) <= 50 ? -7.0f : 7.0f;
                }
                else
                {
                    if (Random.Range(0, 100) <= 50)
                    {
                        GeneratePosition.x += Random.Range(0, 100) <= 50 ? -1.0f : 1.0f;
                    }
                    else
                    {
                        GeneratePosition.y += Random.Range(0, 100) <= 50 ? -0.75f : 0.75f;
                    }
                }

                int randLength = Random.Range(5, 30) + 10;
                lastChangeGeneratePositionZ = Z + randLength - 10.0f;
                currentGroundCollider = GroundColliderPool.Get();

                currentGroundCollider.transform.position = new Vector3(
                    GeneratePosition.x + 2.0f,
                    GeneratePosition.y,
                    Z + randLength / 2.0f);

                currentGroundCollider.GetComponent<BoxCollider>().size = new Vector3(GroundSize.x, 1.0f, randLength + 1.0f);
            }

            if (Z - lastStructureZ > structureOffset)
            {
                if (Random.Range(0, 100) > 80)
                {
                    MapStructure structure = structures[Random.Range(0, structures.Count)];
                    StructureToGenerate structureToGenerate = new StructureToGenerate(instance, structure);
                    structuresToGenerate.Add(structureToGenerate);

                    structureOffset = Random.Range(MinStructureOffset, MinStructureOffset + 2);
                    lastStructureZ = Z + structure.Size.z;
                }
            }
        }

        MapRow mapRow = MapRowPool.Get();

        mapRow.transform.position = new Vector3(GeneratePosition.x, GeneratePosition.y, Z);
        mapRow.Initialize(animate);

        for (int i = 0; i < structuresToGenerate.Count; i++)
        {
            structuresToGenerate[i].Generate();
        }
    }

#region Pools
    private GameObject OnCreateGroundCollider()
    {
        GameObject groundCollider = Instantiate(groundColliderPrefab, Vector3.zero, Quaternion.identity, this.groundColliderContainer.transform);

        return groundCollider;
    }

    private void OnGetGroundCollider(GameObject groundCollider)
    {
        groundCollider.gameObject.SetActive(true);
    }

    private void OnReturnedGroundCollider(GameObject groundCollider)
    {
        groundCollider.gameObject.SetActive(false);
    }

    private void OnDestroyGroundCollider(GameObject groundCollider)
    {
        Destroy(groundCollider);
    }

    private MapRow OnCreateMapRow()
    {
        MapRow mapRow = Instantiate(rowPrefab, new Vector3(0.0f, 0.0f, Z), Quaternion.identity, rowContainer.transform).GetComponent<MapRow>();

        return mapRow;
    }

    private void OnGetMapRow(MapRow mapRow)
    {
        mapRow.gameObject.SetActive(true);
    }

    private void OnReturnedMapRow(MapRow mapRow)
    {
        mapRow.gameObject.SetActive(false);
    }

    private void OnDestroyMapRow(MapRow mapRow)
    {
        Destroy(mapRow);
    }


    private MapObject OnCreateMapObject()
    {
        MapObject mapRow = Instantiate(mapObjectPrefab, Vector3.zero, Quaternion.identity, mapObjectContainer.transform).GetComponent<MapObject>();

        return mapRow;
    }

    private void OnGetMapObject(MapObject mapObject)
    {
        mapObject.gameObject.SetActive(true);
    }

    private void OnReturnedMapObject(MapObject mapObject)
    {
        mapObject.gameObject.SetActive(false);
    }

    private void OnDestroyMapObject(MapObject mapObject)
    {
        Destroy(mapObject);
    }


    private GameObject OnCreateStructureOnStartMapObject()
    {
        GameObject onStartMapObject = Instantiate(new GameObject(), Vector3.zero, Quaternion.identity, mapObjectContainer.transform);

        return onStartMapObject;
    }

    private void OnGetStructureOnStartMapObject(GameObject onStartMapObject)
    {
        onStartMapObject.gameObject.SetActive(true);
    }

    private void OnReturnedStructureOnStartMapObject(GameObject onStartMapObject)
    {
        onStartMapObject.gameObject.SetActive(false);
    }

    private void OnDestroyStructureOnStartMapObject(GameObject onStartMapObject)
    {
        Destroy(onStartMapObject);
    }


    private GameObject OnCreateCollider()
    {
        GameObject onStartMapObject = Instantiate(colliderPrefab, Vector3.zero, Quaternion.identity, colliderContainer.transform);

        return onStartMapObject;
    }

    private void OnGetCollider(GameObject onStartMapObject)
    {
        onStartMapObject.gameObject.SetActive(true);
    }

    private void OnReturnedCollider(GameObject onStartMapObject)
    {
        onStartMapObject.gameObject.SetActive(false);
    }

    private void OnDestroyCollider(GameObject onStartMapObject)
    {
        Destroy(onStartMapObject);
    }
    #endregion

    public void RemoveStructureToGenerate(StructureToGenerate structureToGenerate)
    {
        structuresToGenerate.Remove(structureToGenerate);
    }
}
