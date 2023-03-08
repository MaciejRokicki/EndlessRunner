using UnityEngine;

public class MapObjectRoundHeightToIntStrategy : MapObjectStrategy
{
    public MapObjectRoundHeightToIntStrategy(MapObject mapObject) : base(mapObject) { }

    public override void Update()
    {
        mapObject.transform.position = new Vector3(mapObject.transform.position.x, Mathf.RoundToInt(mapObject.transform.position.y), mapObject.transform.position.z);

        mapObject.MapObjectStrategy = new MapObjectEmptyStrategy(mapObject);
    }
}
