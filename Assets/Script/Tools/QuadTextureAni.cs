using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(QuadTexture4Ngui))]
public class QuadTextureAni : MonoBehaviour
{
    protected List<string> mSpriteNames = new List<string>();
    public int frames { get { return mSpriteNames.Count; } }
    public float mFPS = 30;

    /// <summary>名字前缀 用于命名如XXX_1 XXX_2 类型播放下去</summary>
    public string namePrefix;
    /// <summary>pingpong循环播放 </summary>
    public bool flip;
    /// <summary>是否反向播放 </summary>
    public bool reverse;
    /// <summary>是否循环播放 </summary>
    public bool loop;

    [HideInInspector]
    [SerializeField]
    public bool mirror = false;//做一半，然后镜像，这样可节省内存
    protected QuadTexture4Ngui mSptite;//当前播放的是哪一张
    protected float mDelta = 0f;//时间差
    protected int mIndex = 0;//记录一下当前播放的序号
    public bool mActive = true;//记录一下是否被激活
    public bool isPlaying { get { return mActive; } }
    public delegate void CallBack();
    public event CallBack OnNormalAniFinished;

    protected virtual void Start()
    {
        RebuildSpriteList(true);
        if (reverse)
        {
            mIndex = mSpriteNames.Count - 1;
        }
    }
    static int SortByName(string n1, string n2)
    {
        if (n1.IndexOf('_') == -1)
            return -1;
        else if (n2.IndexOf('_') == -1)
            return 1;
        else if (int.Parse(n1.Substring(n1.LastIndexOf('_') + 1)) < int.Parse(n2.Substring(n2.LastIndexOf('_') + 1)))
            return -1;
        else if (int.Parse(n1.Substring(n1.LastIndexOf('_') + 1)) == int.Parse(n2.Substring(n2.LastIndexOf('_') + 1)))
            return 0;
        else
            return 1;
    }
    public void RebuildSpriteList(bool first = false)
    {
        if (mSptite == null)
            mSptite = GetComponent<QuadTexture4Ngui>();
        mSpriteNames.Clear();
        if (first == false)
            OnNormalAniFinished = null;
        if (mSptite != null && mSptite.Atlas != null)
        {
            List<UISpriteData> sprites = mSptite.Atlas.spriteList;//从mSptite.Atlas取出所有精灵对象
            for (int i = 0, imax = sprites.Count; i < imax; ++i)
            {
                UISpriteData sprite = sprites[i];
                if (string.IsNullOrEmpty(namePrefix) || sprite.name.StartsWith(namePrefix))
                {
                    mSpriteNames.Add(sprite.name);
                }
            }
            if (mSpriteNames.Count != 0 && mSpriteNames[0].Contains("_"))
                mSpriteNames.Sort(SortByName);
        }
    }
    public void Reset()
    {
        mActive = true;
        mIndex = 0;
        if (mSptite != null && mSpriteNames.Count > 0)
            mSptite.spriteName = mSpriteNames[mIndex];//form the very first
    }
    private bool needReverse = false;//是否需要反向播放
    void Update()
    {
        if (mActive && mSpriteNames.Count > 1 && Application.isPlaying && mFPS > 0f)
        {
            mDelta += RealTime.deltaTime;
            float rate = 1.0f / mFPS;
            if (rate < mDelta)
            {
                mDelta = (rate > 0f) ? mDelta - rate : 0f;
                if (flip)
                {
                    if (needReverse)
                    {
                        mIndex--;
                        mActive = loop;
                    }
                    else
                    {
                        mIndex++;
                        mActive = loop;
                    }
                    if (mIndex + 1 >= mSpriteNames.Count)
                    {
                        needReverse = true;
                    }
                    else if (mIndex - 1 < 0)
                    {
                        needReverse = false;
                    }
                }
                else if (reverse)
                {
                    if (--mIndex <= 0)
                    {
                        mIndex = mSpriteNames.Count - 1;
                        mActive = loop;
                    }
                }
                else
                {
                    if (++mIndex >= mSpriteNames.Count)
                    {
                        if (OnNormalAniFinished != null)
                            OnNormalAniFinished();
                        mIndex = 0;
                        mActive = loop;
                    }
                }

                if (mActive)
                {
                    mSptite.spriteName = mSpriteNames[mIndex];
                    mSptite.mirrorX = mirror;
                    mSptite.InitFace();
                }
            }
        }
    }
}
