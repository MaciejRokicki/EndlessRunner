using UnityEngine;

public class EnvironmentObjectSinStrategy : EnvironmentObjectStrategy
{
    private Vector3 originalTransform;

    public EnvironmentObjectSinStrategy(EnvironmentManager environmentManager, EnvironmentObject environmentObject) : base(environmentManager, environmentObject)
    {
        originalTransform = environmentObject.transform.position;
    }

    public override void FixedUpdate()
    {
        environmentObject.transform.position = originalTransform + 
            new Vector3(0, Mathf.Sin(Time.fixedTime * environmentManager.Frequency + originalTransform.z) * environmentManager.Amplitude, 0);
    }
}
