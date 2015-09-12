using UnityEngine;
using System.Collections;

public class CharaStatus : MonoBehaviour
{
    [HideInInspector]
    public enum Dir
    {
        LeftUp,
        Left,
        LeftDown,
        RightUp,
        Right,
        RightDown,
    }
    [HideInInspector]
    public enum Pose
    {
        None = 1,
        Idle,
        Run,
        Attack,
        Summon,
        Die,
    }
    public GameObject bullet;
    [HideInInspector]
    public bool rotateWeapon = false;//是否旋转加农炮
    [HideInInspector]
    public Pose CurPose;
    Dir _CurDir = Dir.RightUp;
    Dir mDir;
    Pose mPose;
    [HideInInspector]
    public Chara Parent;
    private float idleRotateInterval;
    private float idleRotateSpeed;
    private float idleRotateAngle;
    private int idleInvInterval;

    // Use this for initialization
    void Start()
    {
        mDir = _CurDir;
        mPose = CurPose;
        if (Parent != null)
            Parent.Start();
    }
    bool needChangeStatus = false;

    void ChangeAnim()
    {
        if (needChangeStatus == false)
            return;
        bool needMirror = false;
        int mfps = 10;
        string namePrefix = "";
        bool reverse = false;
        if (rotateWeapon)
        {
            CancelInvoke("IdleChange");
        }
        if (mPose == Pose.Run)
        {
            namePrefix = "walk";
            if (mDir == Dir.RightDown)
            {
                namePrefix += "down";
            }
            else if (mDir == Dir.Right)
            {
                namePrefix += "right";
            }
            else if (mDir == Dir.RightUp)
            {
                namePrefix += "up";
            }
            else if (mDir == Dir.LeftDown)
            {
                namePrefix += "down";
                needMirror = true;
            }
            else if (mDir == Dir.Left)
            {
                namePrefix += "right";
                needMirror = true;
            }
            else if (mDir == Dir.LeftUp)
            {
                namePrefix += "up";
                needMirror = true;
            }
        }
        else if (mPose == Pose.Idle)
        {
            if (rotateWeapon)
            {
                InvokeRepeating("IdleChange",0f,1.0f);
                mfps = 1;
            }
            else
            {
                namePrefix = "stand";
                if (mDir == Dir.RightDown)
                {
                    namePrefix += "down";
                }
                else if (mDir == Dir.Right)
                {
                    namePrefix += "right";
                }
                else if (mDir == Dir.RightUp)
                {
                    namePrefix += "up";
                }
                else if (mDir == Dir.LeftDown)
                {
                    namePrefix += "down";
                    needMirror = true;
                }
                else if (mDir == Dir.Left)
                {
                    namePrefix += "right";
                    needMirror = true;
                }
                else if (mDir == Dir.LeftUp)
                {
                    namePrefix += "up";
                    needMirror = true;
                }
                mfps = 5;
            }
        }
        else if (mPose == Pose.Attack)
        {
            if (rotateWeapon)
            {
                //calc in ai
            }
            else
            {
                namePrefix = "attack";
                if (mDir == Dir.RightDown)
                {
                    namePrefix += "down";
                }
                else if (mDir == Dir.Right)
                {
                    namePrefix += "right";
                }
                else if (mDir == Dir.RightUp)
                {
                    namePrefix += "up";
                }
                else if (mDir == Dir.LeftDown)
                {
                    namePrefix += "down";
                    needMirror = true;
                }
                else if (mDir == Dir.Left)
                {
                    namePrefix += "right";
                    needMirror = true;
                }
                else if (mDir == Dir.LeftUp)
                {
                    namePrefix += "up";
                    needMirror = true;
                }
                mfps = 6;
            }
        }
        else if (mPose == Pose.Summon)
        {
            namePrefix = "victory";
            if (mDir == Dir.RightDown)
            {
                namePrefix += "down";
            }
            else if (mDir == Dir.Right)
            {
                namePrefix += "right";
            }
            else if (mDir == Dir.RightUp)
            {
                namePrefix += "up";
            }
            else if (mDir == Dir.LeftDown)
            {
                namePrefix += "down";
                needMirror = true;
            }
            else if (mDir == Dir.Left)
            {
                namePrefix += "right";
                needMirror = true;
            }
            else if (mDir == Dir.LeftUp)
            {
                namePrefix += "up";
                needMirror = true;
            }
            mfps = 6;
        }
        else if (mPose == Pose.Die)
        {
            namePrefix = "";
            QuadTextureAni ani1 = GetComponentInChildren<QuadTextureAni>();
            if (ani1 != null)
                ani1.mActive = false;
            transform.GetChild(0).localPosition = Vector3.zero;
            QuadTexture4Ngui tex = transform.GetChild(0).GetComponent<QuadTexture4Ngui>();
            tex.mSpriteName = "baseruins";
            tex.InitFace();
            transform.GetChild(0).gameObject.GetComponent<Renderer>().sortingOrder = -1;
            if (transform.childCount > 2)
                transform.GetChild(2).gameObject.SetActive(false);
        }
        QuadTextureAni ani = GetComponentInChildren<QuadTextureAni>();
        if (ani != null && string.IsNullOrEmpty(namePrefix))
        {
            ani.namePrefix = namePrefix;
            ani.mFPS = mfps;
            ani.RebuildSpriteList();
            ani.mirror = needMirror;
            if (mPose == Pose.Attack)
                ani.OnNormalAniFinished += OnAttankEnd;
        }
        needChangeStatus = false;
    }

    void OnAttankEnd()
    {
        if (Parent != null)
            Parent.OnAttackEnd();
    }
    // Update is called once per frame
    void Update()
    {
        if (Parent != null)
        {
            Parent.Update();
        }
        //check direction
        CheckDir();
        //change anim
        ChangeAnim();
        //change idle
        ChangeIdle();
    }

    void IdleChange()
    {
        
        idleInvInterval = Random.Range(-10, 10);
        idleRotateInterval = Random.Range(5.0f, 10.0f);
    }

    private static float lastTime = Time.realtimeSinceStartup;
    void ChangeIdle()
    {
        if(!rotateWeapon)
            return;
        if(CurPose!=Pose.Die)
            return;
        if (Time.realtimeSinceStartup > lastTime + idleRotateInterval)
        {
            float curDiv = 0.0f;
            idleRotateAngle = Random.Range(10, 50);
            if (idleInvInterval>0)
            {
                curDiv = idleRotateAngle;
            }
            else
            {
                curDiv = -1*idleRotateAngle;
            }
            gameObject.transform.Rotate(new Vector3(0,curDiv,0));
            lastTime = Time.realtimeSinceStartup;
        }
    }

    void CheckDir()
    {
        if (gameObject == null)
        {
            return;
        }
        needChangeStatus = false;
        if (mDir != _CurDir)
        {
            needChangeStatus = true;
            mDir = _CurDir;
        }
        if (mPose != CurPose)
        {
            needChangeStatus = true;
            mPose = CurPose;
        }
        Quaternion dir = gameObject.transform.localRotation;
        float curAngle = dir.eulerAngles.y % 360.0f;
        curAngle = curAngle < 0.0f ? curAngle + 360.0f : curAngle;
        if (rotateWeapon && CurPose != Pose.Die)
        {
            QuadTexture4Ngui tex = transform.GetChild(0).GetComponent<QuadTexture4Ngui>();
            if ((curAngle >= 0.0f && curAngle <= 151f) || (curAngle > 321.0f))
            {
                int angle = ((int)(curAngle / 10.0f) * 10);
                tex.mSpriteName = "" + angle;
                tex.mirrorX = false;
                tex.InitFace();
            }
            else if (curAngle > 151.0f && curAngle <= 301.0f)
            {
                int angle = ((int)((301.0f - curAngle) / 10.0f) * 10);
                tex.mSpriteName = "" + angle;
                tex.mirrorX = true;
                tex.InitFace();
            }
            else if (curAngle > 301.0f && curAngle <= 321.0f)
            {
                int angle = ((int)((661.0f - curAngle) / 10.0f) * 10);
                tex.mSpriteName = "" + angle;
                tex.mirrorX = true;
                tex.InitFace();
            }
        }
        else
        {
            if ((curAngle >= 0.0f && curAngle < 45.0f) || (curAngle > 315.0f && curAngle <= 360.0f))
            {
                _CurDir = Dir.RightUp;
            }
            else if (curAngle >= 270.0f && curAngle <= 315.0f)
            {
                _CurDir = Dir.LeftUp;
            }
            else if (curAngle >= 45.0f && curAngle < 90.0f)
            {
                _CurDir = Dir.Right;
            }
            else if (curAngle >= 180.0f && curAngle < 270.0f)
            {
                _CurDir = Dir.Left;
            }
            else if (curAngle >= 127.0f && curAngle < 180.0f)
            {
                _CurDir = Dir.LeftDown;
            }
            else if (curAngle >= 90.0f && curAngle < 127.0f)
            {
                _CurDir = Dir.RightDown;
            }
        }
    }
}
