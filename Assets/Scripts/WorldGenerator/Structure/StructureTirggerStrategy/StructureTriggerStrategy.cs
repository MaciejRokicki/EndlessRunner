using UnityEngine;

public class StructureTriggerStrategy
{
    protected StructureTrigger structureTrigger;
    protected PlayerController playerController;

    public StructureTriggerStrategy(StructureTrigger structureTrigger, PlayerController playerController)
    {
        this.structureTrigger = structureTrigger;
        this.playerController = playerController;
    }

    public virtual void OnTriggerEnter(Collider coll) { }
}
