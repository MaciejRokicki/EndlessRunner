using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObjects", menuName = "WorldGenerator/MapStructure")]
public class MapStructure : ScriptableObject
{
    public int Length;
    public List<StructureObject> StructureObjects;

    private void OnValidate()
    {
        StructureObjects = StructureObjects
            .OrderBy(x => x.Position.z)
            .OrderBy(x => x.Position.y)
            .ToList();

        Length = (int)StructureObjects.Max(x => x.Position.z);
    }
}
