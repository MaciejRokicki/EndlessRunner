using UnityEngine;

public class EnvironmentObjectStrategy
{
    protected EnvironmentManager environmentManager;
    protected EnvironmentObject environmentObject;
    protected Vector3 basePosition;

    public EnvironmentObjectStrategy(EnvironmentManager environmentManager, EnvironmentObject environmentObject)
    {
        this.environmentManager = environmentManager;
        this.environmentObject = environmentObject;
        basePosition = this.environmentObject.transform.position;
    }

    public virtual void FixedUpdate() { }
}
