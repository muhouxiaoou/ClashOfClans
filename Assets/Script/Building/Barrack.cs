using UnityEngine;
using System.Collections;

public class Barrack : Building {
    public Barrack()
    {
        this.START_METHOD("Barrack");
        model = (GameObject)GameObject.Instantiate(Resources.Load("barrack"));
        model.name = "" + ID;


        Transform house = model.transform.GetChild(0);
        house.gameObject.GetComponent<Renderer>().sortingOrder=layerOrder=LAYER_BASE+1;
        Transform grass = model.transform.GetChild(1);
        grass.gameObject.GetComponent<Renderer>().sortingOrder = 0;
        status = model.GetComponent<CharaStatus>();
        //POSE:idle die attack
        status.Parent = this;
        buildingType = CharaData.buildingModel.BARRACK;
        //blood

        this.END_METHOD("Barrack");
    }

    public override void Start()
    {
        base.Start();
        BuildingConf conf = BuildingConfManager.Instance.GetConfByID(10102);
        if(conf!=null)
        {
            data.life = conf.life;
            data.maxlife = conf.life;
        }
    }

    public override void Update()
    {
        base.Update();
    }
}
