using UnityEngine;

public class StructureTriggerMovementSpeedStrategy : StructureTriggerStrategy
{
    public StructureTriggerMovementSpeedStrategy(StructureTrigger structureTrigger, PlayerController playerController) : base(structureTrigger, playerController) { }

    public override void OnTriggerEnter(Collider coll)
    {
        if(coll.CompareTag("Player"))
        {
            playerController.PlayerSpeed += structureTrigger.MovementSpeed;
        }
    }
}
