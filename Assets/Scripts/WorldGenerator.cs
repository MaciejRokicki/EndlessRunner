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
    public int GroundWidth = 5;
    public float Height = 1.0f;
    public float HeightOffset = 5.0f;
    [HideInInspector]
    public float MaxGroundHeight = 0.0f;
    public int Z = 0;

    public IObjectPool<MapRow> MapRowPool;
    public IObjectPool<MapObject> MapObjectPool;

    [SerializeField]
    private GameObject groundGameObject;
    [SerializeField]
    private GameObject mapObjectGameObject;
    [SerializeField]
    private GameObject rowPrefab;
    [SerializeField]
    private GameObject groundPrefab;

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

        playerController = gameManager.PlayerController;

        MapRowPool = new ObjectPool<MapRow>(OnCreateMapRow, OnGetMapRow, OnReturnedMapRow, OnDestroyMapRow);
        MapObjectPool = new ObjectPool<MapObject>(OnCreateMapObject, OnGetMapObject, OnReturnedMapObject, OnDestroyMapObject);

        for(int i = 0; i < StartSize; i++)
        {
            GenerateRow(false);
        }

        Z = StartSize;
    }

    public void GenerateRow(bool animate = true)
    {
        MapRow mapRow = MapRowPool.Get();

        mapRow.transform.position = new Vector3(0.0f, 0.0f, Z);
        mapRow.Initialize(animate);

        groundGameObject.GetComponent<BoxCollider>().size = new Vector3(GroundWidth, 1.0f, Z);
        groundGameObject.GetComponent<BoxCollider>().center = new Vector3(2.0f, MaxGroundHeight * 0.7f, Z / 2.0f);
        Z++;
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
        MapObject mapRow = Instantiate(groundPrefab, Vector3.zero, Quaternion.identity, mapObjectGameObject.transform).GetComponent<MapObject>();

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
}
