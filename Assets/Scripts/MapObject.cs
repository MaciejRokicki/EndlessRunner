using UnityEngine;

public class MapObject : MonoBehaviour
{
    [SerializeField]
    private float baseHeight;
    public float BaseHeight
    {
        get
        {
            return baseHeight;
        }
        set
        {
            baseHeight = value;
            step = baseHeight - transform.position.y; 
        }
    }
    [SerializeField]
    private float step;

    private void Update()
    {
        if(transform.position.y < baseHeight)
        {
            transform.position = transform.position + new Vector3(0.0f, step * Time.deltaTime * Random.Range(0.1f, 0.75f), 0.0f);
        }
    }
}
