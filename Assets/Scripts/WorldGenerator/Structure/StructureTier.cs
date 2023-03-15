using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObjects", menuName = "WorldGenerator/StructureTier")]
public class StructureTier : ScriptableObject
{
    public int OrderId;
    public string Name;
    [SerializeField]
    private int chance;
    [HideInInspector]
    public int Chance;

    private void OnEnable()
    {
        Chance = chance;
    }
}
