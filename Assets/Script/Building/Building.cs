using UnityEngine;
using System.Collections;

public class Building : Chara {

    public Transform GetTransform()
    {
        return model.transform;
    }
    public CharaData.buildingModel buildingType;
    bool beGuided = false;//guid missile
    bool beHited = true;//be attacked

    public override void Start()
    {
        data.classtype = (int)CharaData.CharClassType.BUILDING;
        base.Start();
        //blood
    }
    float tempHideTime = Time.realtimeSinceStartup;
    public override void Update()
    {
        base.Update();
    }
}
