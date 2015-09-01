using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 用于创建角色的管理类
/// 相当于建造者底层,工厂模式
/// </summary>
public class CharaManager : IMessageObject {

    List<Chara> chars = new List<Chara>();
    List<Chara> building = new List<Chara>();
    List<Chara> allChara = new List<Chara>();
    List<Vector3> positions = new List<Vector3>();

    public Chara SpawnChar(CharaData.CharClassType classType,int charModelType,int camp,int level,Vector3 pos,Vector3 dir,CharaStatus.Pose pose)
    {
        this.START_METHOD("SpawnChar");
        Chara tempChar = null;
        if(classType==CharaData.CharClassType.CHARACTER)
        {

        }
        else if(classType==CharaData.CharClassType.BUILDING)
        {
            if((CharaData.buildingModel)charModelType==CharaData.buildingModel.BARRACK)
            {
                Barrack chara = new Barrack();//TODO: need change class to resource pool
                chara.SetPos(pos);
                chara.SetDir(dir);
                chara.SetCamp(camp);
                tempChar = chara;
            }
            if (tempChar != null)
                building.Add(tempChar);
            else
                throw new UnityException("no current building type to spawn!!");
        }

        allChara.Add(tempChar);
        this.END_METHOD("SpawnChar");
        return tempChar;
    }

    public void DestroyAll()
    {
        this.START_METHOD("DestroyAll");
        allChara.Clear();
        for (int i = chars.Count - 1; i >= 0; i--)
        {
            chars[i].Destroy();
            chars.RemoveAt(i);
        }
        for (int i = building.Count-1; i >=0; i--)
        {
            building[i].Destroy();
            building.RemoveAt(i);
        }
        Resources.UnloadUnusedAssets();
        System.GC.Collect();
        this.END_METHOD("DestroyAll");
    }

    public void DestroyChar(long id)
    {
        this.START_METHOD("DestroyChar");
        for (int i = chars.Count - 1; i >= 0; i--)
        {
            if(chars[i].ID==id)
            {
                chars[i].Destroy();
                chars.RemoveAt(i);
                break;
            }
        }
        //所有RemoveAt最好倒过来循环，防止报空指针错误，链表特性？ID自动计数，如果中间缺了一块，会累加出错
        for (int i = allChara.Count - 1; i >= 0; i--)
        {
            if (allChara[i].ID == id)
            {
                allChara[i].Destroy();
                allChara.RemoveAt(i);
                break;
            }
        }
        this.END_METHOD("DestroyChar");
    }

    public void DestroyBuilding(long id)
    {
        this.START_METHOD("DestroyBuilding");
        for (int i = building.Count - 1; i >= 0; i--)
        {
            if (building[i].ID == id)
            {
                building[i].Destroy();
                chars.RemoveAt(i);
                break;
            }
        }
        //所有RemoveAt最好倒过来循环，防止报空指针错误，链表特性？ID自动计数，如果中间缺了一块，会累加出错
        for (int i = allChara.Count - 1; i >= 0; i--)
        {
            if (allChara[i].ID == id)
            {
                allChara[i].Destroy();
                allChara.RemoveAt(i);
                break;
            }
        }
        this.END_METHOD("DestroyBuilding");
    }

    public Building GetBuildingByID(long id)
    {
        for (int i = 0; i < building.Count; i++)
        {
            if (building[i].ID == id)
            {
                return building[i] as Building;
            }
        }
        return null;
    }


}
