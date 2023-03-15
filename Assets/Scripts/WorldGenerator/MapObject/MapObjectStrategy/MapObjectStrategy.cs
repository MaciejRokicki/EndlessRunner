using System;

public abstract class MapObjectStrategy
{
    protected readonly MapObject mapObject;

    public MapObjectStrategy(MapObject mapObject)
    {
        this.mapObject = mapObject;
    }

    public virtual void Update() { }
}