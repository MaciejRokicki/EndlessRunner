using UnityEngine;

public class EnvironmentObjectNoiseStrategy : EnvironmentObjectStrategy
{
    public EnvironmentObjectNoiseStrategy(EnvironmentManager environmentManager, EnvironmentObject environmentObject) : base(environmentManager, environmentObject)
    { }

    public override void FixedUpdate()
    {
        environmentObject.transform.localPosition =
            new Vector3(
                basePosition.x,
                basePosition.y + Mathf.PerlinNoise(basePosition.x + Time.fixedTime, basePosition.z + Time.fixedTime) * environmentManager.NoiseHeight,
                0.0f
            );
    }
}
