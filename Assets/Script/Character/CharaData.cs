using UnityEngine;
using System.Collections;

public struct CharaData
{
    /// <summary>
    /// 角色枚举，人物、建筑
    /// </summary>
    public enum CharClassType
    {
        CHARACTER=1,
        BUILDING,
    }
    /// <summary>
    /// 角色类型枚举，士兵、弓箭手、、
    /// </summary>
    public enum charMode
    {
        NONE=-1,
        SOLDIER,
        BOWMAN,
        GIANT,
        BOWMAN1,
        BOWMAN2,
        BOWMAN3,
        BOWMAN4,
        VIKING,
    }
    /// <summary>
    /// 可摧毁建筑，不可摧毁建筑
    /// </summary>
    public enum buildingModel
    {
        BARRACK=1,
        CANNON,
    }
    public void Reset()
    {
        isDirty = true;
    }
    /// <summary>
    /// 是不是脏数据
    /// </summary>
    public bool isDirty;

    public long modelId;
    public int classtype;
    public int modeltype;//1.solder 2.bowman
    public int level;
    public Vector3 pos;
    public float speed;
    public int camp;
    public Vector3 rotation;
    //public CharaStatus.Pose pose;
    public float life;
    public float maxlife;
    public int attackPower;
    public float attackRange;
    public float searchInterval;
    public float attackInterval;
    public int currentUseSkillId;
}
