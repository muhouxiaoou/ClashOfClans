using UnityEngine;
using System.Collections;

/// <summary>
/// Tips:当前实例在所有场景有效，防止切换后自动销毁
/// </summary>
/// <typeparam name="T"></typeparam>
public class UnityAllSceneSingleton<T> : MonoBehaviour
    where T:Component{//保证所有静态类都为控件，不然会出错
    private static T _Instance;
    public static T Instance
    {
        get
        {
            if(_Instance==null)
            {
                _Instance = FindObjectOfType(typeof(T)) as T;
                if (_Instance==null)
                {
                    GameObject obj =new GameObject();

                    //告诉unity新建gameObject后要做什么
                    //DontSave:表示我做了GameObject这个对象后，不需要保存，切换场景后对象后不丢失
                    //HideAndDontSave:不光不保存，而且还是隐藏的
                    //HideInHierarchy:指在Hierarchy列表中隐藏
                    //NotEditable:造出来后不能被更改，不能在属性面板中更改，也不能在代码中更改
                    obj.hideFlags = HideFlags.HideAndDontSave;//保持GameObject始终保持最顶层，切换场景是不会被删掉

                    _Instance = (T)obj.AddComponent(typeof(T));//强转换成T类型，注意All是GetComponent，One是Add，不知作者有没有搞错，我自己改正为AddComponent
                }
            }
            return _Instance;
        }
    }

    public virtual void Awake()//虚拟化后可以重载
    {
        DontDestroyOnLoad(this.gameObject);

        if(_Instance==null)
        {
            _Instance = this as T;//如果是单场景，当前对象就是单例了，这时候直接取值就行了
        }
        else
        {
            Destroy(gameObject);
        }
    }

}

