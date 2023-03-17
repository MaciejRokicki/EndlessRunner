using UnityEngine;

public class EnvironmentObjectSinStrategy : EnvironmentObjectStrategy
{
    public EnvironmentObjectSinStrategy(EnvironmentManager environmentManager, EnvironmentObject environmentObject) : base(environmentManager, environmentObject)
    { }

    public override void FixedUpdate()
    {
        environmentObject.transform.localPosition = 
            new Vector3(
                basePosition.x,
                basePosition.y + Mathf.Sin(Time.fixedTime * environmentManager.Frequency + basePosition.z) * environmentManager.Amplitude, 
                0.0f
            );
    }
}
