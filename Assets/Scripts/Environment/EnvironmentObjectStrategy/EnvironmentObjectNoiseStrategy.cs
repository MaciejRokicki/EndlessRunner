using UnityEngine;

public class EnvironmentObjectNoiseStrategy : EnvironmentObjectStrategy
{
    private Vector3 originalTransform;

    public EnvironmentObjectNoiseStrategy(EnvironmentManager environmentManager, EnvironmentObject environmentObject) : base(environmentManager, environmentObject)
    {
        originalTransform = environmentObject.transform.position;
    }

    public override void FixedUpdate()
    {
        environmentObject.transform.position = originalTransform +
            new Vector3(0, Mathf.PerlinNoise(originalTransform.x + Time.fixedTime, originalTransform.z + Time.fixedTime) * environmentManager.NoiseHeight, 0);
    }
}
