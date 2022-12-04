using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectSprite : MonoBehaviour
{
    private SpriteRenderer _renderer;
    public float Width = 0;
    public float Height = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        Width = _renderer.bounds.size.x;
        Height = _renderer.bounds.size.y;
    }
}
