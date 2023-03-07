using UnityEngine;

public class MapObject : MonoBehaviour
{
    private float baseHeight;
    public float BaseHeight
    {
        get { return baseHeight; }
        set
        {
            baseHeight = value;
            step = baseHeight - transform.position.y; 
        }
    }

    public MapObjectStrategy mapObjectStrategy;

    [HideInInspector]
    public float step;
    public bool RoundHeightToInt = false;

    private void Start()
    {
        mapObjectStrategy = new MapObjectAnimateStrategy(this);
    }

    private void OnEnable()
    {
        mapObjectStrategy = new MapObjectAnimateStrategy(this);
    }

    private void Update()
    {
        mapObjectStrategy.Update();
    }
}
