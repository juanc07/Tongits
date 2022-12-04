using UnityEngine;

public class SpriteRendererHelper
{
    public static Vector3 GetChildCenter(Transform targetTransform)
    {
        var center = Vector3.zero;
        foreach (Transform child in targetTransform)
        {
            center += child.gameObject.GetComponent<SpriteRenderer>().bounds.center;
        }
        center /= targetTransform.childCount; //center is average center of children
        return center;
    }
    
    public static float GetTotalChildWidth(Transform targetTransform)
    {
        var totalWidth = 0f;
        foreach (Transform child in targetTransform)
        {
            totalWidth += (child.gameObject.GetComponent<SpriteRenderer>().bounds.size.x);
        }
        return totalWidth;
    }
}
