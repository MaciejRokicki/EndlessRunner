using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    #region Singletons
    private static EnvironmentManager instance;
    public static EnvironmentManager Instance
    {
        get { return instance; }
    }

    private WorldGenerator worldGenerator;
    #endregion

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

    private List<GameObject> nonConeEnvironmentRows = new List<GameObject>();
    private List<GameObject> coneEnvironmentRows = new List<GameObject>();

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
    }

    private void Start()
    {     
        GenerateStartEnvironment();
    }

    private void GenerateStartEnvironment()
    {
        float lastEnvironmentRowZ = -environmentObjectPrefab.transform.localScale.z;
        int radius = length - 10;

        for (int i = 0; i < length; i++)
        {
            GameObject environmentRow = Instantiate(environmentRowPrefab, Vector3.zero, Quaternion.identity, environmentContainer);

            if (i >= length / 4)
            {
                radius = length - i - 1;

                coneEnvironmentRows.Add(environmentRow);
            }
            else
            {
                nonConeEnvironmentRows.Add(environmentRow);
            }

            environmentRow.transform.position = new Vector3(
                2.0f + worldGenerator.GeneratePosition.x,
                worldGenerator.GeneratePosition.y,
                lastEnvironmentRowZ + environmentObjectPrefab.transform.localScale.z
            );

            lastEnvironmentRowZ = environmentRow.transform.position.z;

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
    }

    public void GenerateNextEnvironmentRow()
    {
        foreach (GameObject nonConeEnvironmentRow in nonConeEnvironmentRows)
        {
            nonConeEnvironmentRow.GetComponent<EnvironmentRow>().destinationPosition = new Vector3(
                2.0f + worldGenerator.GeneratePosition.x,
                worldGenerator.GeneratePosition.y,
                nonConeEnvironmentRow.transform.position.z + environmentObjectPrefab.transform.localScale.z
            );
        }

        foreach (GameObject coneEnvironmentRow in coneEnvironmentRows)
        {
            coneEnvironmentRow.GetComponent<EnvironmentRow>().destinationPosition = new Vector3(
                2.0f + worldGenerator.GeneratePosition.x,
                worldGenerator.GeneratePosition.y,
                coneEnvironmentRow.transform.position.z + environmentObjectPrefab.transform.localScale.z
            );
        }
    }
}
