using UnityEngine;

public class NumberView : MonoBehaviour
{
    public AtlasContainer AtlasContainer; 
    
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = AtlasContainer.GetNumberSprite("number1");
    }
}
