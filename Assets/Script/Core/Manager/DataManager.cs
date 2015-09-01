using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LumenWorks.Framework.IO.Csv;//一个读取Csv的小插件LumenWorks.Framework.IO.dll %% LumenWorks.Framework.IO.XML

public class DataManager : UnityAllSceneSingleton<DataManager>, IMessageObject
{

    public static readonly string PathURL =
#if UNITY_ANDROID && !UNITY_EDITOR
        "jar:file://" + Application.dataPath + "!/asset/";
#elif UNITY_IPHONE
        Application.dataPath + "/Raw/";
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
 "file://" + Application.dataPath + "/StreamingAssets" + "/";
#endif

    public bool HasDoneResource = false;

    private static DataManager ins;
    public static void Init()
    {
        ins = DataManager.Instance;
    }

    void Start()
    {
        StartCoroutine(LoadMainGameObject(PathURL));
    }

    /// <summary>
    /// 读取csv中的数据，Buildings，hero，skills
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private IEnumerator LoadMainGameObject(string path)
    {
        this.PRINT("path" + path);
        string tempPath = path + "csv.assetbundle";
        WWW bundle = new WWW(tempPath);

        yield return bundle;
        Object[] objs = bundle.assetBundle.LoadAllAssets(typeof(soCsv));

        this.PRINT("counts:" + objs.Length);
        for (int i = 0, max = objs.Length; i < max; i++)
        {
            this.PRINT("name:" + objs[i].name);
            Object obj = objs[i];
            soCsv csv = obj as soCsv;
            if (csv.name != "buildings" && csv.name != "hero" && csv.name != "skills")
            {
                continue;
            }
            MemoryStream ms = new MemoryStream(csv.content);
            if (ms == null)
            {
                this.PRINT("convert csv failed!!");
                continue;
            }
            StreamReader sr = new StreamReader(ms, Encoding.GetEncoding("GB2312"), true);
            TextReader tr = sr as TextReader;
            if (tr == null)
            {
                this.PRINT("text reader is null");
                continue;
            }
            CsvReader cr = new CsvReader(tr, true);
            if (cr == null)
            {
                this.PRINT("csvreader is null");
                continue;
            }

            //read cv from tables one by one
            if (csv.name == "buildings")
            {
                ReadBuilding(cr);
            }
            else if (csv.name=="hero")
            {
                ReadHero(cr);
            }
            else if (csv.name=="skills")
            {
                ReadSkill(cr);
            }
        }

        bundle.assetBundle.Unload(false);

        tempPath = path + "txt.assetbundle";
        bundle = new WWW(tempPath);
        yield return bundle;
        TextAsset asset = bundle.assetBundle.LoadAsset("string", typeof(TextAsset)) as TextAsset;
        StringReader reader = new StringReader(asset.text);
        string line = reader.ReadLine();
        while (line!=null)
        {
            //RemoveEmptyEntries移除空部分
            string[] str = line.Split(new char[] { '=' }, System.StringSplitOptions.RemoveEmptyEntries);
            StringConfManager.Instance.AddStringConf(str[0], str[1]);
            this.PRINT(str[0] + "=" + str[1]);
            line = reader.ReadLine();
        }
        bundle.assetBundle.Unload(false);//false不全部卸载资源
        //set curstatus loadscene
        GameManager.Instance.CurStatus = GameManager.Status.LOAD_SCENE;
        this.PRINT("===================== data loaded ====================");
    }

    /// <summary>
    /// 读取建筑CSV配置
    /// </summary>
    /// <param name="cr"></param>
    private void ReadBuilding(CsvReader cr)
    {
        int fieldCount = cr.FieldCount;
        string[] headers = cr.GetFieldHeaders();
        while (cr.ReadNextRecord())
        {
            int i = 0;
            BuildingConf conf = new BuildingConf();
            conf.id = int.Parse(cr[i++]);
            conf.name = cr[i++];
            conf.type = int.Parse(cr[i++]);
            string[] buildRange = cr[i++].Split(';');
            conf.buildRange[0] = int.Parse(buildRange[0]);
            conf.buildRange[1] = int.Parse(buildRange[1]);
            conf.life = int.Parse(cr[i++]);
            conf.hitRate = int.Parse(empty2number(cr[i++]));
            conf.attackSpeed = float.Parse(empty2number(cr[i++]));
            string[] attack = cr[i++].Split(new char[] { ';' }, System.StringSplitOptions.RemoveEmptyEntries);
            if (attack.Length != 0)
            {
                conf.attack[0] = int.Parse(attack[0]);
                conf.attack[1] = int.Parse(attack[1]);
            }
            string[] attackRange = cr[i++].Split(new char[] { ';' }, System.StringSplitOptions.RemoveEmptyEntries);
            if (attackRange.Length != 0)
            {
                conf.attackRange[0] = float.Parse(attackRange[0]);
                conf.attackRange[1] = float.Parse(attackRange[1]);
            }
            conf.attackMode = int.Parse(empty2number(cr[i++]));
            conf.damageRange = float.Parse(empty2number(cr[i++]));
            conf.cooldownHit = int.Parse(empty2number(cr[i++]));
            conf.cooldownTime = float.Parse(empty2number(cr[i++]));
            conf.buffID = int.Parse(empty2number(cr[i++]));
            conf.level = int.Parse(empty2number(cr[i++]));
            conf.desc = cr[i++];
            conf.atlas = cr[i++];
            BuildingConfManager.Instance.AddConf(conf);
        }
    }

    /// <summary>
    /// 读取英雄配置
    /// </summary>
    /// <param name="cr"></param>
    private void ReadHero(CsvReader cr)
    {
        int fieldCount = cr.FieldCount;
        string[] headers = cr.GetFieldHeaders();
        while (cr.ReadNextRecord())
        {
            int i = 0;
            HeroConf conf = new HeroConf();
            conf.id = int.Parse(cr[i++]);
            conf.name = cr[i++];
            conf.type = int.Parse(cr[i++]);
            conf.level = int.Parse(cr[i++]);
            conf.exportname = cr[i++];
            conf.exportnameNpc = cr[i++];
            conf.moveSpeed = float.Parse(cr[i++]);
            conf.hitPoint = int.Parse(cr[i++]);
            conf.attackRange = float.Parse(cr[i++]);
            conf.attackSpeed = float.Parse(cr[i++]);
            conf.attackPower = int.Parse(cr[i++]);
            conf.attackRadius = float.Parse(cr[i++]);
            conf.activeSkill = int.Parse(cr[i++]);
            HeroConfManager.Instance.AddConf(conf);
        }
    }
    private void ReadSkill(CsvReader cr)
    {
        int fieldCount = cr.FieldCount;
        string[] headers = cr.GetFieldHeaders();
        while (cr.ReadNextRecord())
        {
            int i = 0;
            SkillConf conf = new SkillConf();
            conf.id = int.Parse(cr[i++]);
            conf.name = cr[i++];
            conf.skillType = int.Parse(cr[i++]);
            conf.kind = int.Parse(cr[i++]);
            conf.isManual = int.Parse(cr[i++]);
            conf.skillLimit = int.Parse(cr[i++]);
            conf.skillTime = int.Parse(cr[i++]);
            conf.summonID = int.Parse(empty2number(cr[i++]));
            conf.summonNum = int.Parse(empty2number(cr[i++]));
            conf.skillRange = int.Parse(empty2number(cr[i++]));
            SkillConfManager.Instance.AddConf(conf);
        }
    }

    /// <summary>
    /// 检测csv表格空值，并自动赋值0
    /// </summary>
    /// <param name="val"></param>
    /// <returns></returns>
    private string empty2number(string val)
    {
        return (val == "" || val == null) ? "0" : val;
    }
}

public class StringConfManager
{
    private static StringConfManager _instance;
    public Dictionary<string, string> stringConfs = new Dictionary<string, string>();
    public static StringConfManager Instance
    {
        get 
        {
            if (_instance == null)
                _instance = new StringConfManager();
            return _instance; 
        }
    }
    public void AddStringConf(string name,string txt)
    {
        stringConfs[name] = txt;
    }
    public string GetStringByName(string name)
    {
        if(stringConfs.ContainsKey(name))
        {
            return stringConfs[name];
        }
        return null;
    }
}

#region 建筑类
/// <summary>
/// 建筑属性
/// </summary>
public class BuildingConf
{
    /// <summary>建筑ID</summary>
    public int id;
    /// <summary>建筑名字</summary>
    public string name;
    /// <summary>建筑类型</summary>
    public int type;
    /// <summary>建筑占地面积</summary>
    public int[] buildRange = new int[2];
    /// <summary>建筑生命条</summary>
    public int life;
    /// <summary>命中频率</summary>
    public int hitRate;
    /// <summary>攻击频率</summary>
    public float attackSpeed;
    /// <summary>攻击力</summary>
    public int[] attack = new int[2];
    /// <summary>攻击范围</summary>
    public float[] attackRange = new float[2];
    /// <summary>攻击方式</summary>
    public int attackMode;
    /// <summary>伤害范围</summary>
    public float damageRange;
    /// <summary>冷却攻击</summary>
    public int cooldownHit;
    /// <summary>冷却时间</summary>
    public float cooldownTime;
    /// <summary>技能ID</summary>
    public int buffID;
    /// <summary>等级</summary>
    public int level;
    /// <summary>desc</summary>
    public string desc;
    /// <summary>模型名字</summary>
    public string atlas;
}
public class BuildingConfManager : ConfManager<BuildingConfManager, BuildingConf>
{
    public override void AddConf(BuildingConf conf)
    {
        confs.Add(conf.id, conf);
    }
}
#endregion

#region 技能类
public class SkillConf
{
    public int id;
    public string name;
    public int skillType;//1.command,2.hero,3.positive
    public int kind;//10.call
    /// <summary>是否为手动触发技能</summary>
    public int isManual;
    public int skillLimit;
    public int skillTime;//during time
    public int summonID;
    public int summonNum;
    public int skillRange;
}
public class SkillConfManager : ConfManager<SkillConfManager, SkillConf>
{
    public override void AddConf(SkillConf conf)
    {
        confs.Add(conf.id, conf);
    }
}
#endregion

#region 英雄类
public class HeroConf
{
    public int id;
    public string name;
    public int type;
    public int level;
    public string exportname;
    public string exportnameNpc;
    public float moveSpeed;
    public int hitPoint;
    public float attackRange;
    public float attackSpeed;
    public int attackPower;
    public float attackRadius;
    public int activeSkill;
}
public class HeroConfManager : ConfManager<HeroConfManager, HeroConf>
{
    public HeroConf GetHeroConfByType(int type)//CharaData.charModel type)
    {
        foreach (int id in confs.Keys)
        {
            if (confs[id].type == (int)type)
            {
                return confs[id];
            }
        }
        return null;
    }
    public override void AddConf(HeroConf conf)
    {
        confs.Add(conf.id, conf);
    }
}
#endregion

public abstract class ConfManager<T, U> where T : new()
{
    private static T _instance;
    protected Dictionary<int, U> confs = new Dictionary<int, U>();
    public static T Instance
    {
        get
        {
            if (_instance == null)
                _instance = new T();
            return _instance;
        }
    }
    public abstract void AddConf(U conf);
    public U GetConfByID(int id)
    {
        if (confs.ContainsKey(id))
        {
            return confs[id];
        }
        return default(U);
    }
}