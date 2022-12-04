using UnityEngine;

public class UIAnchorHelper
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
    }
    
    public static Vector3 GetAnchorType(AnchorType type)
    {
        Vector3 anchor;
        switch (type)
        {
            case AnchorType.BottomLeft:
                anchor =  CameraViewportHandler.Instance.BottomLeft;
                break;
            case AnchorType.BottomCenter:
                anchor = CameraViewportHandler.Instance.BottomCenter;
                break;
            case AnchorType.BottomRight:
                anchor = CameraViewportHandler.Instance.BottomRight;
                break;
            case AnchorType.MiddleLeft:
                anchor = CameraViewportHandler.Instance.MiddleLeft;
                break;
            case AnchorType.MiddleCenter:
                anchor = CameraViewportHandler.Instance.MiddleCenter;
                break;
            case AnchorType.MiddleRight:
                anchor = CameraViewportHandler.Instance.MiddleRight;
                break;
            case AnchorType.TopLeft:
                anchor = CameraViewportHandler.Instance.TopLeft;
                break;
            case AnchorType.TopCenter:
                anchor = CameraViewportHandler.Instance.TopCenter;
                break;
            case AnchorType.TopRight:
                anchor = CameraViewportHandler.Instance.TopRight;
                break;
            default: 
                anchor =CameraViewportHandler.Instance.MiddleCenter;
                break;
        }
        return anchor;
    }
}
