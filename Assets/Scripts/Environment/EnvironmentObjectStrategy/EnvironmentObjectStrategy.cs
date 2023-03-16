public class EnvironmentObjectStrategy
{
    protected EnvironmentManager environmentManager;
    protected EnvironmentObject environmentObject;

    public EnvironmentObjectStrategy(EnvironmentManager environmentManager, EnvironmentObject environmentObject)
    {
        this.environmentManager = environmentManager;
        this.environmentObject = environmentObject;
    }

    public virtual void FixedUpdate() { }
}
