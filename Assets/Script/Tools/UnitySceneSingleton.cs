using UnityEngine;
using System.Collections;


/// <summary>
/// Tips:当前实例仅仅在本场景有效，切换后会自动销毁
/// </summary>
/// <typeparam name="T"></typeparam>
public class UnitySceneSingleton<T> : MonoBehaviour
    where T : Component
{
    private static T _Instance;
    public static T Instance
    {
        get
        {
            if(_Instance==null)
            {
                _Instance = FindObjectOfType(typeof(T)) as T;
                if(_Instance==null)
                {
                    GameObject obj = new GameObject();
                    obj.hideFlags = HideFlags.HideAndDontSave;
                    _Instance = obj.AddComponent(typeof(T)) as T;
                }
            }
            return _Instance;
        }
    }
}