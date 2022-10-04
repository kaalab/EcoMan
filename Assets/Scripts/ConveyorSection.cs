using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorSection : MonoBehaviour
{

    public float Width { get => SpriteRendererComp.bounds.size.x; }
    public float Height { get => SpriteRendererComp.bounds.size.y; }
    public float MaxY { get => SpriteRendererComp.bounds.max.y; }
    public float MinY { get => SpriteRendererComp.bounds.min.y; }
    //public Bounds bounds { get => GetComponent<SpriteRenderer>().bounds; }
    //public Transform transform { get => this.transform; }

    public void Move(Vector2 position)
    {
        Debug.Log($"position={position}");
        SpriteRendererComp.transform.position = position;
    }
    
    private SpriteRenderer _spriteRenderer;
    private SpriteRenderer SpriteRendererComp 
    {
        get 
        {
            if (_spriteRenderer == null)
                _spriteRenderer = GetComponent<SpriteRenderer>();
            return _spriteRenderer; 
        }
    }

    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        //sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
