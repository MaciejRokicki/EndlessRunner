using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObjects", menuName = "WorldGenerator/GroundStructure")]
public class GroundStructure : MapStructure
{
    public List<Transform> paths;
    public List<Transform> triggers;

    private new void OnValidate()
    {
        base.OnValidate();

        paths.Clear();
        triggers.Clear();   

        if (structure)
        {
            foreach (Transform obj in structure)
            {
                if(obj.CompareTag("GroundStructurePath"))
                {
                    paths.Add(obj);
                }
                else if(obj.CompareTag("GroundStructurePathTrigger"))
                {
                    triggers.Add(obj);
                }
            }
        }
    }
}