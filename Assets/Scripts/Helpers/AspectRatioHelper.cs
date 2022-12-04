using UnityEngine;

public class AspectRatioHelper
{
    public enum ScreenType
    {
        Phone,
        Tablet,
        Phablet,
        IpadTouch,
        ZFoldPhone,
        ZFoldTablet,
        Others
    }

    public static bool isFourThreeAspect()
    {
        int factor = gcd(Screen.width, Screen.height);
        int wFactor = Screen.width / factor;
        int hFactor = Screen.height / factor;

        if (wFactor == 3 && hFactor == 4)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool isSixteenNineAspect()
    {
        int factor = gcd(Screen.width, Screen.height);
        int wFactor = Screen.width / factor;
        int hFactor = Screen.height / factor;

        if (wFactor == 9 && hFactor == 16)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static Vector2 getWHFactor()
    {
        int factor = gcd(Screen.width, Screen.height);
        int wFactor = Screen.width / factor;
        int hFactor = Screen.height / factor;

        return new Vector2(wFactor, hFactor);
    }

    public static int gcd(int a, int b)
    {
        return (b == 0) ? a : gcd(b, a % b);
    }

    public static ScreenType GetScreenType(Vector2 whFactor)
    {
        ScreenType type = ScreenType.Others;
        if ((whFactor.x == 4 && whFactor.y == 3)
            || (whFactor.x == 199 && whFactor.y == 139)
            || (whFactor.x == 59 && whFactor.y == 41)
            || (whFactor.x == 683 && whFactor.y == 512)
            || (whFactor.x == 8 && whFactor.y == 5)
        )
        {
            type = ScreenType.Tablet;
        }
        else if ((whFactor.x == 16 && whFactor.y == 9)
                 || (whFactor.x == 448 && whFactor.y == 207)
                 || (whFactor.x == 422 && whFactor.y == 195)
                 || (whFactor.x == 667 && whFactor.y == 375)
                 || (whFactor.x == 812 && whFactor.y == 375)
                 || (whFactor.x == 2 && whFactor.y == 1)
                 || (whFactor.x == 11 && whFactor.y == 5)
        )
        {
            type = ScreenType.Phone;
        }
        else if ((whFactor.x == 13 && whFactor.y == 6)
                 || (whFactor.x == 19 && whFactor.y == 9)
                 || (whFactor.x == 94 && whFactor.y == 45)
                 || (whFactor.x == 463 && whFactor.y == 214)
                 || (whFactor.x == 1169 && whFactor.y == 540)
        )
        {
            type = ScreenType.Phablet;
        }
        else if ((whFactor.x == 71 && whFactor.y == 40))
        {
            type = ScreenType.IpadTouch;
        }
        else if ((whFactor.x == 443 && whFactor.y == 160))
        {
            type = ScreenType.ZFoldPhone;
        }else if ((whFactor.x == 276 && whFactor.y == 221))
        {
            type = ScreenType.ZFoldTablet;
        }

        return type;
    }
}