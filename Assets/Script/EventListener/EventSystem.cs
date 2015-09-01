using UnityEngine;
using System.Collections;

namespace Core
{
    /// <summary>
    /// 事件监听类，注意C#事件监听是不按顺序执行的
    /// partial表示这个类只是一部分
    /// </summary>
    public partial class EventSystem
    {
        //下面三种代理是以后游戏开发常用的代理写法
        public delegate void NoParamDelegate();//没有参数的代理
        public delegate void OneParamDelegate<T>(T t);//只有一个参数代理
        //other delegate
        public delegate void TargetChangeHandle(Vector3 pos, bool isTerrian);//切换目标会用这个代理，在地形上切换目标，点击人物后在地上放人
    }

}
