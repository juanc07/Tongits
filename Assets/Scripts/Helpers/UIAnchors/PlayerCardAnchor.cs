using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(PlayerCardView))]
public class PlayerCardAnchor : MonoBehaviour
{
    public enum AnchorType
    {
        BottomLeft,
        BottomCenter,
        BottomRight,
        MiddleLeft,
        MiddleCenter,
        MiddleRight,
        TopLeft,
        TopCenter,
        TopRight,
    };

    public bool executeInUpdate;

    public AnchorType anchorType;
    private PlayerCardView _playerCardView;
    
    IEnumerator updateAnchorRoutine; //Coroutine handle so we don't start it if it's already running

    // Use this for initialization
    void Start()
    {
        _playerCardView = GetComponent<PlayerCardView>();
        updateAnchorRoutine = UpdateAnchorAsync();
        StartCoroutine(updateAnchorRoutine);
    }

    /// <summary>
    /// Coroutine to update the anchor only once CameraFit.Instance is not null.
    /// </summary>
    IEnumerator UpdateAnchorAsync()
    {

        uint cameraWaitCycles = 0;

        while (CameraViewportHandler.Instance == null)
        {
            ++cameraWaitCycles;
            yield return new WaitForEndOfFrame();
        }

        if (cameraWaitCycles > 0)
        {
            print(string.Format("CameraAnchor found CameraFit instance after waiting {0} frame(s). " +
                "You might want to check that CameraFit has an earlie execution order.", cameraWaitCycles));
        }

        UpdateAnchor();
        updateAnchorRoutine = null;

    }

    void UpdateAnchor()
    {
        switch (anchorType)
        {
            case AnchorType.BottomLeft:
                SetAnchor(CameraViewportHandler.Instance.BottomLeft);
                break;
            case AnchorType.BottomCenter:
                SetAnchor(CameraViewportHandler.Instance.BottomCenter);
                break;
            case AnchorType.BottomRight:
                SetAnchor(CameraViewportHandler.Instance.BottomRight);
                break;
            case AnchorType.MiddleLeft:
                SetAnchor(CameraViewportHandler.Instance.MiddleLeft);
                break;
            case AnchorType.MiddleCenter:
                SetAnchor(CameraViewportHandler.Instance.MiddleCenter);
                break;
            case AnchorType.MiddleRight:
                SetAnchor(CameraViewportHandler.Instance.MiddleRight);
                break;
            case AnchorType.TopLeft:
                SetAnchor(CameraViewportHandler.Instance.TopLeft);
                break;
            case AnchorType.TopCenter:
                SetAnchor(CameraViewportHandler.Instance.TopCenter);
                break;
            case AnchorType.TopRight:
                SetAnchor(CameraViewportHandler.Instance.TopRight);
                break;
        }
    }
    
    Vector3 GetAnchor()
    {
        var foundAnchor = Vector3.zero;
        switch (anchorType)
        { 
            case AnchorType.BottomLeft:
                foundAnchor =  CameraViewportHandler.Instance.BottomLeft;
                break;
            case AnchorType.BottomCenter:
                foundAnchor = CameraViewportHandler.Instance.BottomCenter;
                break;
            case AnchorType.BottomRight:
                foundAnchor = CameraViewportHandler.Instance.BottomRight;
                break;
            case AnchorType.MiddleLeft:
                foundAnchor = CameraViewportHandler.Instance.MiddleLeft;
                break;
            case AnchorType.MiddleCenter:
                foundAnchor = CameraViewportHandler.Instance.MiddleCenter;
                break;
            case AnchorType.MiddleRight:
                foundAnchor = CameraViewportHandler.Instance.MiddleRight;
                break;
            case AnchorType.TopLeft:
                foundAnchor = CameraViewportHandler.Instance.TopLeft;
                break;
            case AnchorType.TopCenter:
                foundAnchor = CameraViewportHandler.Instance.TopCenter;
                break;
            case AnchorType.TopRight:
                foundAnchor = CameraViewportHandler.Instance.TopRight;
                break;
        }

        return foundAnchor;
    }

    void SetAnchor(Vector3 anchor)
    {
        Vector3 newPos = anchor + _playerCardView.AnchorOffset;
        if (!transform.position.Equals(newPos))
        {
            transform.position = newPos;
        }
    }
    
    public void ManualSetAnchor(float centerX)
    {
        Vector3 newPos =GetAnchor() + new Vector3(centerX + _playerCardView.AnchorOffset.x, _playerCardView.AnchorOffset.y, _playerCardView.AnchorOffset.z);
        if (!transform.position.Equals(newPos))
        {
            transform.position = newPos;
        }
    }

#if UNITY_EDITOR
    // Update is called once per frame
    void Update()
    {
        if (updateAnchorRoutine == null && executeInUpdate)
        {
            updateAnchorRoutine = UpdateAnchorAsync();
            StartCoroutine(updateAnchorRoutine);
        }
    }
#endif
}
