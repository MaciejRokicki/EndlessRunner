using UnityEngine;
using UnityEngine.Pool;

public class EnvironmentManager : MonoBehaviour
{
    private static EnvironmentManager instance;
    public static EnvironmentManager Instance
    {
        get { return instance; }
    }

    private WorldGenerator worldGenerator;

    [SerializeField]
    private Transform environmentContainer;
    [SerializeField]
    private GameObject environmentRowPrefab;

    [SerializeField]
    private int length;
    private float lastEnvironmentRowZ;
    public IObjectPool<GameObject> environmentRowPool;

    #region Pool
    private GameObject OnCreateEnvironmentRow()
    {
        GameObject environmentRow = Instantiate(environmentRowPrefab, Vector3.zero, Quaternion.identity, environmentContainer);

        return environmentRow;
    }

    private void OnGetEnvironmentRow(GameObject environmentRow)
    {
        environmentRow.gameObject.SetActive(true);
    }

    private void OnReturnedEnvironmentRow(GameObject environmentRow)
    {
        environmentRow.gameObject.SetActive(false);
    }

    private void OnDestroyEnvironmentRow(GameObject environmentRow)
    {
        Destroy(environmentRow);
    }
    #endregion

    public float Amplitude = 1.0f;
    public float Frequency = 1.0f;

    public float NoiseHeight = 2.0f;

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

        worldGenerator = WorldGenerator.Instance;

        environmentRowPool = new ObjectPool<GameObject>(OnCreateEnvironmentRow, OnGetEnvironmentRow, OnReturnedEnvironmentRow, OnDestroyEnvironmentRow);
    }

    private void Start()
    {
        for(int i = 0; i < length; i++)
        {
            GenerateNextEnvironmentRow();
        }
    }

    public void GenerateNextEnvironmentRow()
    {
        GameObject environmentRow = environmentRowPool.Get();

        environmentRow.transform.position = new Vector3(worldGenerator.GeneratePosition.x, worldGenerator.GeneratePosition.y, lastEnvironmentRowZ + 4.0f);

        lastEnvironmentRowZ = environmentRow.transform.position.z;
    }
}
