using UnityEngine;
using UnityEngine.U2D;

public class AtlasContainer : MonoBehaviour
{
    public SpriteAtlas NumberAtlas;

    public Sprite GetNumberSprite(string key)
    {
        return  NumberAtlas.GetSprite(key);
    }
}
