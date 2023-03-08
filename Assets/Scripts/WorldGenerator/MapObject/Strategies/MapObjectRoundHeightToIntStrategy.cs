using UnityEngine;

public class MapObjectRoundHeightToIntStrategy : MapObjectStrategy
{
    public MapObjectRoundHeightToIntStrategy(MapObject mapObject) : base(mapObject) { }

    public override void Update()
    {
        mapObject.transform.position = new Vector3(mapObject.transform.position.x, mapObject.BaseHeight, mapObject.transform.position.z);

        mapObject.MapObjectStrategy = new MapObjectEmptyStrategy(mapObject);
    }
}
