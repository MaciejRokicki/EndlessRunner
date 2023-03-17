using System.Collections.Generic;
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

    [Header("Row generation settings")]
    [SerializeField]
    private GameObject environmentObjectPrefab;
    [SerializeField]
    private Transform environmentContainer;
    [SerializeField]
    private GameObject environmentRowPrefab;
    [SerializeField]
    private int length;
    [SerializeField]
    private int circleIterations;
    private float lastEnvironmentRowZ;
    public IObjectPool<GameObject> environmentRowPool;
    private List<GameObject> coneEnvironmentRows = new List<GameObject>(); 

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

    [Header("Environment Object Settings")]
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
        float lastConeEnvironmentRowZ = 0.0f;
        Transform tmp = null;

        int radius = length - 10;

        for (int i = 0; i < length; i++)
        {
            GameObject environmentRow;

            if(i >= length / 4)
            {
                radius = length - i;

                environmentRow = Instantiate(environmentRowPrefab, Vector3.zero, Quaternion.identity, environmentContainer);

                coneEnvironmentRows.Add(environmentRow);
            }
            else
            {
                environmentRow = environmentRowPool.Get();
                tmp = environmentRow.transform;
            }

            environmentRow.transform.position = new Vector3(
                2.0f + worldGenerator.GeneratePosition.x,
                worldGenerator.GeneratePosition.y,
                lastConeEnvironmentRowZ + environmentObjectPrefab.transform.localScale.z
            );

            lastConeEnvironmentRowZ = environmentRow.transform.position.z;

            for (int j = 0; j < circleIterations; j++)
            {
                float progress = (float)j / (circleIterations - 2);
                float rad = progress * 2.0f * Mathf.PI;

                float x = Mathf.Cos(rad) * radius;
                float y = Mathf.Sin(rad) * radius;

                Vector3 pos = new Vector3(x, y, i * environmentObjectPrefab.transform.localScale.z);

                Instantiate(environmentObjectPrefab, pos, Quaternion.identity, environmentRow.transform);
            }
        }

        lastEnvironmentRowZ = tmp.position.z;
    }

    public void GenerateNextEnvironmentRow()
    {
        lastEnvironmentRowZ += environmentObjectPrefab.transform.localScale.z;

        GameObject environmentRow = environmentRowPool.Get();

        environmentRow.transform.position = new Vector3(
            2.0f + worldGenerator.GeneratePosition.x,
            worldGenerator.GeneratePosition.y,
            lastEnvironmentRowZ
        );

        foreach (GameObject coneEnvironmentObject in coneEnvironmentRows)
        {
            coneEnvironmentObject.transform.position = new Vector3(
                2.0f + worldGenerator.GeneratePosition.x,
                worldGenerator.GeneratePosition.y,
                coneEnvironmentObject.transform.position.z + environmentObjectPrefab.transform.localScale.z
            );
        }
    }
}
