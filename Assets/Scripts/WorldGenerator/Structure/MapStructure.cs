using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObjects", menuName = "WorldGenerator/MapStructure")]
public class MapStructure : ScriptableObject
{
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
            StructureObjects.Clear();

            if(structure)
            {
                foreach (Transform obj in structure)
                {
                    if(obj.CompareTag("StructureCube"))
                    {
                        StructureObjects.Add(obj);
                    }
                }

                StructureObjects = StructureObjects
                    .OrderBy(x => x.position.z)
                    .OrderBy(x => x.position.y)
                    .ToList();

                Width = Mathf.Abs((int)StructureObjects.Min(x => x.position.x) - (int)StructureObjects.Max(x => x.position.x)) + 1;
                Length = (int)StructureObjects.Max(x => x.position.z);
            }
        }
    }
    [HideInInspector]
    public int Length;
    [HideInInspector]
    public int Width;
    //[HideInInspector]
    public List<Transform> StructureObjects;
    public bool RandomizePosition = true;

    protected void OnValidate()
    {
        Structure = structure;
    }
}
