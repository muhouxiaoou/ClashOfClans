using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;


/// <summary>
/// 池化资源，让多余的资源不会被销毁，以便循环利用——对象池实现
/// </summary>
public class MemoryManager : UnityAllSceneSingleton<MemoryManager>,IMessageObject {

	// 创建对象池
    List<object> structs = new List<object>();
    Dictionary<string, List<object>> structLists = new Dictionary<string, List<object>>();


    /// <summary>
    /// 创建方法，构建本地化对象(此方法实例结构体)
    /// </summary>
    /// <param name="className"></param>
    /// <returns></returns>
    public object CreateNativeStruct(string className)
    {
        //执行到这个方法的时候调试输出
        this.START_METHOD("CreateNativeStruct");

        //构建反射，用代码去探测我们的类名，对象，变量等等
        //反射通常在系统核心化构件中起作用，因为不知道别人起的类名，方法名， 这时候反射机制就起作用了
        Type type = Type.GetType(className);

        //暂定开100个对象，策划人物平衡
        if(structs.Count<=100)
        {
            //如果不包含这个类就构建，然后进队列
            if (!structLists.ContainsKey(className))
            {
                structLists.Add(className, structs);
            }
            else
            {
                //否则拿出来
                structs = structLists[className];
            }
            //拿出来后实例化对象，用Activator创建具现化对象,原版Activator.CreateInstance(Type.GetType(className));
            object ob = Activator.CreateInstance(type);
            structs.Add(ob);
            this.END_METHOD("CreateNativeStruct");//打标记，表示方法结束了
            return ob;
        }
        throw new UnityException("try to create wrong struct");//对象超过100个对象超出异常
    }

    public override void Awake()
    {
        base.Awake();

        ////启动后延迟120毫秒，并每300毫秒执行一次
        //InvokeRepeating("ResizeDic", 120, 300);//通过ResizeDic方法不停回收释放的垃圾
    }

    ///// <summary>
    ///// 回收释放的垃圾
    ///// </summary>
    //void ResizeDic()
    //{

    //}

}
