using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour
{

    public Camera mCamera;

    /// <summary>是否要同步旋转</summary>
    public bool needSynRotate = false;

    /// <summary>设置欧拉方向</summary>
    Quaternion direction = new Quaternion();
    // Use this for initialization
    void Start()
    {
        if (mCamera == null)
        {
            mCamera = Camera.main;
        }
        direction.x = transform.localRotation.x;
        direction.y = transform.localRotation.y;
        direction.z = transform.localRotation.z;
        direction.w = transform.localRotation.w;
    }
    float deltaTime = 0.0f;
    // Update is called once per frame
    void Update()
    {
        Camera cam = null;
        if(mCamera!=null)
        {
            cam = mCamera;
        }
        else
        {
            cam = Camera.current;
            if (!cam)//!cam相当于cam=null
                return;
        }
        deltaTime += Time.deltaTime;
        if (needSynRotate)
        {
            transform.rotation = cam.transform.rotation * new Quaternion(direction.x, direction.y, direction.z - transform.localRotation.x, direction.w);//xz之间始终朝向一个目标点
            //transform.rotation=Mathf.Lerp(cam.transform.rotation,cam.transform.rotation*new Quaternion(direction.x,direction.y,direction.z,direction.w)*Time.deltaTime);
        }           
        else
        {
            transform.rotation = cam.transform.rotation * new Quaternion(direction.x, direction.y, direction.z, direction.w);
        }    
    }
}
