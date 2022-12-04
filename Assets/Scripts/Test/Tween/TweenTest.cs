using DG.Tweening;
using UnityEngine;

public class TweenTest : MonoBehaviour
{
    public Ease easeType;
    public int loop;
    public float targetX;
    // Start is called before the first frame update
    void Start()
    {
        DOTween.Init(true, true, null);
        //transform.DOMoveX(5, 1);
        
        
        transform.DOMove(new Vector3(targetX,transform.position.y,transform.position.z), 1f)
            .SetEase(easeType)
            .SetLoops(loop)
            .OnStart(OnStartTween)
            .OnComplete(OnCompleteTween);
        
        // Same as the previous examples, but force the transform to
        // snap on integer values (very useful for pixel perfect stuff)
        /*transform.DOMove(new Vector3(5,transform.position.y,transform.position.z), 1f)
            .SetOptions(true)
            .SetEase(Ease.InSine)
            .SetLoops(4)
            .OnComplete(myFunction);*/
    }

    private void OnStartTween()
    {
        Debug.Log("OnStartTween");
    }

    private void OnCompleteTween()
    {
        Debug.Log($"OnCompleteTween {easeType.ToString()}");
    }
}
