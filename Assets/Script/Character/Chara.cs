using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// 统管所有角色类
/// </summary>
public class Chara : IMessageObject,IComparable<Chara> {

    /// <summary>人物基础层，默认为3 </summary>
    protected static int LAYER_BASE = 3;
    
    protected CharaData data;//控制数据的

    /// <summary>实体模型 </summary>
    protected GameObject model;
    /// <summary>血条 </summary>
    protected GameObject blood;

    protected CharaStatus status;//控制表现

    /// <summary>渲染层顺序 </summary>
    protected int layerOrder;

    /// <summary>人物死亡后回调委托 </summary>
    public delegate void OnDieHandle();
    /// <summary>人物死亡事件 </summary>
    public OnDieHandle OnDieEvent;

    static long id;
    long mId;
    public long ID { get { return mId; } }

    /// <summary>本地坐标位置 </summary>
    public Vector3 GetLocalPos() { return model.transform.localPosition; }
    /// <summary>世界坐标位置 </summary>
    public Vector3 GetRealPos() { return model.transform.position; }

    private bool bNeedChange = false;
    private bool bInited = false;
    public Chara()
    {
        this.START_METHOD("chara");
        data = (CharaData)MemoryManager.Instance.CreateNativeStruct("CharaData");
        id++;
        mId = id;
        bInited = false;
        this.END_METHOD("Chara");
    }
	public virtual void Start () {
        if (data.classtype == (int)CharaData.CharClassType.CHARACTER)
            mId += 10000;
	}
    public virtual void Destroy()
    {
        GameObject.Destroy(model);
    }
    public Vector3 GetPos()
    {
        return data.pos;
    }

    public virtual void OnAttackEnd()
    {
        
    }
    public void SetPos(Vector3 pos)
    {
        this.START_METHOD("SetPos");
        data.pos = pos;
        bNeedChange = true;
        this.END_METHOD("SetPos");
    }
    public void SetLevel(int level)
    {
        this.START_METHOD("SetLevel");
        data.level = level;
        this.END_METHOD("SetLevel");
    }
    public void SetColor(Color color)
    {
        model.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.SetColor("_Color", color);
    }
    public void SetCamp(int camp)
    {
        this.START_METHOD("SetCamp");
        data.camp = camp;
        this.END_METHOD("SetCamp");
    }
    public void SetDir(Vector3 rotate)
    {
        this.START_METHOD("SetDir");
        data.rotation = rotate;
        bNeedChange = true;
        this.END_METHOD("SetDir");
    }
    public virtual void SetLayer(int order)
    {
        this.START_METHOD("SetLayer");
        layerOrder = order + LAYER_BASE;
        model.transform.GetChild(0).GetComponent<Renderer>().sortingOrder = layerOrder;//model.transform.GetChild(0).renderer.sortingOrder = layerOrder;
        this.END_METHOD("SetLayer");
    }
    public int CompareTo(Chara other)
    {
        return 0;
    }

    public virtual void Update()
    {
        if(bNeedChange)
        {
            model.transform.localPosition = data.pos;
            model.transform.localRotation = Quaternion.Euler(data.rotation);
            bNeedChange = false;
        }
    }


}
