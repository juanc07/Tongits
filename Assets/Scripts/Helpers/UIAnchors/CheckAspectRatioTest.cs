using UnityEngine;

public class CheckAspectRatioTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vector2 whFactor = AspectRatioHelper.getWHFactor();
        Debug.Log($"w: {whFactor.x} h: {whFactor.y}");
        var screenType = AspectRatioHelper.GetScreenType(whFactor);
        Debug.Log($"screenType: {screenType}");
    }
}