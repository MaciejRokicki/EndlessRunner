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
    private GameObject rowPrefab;
    [SerializeField]
    private GameObject colliderPrefab;
    [SerializeField]
    private GameObject mapObjectPrefab;

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
    private float lastStructureZ = 0;

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
    }

    public void GenerateRow(bool animate = true)
    {
        if (Z > StartLength && Z - lastStructureZ > structureOffset)
        {
            int chance = Random.Range(0, 100);

            if (chance > 80)
            {
                MapStructure structure = structures[Random.Range(0, structures.Count)];
                StructureToGenerate structureToGenerate = new StructureToGenerate(instance, structure);
                structuresToGenerate.Add(structureToGenerate);

                structureOffset = Random.Range(MinStructureOffset, MinStructureOffset * 2);
                lastStructureZ = Z + structure.Size.z;
            }
        }

        MapRow mapRow = MapRowPool.Get();

        mapRow.transform.position = new Vector3(GeneratePosition.x, GeneratePosition.y, Z);
        mapRow.Initialize(animate);

        rowContainer.GetComponent<BoxCollider>().size = new Vector3(GroundSize.x, 1.0f, Z);
        rowContainer.GetComponent<BoxCollider>().center = new Vector3(2.0f, GeneratePosition.y + MaxGroundHeight * 0.7f, Z / 2.0f);

        Z++;

        for (int i = 0; i < structuresToGenerate.Count; i++)
        {
            structuresToGenerate[i].Generate();
        }
    }

#region Pools
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
