using UnityEngine;
using System.Collections;

/// <summary>
/// PlatformUtil：IsTouchDevice判断是否触摸设备
/// </summary>
public static class PlatformUtil
{
    /// <summary>
    /// 判断是否触摸设备，现在只预设了IOS和Android
    /// </summary>
    public static bool IsTouchDevice
    {
        get
        {
            return Application.platform == RuntimePlatform.IPhonePlayer ||
                Application.platform == RuntimePlatform.Android;
        }
    }
}

/// <summary>
/// 按键属于接口
/// </summary>
public interface IGameInput
{
    /// <summary> 定义鼠标点击事件 </summary>
    bool IsClickDown { get; }
    /// <summary> 定义鼠标点击事件 </summary>
    bool IsClickUp { get; }

    /// <summary> 长按事件 </summary>
    bool IsClicking { get; }

    /// <summary> 是否触碰 </summary>
    bool HasTouch { get; }

    //跟Touch分开是为了移动时不触发点击事件
    bool IsMove { get; }
    Vector3 MousePosition { get; }

    /// <summary> 返回几个手机触碰 </summary>
    int TouchCount { get; }
}

/// <summary>
/// Window系统下的操作事件
/// </summary>
public class WinGameInput:IGameInput
{
    //边界，以后做成表格控制器
    public static float EdgeLeftX = 20.2f;
    public static float EdgeRightX = 38.3f;
    public static float EdgeDownY = 7.6f;
    public static float EdgeUpY = 21.6f;
    /// <summary>摄像机移动速度 </summary>
    public static float CameraMoveSpeed = 0.3f;
    public static float EdgeWidth = 0.5f;

    /// <summary>
    /// 鼠标按下
    /// </summary>
    public bool IsClickDown
    {
        get
        {
            return Input.GetMouseButtonDown(0);//0左键，1右键，2中键
        }
    }
    /// <summary>
    /// 鼠标抬起
    /// </summary>
    public bool IsClickUp
    {
        get
        {
            return Input.GetMouseButtonUp(0);
        }
    }
    public bool IsMove
    {
        get
        {
            return IsClicking;//表示一直按着
        }
    }
    public bool IsClicking
    {
        get
        {
            return Input.GetMouseButton(0);
        }
    }
    public Vector3 MousePosition
    {
        get
        {
            return Input.mousePosition;
        }
    }
    public bool HasTouch { get { return true; } }//Windows没有触摸事件，所以一定返回True
    public int TouchCount { get { return 1; } }//Windows下只有一个点
}

public class SingleTouchGameInput:IGameInput
{
    public bool IsClickDown
    {
        get
        {
            //Input.touchCount == 1是不是只有一个手指
            return Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began;
        }
    }
    public bool IsClickUp
    {
        get
        {
            return Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended;
        }
    }
    public bool IsMove
    {
        get
        {
            return Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved;
        }
    }
    public bool IsClicking
    {
        get
        {//TouchPhase.Stationary正处在激活状态中
            return Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Stationary;
        }
    }
    public Vector3 MousePosition
    {
        get
        {
            if(Input.touchCount==1)
            {
                return Input.GetTouch(0).position;
            }
            else
            {
                return Input.mousePosition;
            }
        }
    }
    public bool HasTouch { get { return Input.touchCount>0; } }
    public int TouchCount { get { return Input.touchCount; } }
}

