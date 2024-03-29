using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObjects", menuName = "WorldGenerator/MapStructure")]
public class MapStructure : ScriptableObject
{
    public StructureTier Tier;
    [SerializeField]
    protected Transform structure;
    public Transform Structure
    {
        get
        {
            return structure;
        }
        set
        {
            structure = value;

            Colliders.Clear();
            ObjectRows = new List<List<Transform>>();

            if (structure)
            {
                List<Transform> structureObjectsTmp = new List<Transform>();

                foreach (Transform obj in structure)
                {
                    if (obj.CompareTag("MapObject"))
                    {
                        structureObjectsTmp.Add(obj);
                    }
                    
                    if(obj.CompareTag("StructureCollider"))
                    {
                        Colliders.Add(obj);
                    }
                }

                structureObjectsTmp = structureObjectsTmp
                    .OrderBy(x => x.position.z)
                    .OrderBy(x => x.position.y)
                    .ToList();

                Size = new Vector3(
                    Mathf.Abs(structureObjectsTmp.Min(x => x.position.x) + structureObjectsTmp.Max(x => x.position.x)) + 1,
                    Mathf.Abs(structureObjectsTmp.Min(x => x.position.y) + structureObjectsTmp.Max(x => x.position.y)) + 1,
                    Mathf.Abs(structureObjectsTmp.Min(x => x.position.z) + structureObjectsTmp.Max(x => x.position.z)) + 1
                );

                for (int i = 0; i < Size.z; i++)
                {
                    ObjectRows.Add(new List<Transform>());
                }

                float zTmp = structureObjectsTmp[0].position.z;

                foreach (Transform structureObject in structureObjectsTmp)
                {
                    ObjectRows[(int)structureObject.position.z].Add(structureObject);
                }

                structureObjectsTmp.Clear();
            }
        }
    }

    [Header("Settings")]
    public bool RandomizePosition = true;
    public bool CustomGround = false;

    [Header("Info")]
    public Vector3 Size;
    [HideInInspector]
    public List<Transform> Colliders;
    [HideInInspector]
    public List<List<Transform>> ObjectRows;

    private void OnEnable()
    {
        Structure = structure;
    }

    private void OnValidate()
    {
        Structure = structure;
    }
}
