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

    private GameManager gameManager;

    private PlayerController playerController;
    public PlayerController PlayerController
    { 
        get { return playerController; }
        set { playerController = value; }
    }

    public int StartSize = 25;
    public Vector2 GroundSize = new Vector2(5.0f, 1.0f);
    public float HeightOffset = 5.0f;
    [HideInInspector]
    public float MaxGroundHeight = 0.0f;
    public int Z = 0;
    public Vector2 GeneratePosition;

    public IObjectPool<MapRow> MapRowPool;
    public IObjectPool<MapObject> MapObjectPool;
    public IObjectPool<MapObject> StructureMapObjectPool;

    [SerializeField]
    private GameObject groundGameObject;
    [SerializeField]
    private GameObject mapGameObject;
    [SerializeField]
    private GameObject structureMapGameObject;
    [SerializeField]
    private GameObject rowPrefab;
    [SerializeField]
    private GameObject groundPrefab;
    [SerializeField]
    private GameObject structurePrefab;

    [SerializeField]
    private List<MapStructure> mapStructures = new List<MapStructure>();
    private int structureOffset;
    public int MinStructureOffset = 10;
    private int lastStructureZ = 0;

    [SerializeField]
    private List<StructureToGenerate> structuresToGenerate = new List<StructureToGenerate>();

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
        gameManager = GameManager.Instance;

        MapRowPool = new ObjectPool<MapRow>(OnCreateMapRow, OnGetMapRow, OnReturnedMapRow, OnDestroyMapRow);
        MapObjectPool = new ObjectPool<MapObject>(OnCreateMapObject, OnGetMapObject, OnReturnedMapObject, OnDestroyMapObject);
        StructureMapObjectPool = new ObjectPool<MapObject>(OnCreateStructureMapObject, OnGetStructureMapObject, OnReturnedStructureMapObject, OnDestroyStructureMapObject);

        structureOffset = MinStructureOffset;

        for(int i = 0; i < StartSize; i++)
        {
            GenerateRow(false);
        }

        Z = StartSize;
    }

    public void GenerateRow(bool animate = true)
    {
        if(Z > StartSize && Z - lastStructureZ > structureOffset)
        {
            int chance = Random.Range(0, 100);

            if(chance > 80)
            {
                MapStructure mapStructure = mapStructures[Random.Range(0, mapStructures.Count)];
                StructureToGenerate structureToGenerate = new StructureToGenerate(instance, mapStructure);
                structuresToGenerate.Add(structureToGenerate);

                structureOffset = Random.Range(MinStructureOffset, MinStructureOffset * 2);
                lastStructureZ = Z + mapStructure.Length;
            }
        }

        MapRow mapRow = MapRowPool.Get();

        mapRow.transform.position = new Vector3(GeneratePosition.x, GeneratePosition.y, Z);
        mapRow.Initialize(animate);

        groundGameObject.GetComponent<BoxCollider>().size = new Vector3(GroundSize.x, 1.0f, Z);
        groundGameObject.GetComponent<BoxCollider>().center = new Vector3(2.0f, GeneratePosition.y + MaxGroundHeight * 0.7f, Z / 2.0f);

        Z++;

        for (int i = 0; i < structuresToGenerate.Count; i++)
        {
            structuresToGenerate[i].Generate();
        }
    }

    private MapRow OnCreateMapRow()
    {
        MapRow mapRow = Instantiate(rowPrefab, new Vector3(0.0f, 0.0f, Z), Quaternion.identity, groundGameObject.transform).GetComponent<MapRow>();

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
        MapObject mapRow = Instantiate(groundPrefab, Vector3.zero, Quaternion.identity, mapGameObject.transform).GetComponent<MapObject>();

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

    private MapObject OnCreateStructureMapObject()  
    {
        MapObject mapRow = Instantiate(structurePrefab, Vector3.zero, Quaternion.identity, structureMapGameObject.transform).GetComponent<MapObject>();

        return mapRow;
    }

    private void OnGetStructureMapObject(MapObject mapObject)
    {
        mapObject.gameObject.SetActive(true);
    }

    private void OnReturnedStructureMapObject(MapObject mapObject)
    {
        mapObject.gameObject.SetActive(false);
    }

    private void OnDestroyStructureMapObject(MapObject mapObject)
    {
        Destroy(mapObject);
    }

    public void RemoveStructureToGenerate(StructureToGenerate structureToGenerate)
    {
        structuresToGenerate.Remove(structureToGenerate);
    }
}
