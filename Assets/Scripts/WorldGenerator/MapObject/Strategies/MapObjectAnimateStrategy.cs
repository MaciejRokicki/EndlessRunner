using UnityEngine;

public class MapObjectAnimateStrategy : MapObjectStrategy
{
    public MapObjectAnimateStrategy(MapObject mapObject) : base(mapObject) { }

    public override void Update()
    {
        if (mapObject.transform.position.y < mapObject.BaseHeight)
        {
            mapObject.transform.position = mapObject.transform.position + new Vector3(0.0f, mapObject.step * Time.deltaTime * Random.Range(0.5f, 1.5f), 0.0f);
        }
        else
        {
            if (mapObject.ShouldRoundHeightToInt)
            {              
                mapObject.mapObjectStrategy = new MapObjectRoundHeightToIntStrategy(mapObject);
            }
            else
            {
                mapObject.mapObjectStrategy = new MapObjectDefaultStrategy(mapObject);
            }
        }
    }
}