using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveToTargetTest : MonoBehaviour
{
    public List<Transform> Targets = new List<Transform>();
    public Ease easeType;
    public float Duration = 1f;
    public Vector3 initialPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        DOTween.Init(true, true, null);
        initialPosition = transform.localPosition;
    }

    public void ResetPosition()
    {
        transform.localPosition = initialPosition;
    }

    public void Move()
    {
        var randomTarget = Random.Range(0, 3);
        var localPosition = Targets[randomTarget].localPosition;
        transform.DOMove(new Vector3(localPosition.x,localPosition.y,localPosition.z), Duration)
            .SetEase(easeType)
            .OnStart(OnStartTween)
            .OnComplete(OnCompleteTween);
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
