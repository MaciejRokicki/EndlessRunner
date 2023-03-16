using UnityEngine;

public enum EnvironmentObjectType
{
    Sin,
    Noise
}

public class EnvironmentObject : MonoBehaviour
{
    private EnvironmentManager environmentManager;
    private EnvironmentObjectStrategy environmentObjectStrategy;

    [SerializeField]
    private EnvironmentObjectType environmentObjectType;
    public EnvironmentObjectType EnvironmentObjectType
    {
        get
        {
            return environmentObjectType;
        }
        set
        {
            switch(value)
            {
                case EnvironmentObjectType.Sin:
                    environmentObjectStrategy = new EnvironmentObjectSinStrategy(environmentManager, this);
                    break;
                case EnvironmentObjectType.Noise:
                    environmentObjectStrategy = new EnvironmentObjectNoiseStrategy(environmentManager, this);
                    break;
            }
        }
    }

    private void Start()
    {
        environmentManager = EnvironmentManager.Instance;

        EnvironmentObjectType = environmentObjectType;
    }

    private void FixedUpdate()
    {
        environmentObjectStrategy.FixedUpdate();
    }
}
