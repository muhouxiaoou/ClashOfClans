using UnityEngine;
using System.Collections;

//用于测试游戏，找出卡在哪里
public class GlobalManager : UnityAllSceneSingleton<GlobalManager> {

    public float f_UpdateInterval = 0.5f;//刷新频率
    private float f_LastInterval;//上一帧的刷新帧
    private int i_Frames = 0;//帧数
    private float f_FPS;//游戏平均帧数 Frame Per Second每秒帧数
    public static int verts;//顶点数
    public static int tris;//面数


    public override void Awake()
    {
        base.Awake();
        Application.targetFrameRate = 45;//锁帧，防止游戏在不同手机上运行出现时快时慢,电脑上显示60FPS相对于手机30FPS
    }

    void Start()
    {
        //load all managers
        GameManager.Instance.CurStatus = GameManager.Status.LOAD_RESOURCE;
        InputListener.Init();
        DataManager.Init();

        f_LastInterval = Time.realtimeSinceStartup;//从游戏开始到现在，游戏经过的时间
        i_Frames = 0;
    }

    /// <summary>
    /// 获取游戏的所有状态，顶点数，面数
    /// </summary>
    void GetObjectStats()
    {
        verts = 0;
        tris = 0;
        GameObject[] ob = FindObjectsOfType(typeof(GameObject)) as GameObject[];//寻找游戏的所有对象
        foreach (GameObject obj in ob)
        {
            GetObjectStats(obj);
        }
    }

    /// <summary>
    /// 获取单个对象状态
    /// </summary>
    /// <param name="obj"></param>
    void GetObjectStats(GameObject obj)
    {
        Component[] filters;//对象过滤器，因为不是所有游戏对象都要，有些是Unity原生对象，比如辅助线
        filters = obj.GetComponentsInChildren<MeshFilter>();//MeshFilter面片的意思

        //将所有顶点统计出来
        foreach (MeshFilter f in filters)//Unity通过MeshFilter（过滤器），或者MeshRender（渲染器）画出游戏物体
        {
            tris += f.sharedMesh.triangles.Length / 3;//一个三角面有三个顶点，这样表示只有一个面
            verts += f.sharedMesh.vertexCount;
        }
    }

    void OnGUI()
    {
        GUI.skin.label.normal.textColor = new Color(255, 255, 255, 1.0f);//白色
        GUI.Label(new Rect(0, 10, 200, 200), "FPS:" + (f_FPS).ToString("f2"));//f2取两位小数
        string vertsdisplay = verts.ToString("#,##0 verts");
        GUILayout.Label(vertsdisplay);
        string trisdisplay = tris.ToString("#,##0 tris");
        GUILayout.Label(trisdisplay);
    }

    void Update()
    {
        ++i_Frames;
        if (Time.realtimeSinceStartup > f_LastInterval + f_UpdateInterval)//统计帧数
        {
            f_FPS = i_Frames / (Time.realtimeSinceStartup - f_LastInterval);
            i_Frames = 0;//统计后还原

            f_LastInterval = Time.realtimeSinceStartup;
            GetObjectStats();
        }
    }
}
