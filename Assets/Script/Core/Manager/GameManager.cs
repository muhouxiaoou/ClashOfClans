using UnityEngine;
using System.Collections;

/// <summary>
/// 主管整个游戏的状态转换以及消失传递
/// 相当于MVC的C
/// </summary>
public class GameManager : UnityAllSceneSingleton<GameManager>,IMessageObject {
    public enum Status
    {
        NONE=1,
        LOAD_RESOURCE,
        LOAD_SCENE,
        PREPARE_SCAN,//预检测
        START_GAME,
        END_GAME,
    }
    public CharaManager cManager;
    public Status CurStatus = Status.NONE;
    public override void Awake()
    {
        base.Awake();
        cManager = new CharaManager();
    }
    void Start()
    {

    }

    public void ReloadScene(int scene)
    {
        this.START_METHOD("ReloadScene");
        //造兵营
        Vector3 obstacle3Pos = new Vector3(9.76687f, 1f, 9.307766f);
        Barrack barrack = (Barrack)cManager.SpawnChar(CharaData.CharClassType.BUILDING, (int)CharaData.buildingModel.BARRACK, 1, 1, obstacle3Pos, new Vector3(0, 0, 0), CharaStatus.Pose.Idle);

        Vector3 obstacle4Pos = new Vector3(13.17646f, 1f, 7.730552f);
        barrack = (Barrack)cManager.SpawnChar(CharaData.CharClassType.BUILDING, (int)CharaData.buildingModel.BARRACK, 1, 1, obstacle4Pos, new Vector3(0, 0, 0), CharaStatus.Pose.Idle);

        Vector3 obstacle5Pos = new Vector3(10.62819f, 1f, 21.06617f);
        barrack = (Barrack)cManager.SpawnChar(CharaData.CharClassType.BUILDING, (int)CharaData.buildingModel.BARRACK, 1, 1, obstacle5Pos, new Vector3(0, 0, 0), CharaStatus.Pose.Idle);

        Vector3 obstacle6Pos = new Vector3(30.74964f, 1f, 18.58432f);
        barrack = (Barrack)cManager.SpawnChar(CharaData.CharClassType.BUILDING, (int)CharaData.buildingModel.BARRACK, 1, 1, obstacle6Pos, new Vector3(0, 0, 0), CharaStatus.Pose.Idle);

        //do delay 对地形扫描
        this.END_METHOD("ReloadScene");
    }
    void Update()
    {
        switch(CurStatus)
        {
            case Status.LOAD_RESOURCE:
                break;
            case Status.LOAD_SCENE:
                ReloadScene(1);
                CurStatus = Status.PREPARE_SCAN;
                break;
            case Status.PREPARE_SCAN:
                CurStatus = Status.START_GAME;//a* scan time
                break;
            case Status.START_GAME:
                break;
            case Status.END_GAME:
                break;
        }
    }
}
