using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StructureManager : MonoBehaviour
{
    #region Singletons
    private static StructureManager instance;
    public static StructureManager Instance
    {
        get { return instance; }
    }
    #endregion

    [Header("Tiers & Structures")]
    [SerializeField]
    private List<StructureTier> structureTiers;
    [SerializeField]
    private List<MapStructure> structures = new List<MapStructure>();
    [SerializeField]
    private List<MapStructure> effectStructures = new List<MapStructure>();

    private List<int> structureTierChances = new List<int>();
    private int structureTierMaxChance;
    private Dictionary<StructureTier, List<MapStructure>> structuresGroupedByTier = new Dictionary<StructureTier, List<MapStructure>>();

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
        InitStructureTiers();
        RefreshStructureTierChances();

        foreach (MapStructure mapStructure in structures)
        {
            structuresGroupedByTier[mapStructure.Tier].Add(mapStructure);
        }
    }

    private void InitStructureTiers()
    {
        HashSet<int> structureTierIds = new HashSet<int>();

        foreach (StructureTier structureTier in structureTiers)
        {
            structuresGroupedByTier[structureTier] = new List<MapStructure>();
            structureTierMaxChance += structureTier.Chance;

            if (!structureTierIds.Add(structureTier.OrderId))
            {
                Debug.LogError($"Structure tier's order id is not unique. (StructureTier name: {structureTier.Name})");
            }
        }

        structureTiers = structureTiers
            .OrderBy(x => x.OrderId)
            .ToList();
    }

    private void RefreshStructureTierChances()
    {
        structureTierChances.Clear();

        for (int i = 0; i < structureTiers.Count; i++)
        {
            structureTierChances.Add(structureTiers[i].Chance);

            if (i > 0)
            {
                for (int j = i - 1; j >= 0; j--)
                {
                    structureTierChances[i] += structureTiers[j].Chance;
                }
            }
        }
    }

    public MapStructure GetRandomMapStructure()
    {
        int structureTierChance = Random.Range(0, structureTierMaxChance);
        StructureTier randStuctureTier = structureTiers[0];

        for (int i = 1; i < structureTierChances.Count; i++)
        {
            if (structureTierChance > structureTierChances[i - 1] && structureTierChance <= structureTierChances[i])
            {
                randStuctureTier = structureTiers[i];
                break;
            }
        }

        return structuresGroupedByTier[randStuctureTier][Random.Range(0, structuresGroupedByTier[randStuctureTier].Count)];
    }

    public MapStructure GetRandomEffectStructure()
    {
        return effectStructures[Random.Range(0, effectStructures.Count)];
    }

    public void ChangeStructureTierChance(string structureTierName, int chanceToAdd)
    {
        StructureTier structureTier = structureTiers
            .Where(x => x.Name == structureTierName)
            .FirstOrDefault();

        structureTier.Chance += chanceToAdd;

        structureTier.Chance = structureTier.Chance < 1 ? 1 : structureTier.Chance;

        RefreshStructureTierChances();
    }
}
