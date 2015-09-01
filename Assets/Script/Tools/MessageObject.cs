using UnityEngine;
using System.Collections;


public interface IMessageObject
{

}

/// <summary>
/// 辅助处理消息类，方便以后Debug调试信息，注意：Unity的Debug.log不受系统脚本控制
/// </summary>
public static class MessageObject {
    
    /// <summary>
    /// 调试方法开始
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t"></param>
    /// <param name="methodName"></param>
    public static void START_METHOD<T>(this T t,string methodName) where T:IMessageObject
    {

        //宏定义，要启用需设置Unity3D_PlayerSettings_Other Settings_Script Define Symbols为NEEDLOGMETHOD
#if NEEDLOGMETHOD
        Debug.Log("Start method "+t.GetType().Name+".Method："+methodName+"======");
#endif

    }

    /// <summary>
    /// 调试方法结束
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t"></param>
    /// <param name="methodName"></param>
    public static void END_METHOD<T>(this T t,string methodName) where T:IMessageObject
    {
#if NEEDLOGMETHOD
        Debug.Log("End method " + t.GetType().Name + ".Method：" + methodName + "======");
#endif
    }

    /// <summary>
    /// 打印调试信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t"></param>
    /// <param name="msg"></param>
    public static void PRINT<T>(this T t,string msg) where T:IMessageObject
    {
#if NEEDLOGMETHOD
        Debug.Log("Method " + t.GetType().Name + ".Message：" + msg);
#endif
    }
}
