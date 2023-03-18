using UnityEngine;

public enum StructureTriggerStrategyType
{
    DefaultStrategy,
    MovementSpeedStrategy
}

public class StructureTrigger : MonoBehaviour
{
    #region Singletons
    private GameManager gameManager;
    #endregion

    //References
    private PlayerController playerController;

    private StructureTriggerStrategy triggerStrategy;
    public StructureTriggerStrategyType StrategyType;
    public float MovementSpeed = 0.0f;

    private void Awake()
    {
        gameManager = GameManager.Instance;
        playerController = gameManager.PlayerController;

        SetStrategyType(StructureTriggerStrategyType.DefaultStrategy);
    }

    public void SetStrategyType(StructureTriggerStrategyType type)
    {
        switch (type)
        {
            case StructureTriggerStrategyType.DefaultStrategy:
                triggerStrategy = new StructureTriggerStrategy(this, playerController);
                break;

            case StructureTriggerStrategyType.MovementSpeedStrategy:
                triggerStrategy = new StructureTriggerMovementSpeedStrategy(this, playerController);
                break;
        }
    }

    private void OnTriggerEnter(Collider coll)
    {
        triggerStrategy.OnTriggerEnter(coll);
    }
}
