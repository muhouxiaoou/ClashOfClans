using UnityEngine;
using System.Collections;
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Billboard))]
//CHECK IS BILLBOARD EXIST
public class QuadTexture4Ngui : MonoBehaviour,IMessageObject
{
    public UIAtlas Atlas;
    public string mSpriteName;
    public float ScaleFactor = 1;
    public bool mirrorX = false;
    public bool mirrorY = false;
    public bool mirrorXY = false;
    [System.NonSerialized]
    protected UISpriteData mSprite;
    bool mSpriteSet = false;//确认图片是否被设置过
    private Mesh mesh;

    void Start()
    {
        if (ScaleFactor == 0)
        {
            ScaleFactor = 1;
        }
        InitAtlas();
        InitFace();
    }
    void InitAtlas()
    {
        if (null != Atlas)
        {
            mSpriteSet = false;
            mSprite = null;

            if (string.IsNullOrEmpty(mSpriteName))
            {
                if (Atlas != null && Atlas.spriteList.Count > 0)
                {
                    SetAtlasSprite(Atlas.spriteList[6]);
                    mSpriteName = mSprite.name;
                }
            }
            if (!string.IsNullOrEmpty(mSpriteName))
            {
                string sprite = mSpriteName;
                mSpriteName = "";
                spriteName = sprite;
            }
        }
    }
    public string spriteName
    {
        get
        {
            return mSpriteName;
        }
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                if (string.IsNullOrEmpty(mSpriteName)) return;

                mSpriteName = "";

                mSprite = null;

                mSpriteSet = false;
            }
            else if (mSpriteName != value)
            {
                mSpriteName = value;
                mSprite = null;
                mSpriteSet = false;
            }
        }
    }

    protected void SetAtlasSprite(UISpriteData sp)
    {
        mSpriteSet = true;
        if (sp != null)
        {
            mSprite = sp;
            mSpriteName = mSprite.name;
        }
        else
        {
            mSpriteName = (mSprite != null) ? mSprite.name : "";
            mSprite = sp;
        }
    }

    public UISpriteData GetAtlasSprite()
    {
        if (!mSpriteSet)
            mSprite = null;
        if (mSprite == null && Atlas != null)
        {
            if(!string.IsNullOrEmpty(mSpriteName))
            {
                UISpriteData sp = Atlas.GetSprite(mSpriteName);
                if (sp == null) return null;
                SetAtlasSprite(sp);
            }
            if (mSprite == null && Atlas.spriteList.Count > 0)
            {
                UISpriteData sp = Atlas.spriteList[0];
                if (sp == null) return null;
                SetAtlasSprite(sp);
                if (mSprite == null)
                {
                    this.PRINT(Atlas.name + "seems to have a null sprite");

                    return null;
                }
                mSpriteName = mSprite.name;
            }
        }
        return mSprite;
    }

    public void InitFace(bool needSnap=true)
    {
        MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
        if(meshFilter==null)
        {
            return;
        }

        mesh = meshFilter.mesh;

        UISpriteData mSprite1 = Atlas.GetSprite(spriteName);
        if (mSprite1 == null) return;

        Texture tex = meshFilter.GetComponent<Renderer>().material.mainTexture;

        //Texture tex = gameObject.GetComponent<MeshRenderer>().material.mainTexture;

        Rect outer = new Rect(mSprite1.x, mSprite1.y, mSprite1.width, mSprite1.height);//0,0,1,1
        if(!mirrorX)
        {
            //mesh.uv = new Vector2[]{new Vector2(outer.xMin/tex.width,1.0f-outer.yMax/tex.height),//0,1,1,0
            //new Vector2(outer.xMax/tex.width,1.0f-outer.xMin/tex.height),
            //new Vector2(outer.xMax/tex.width,1.0f-outer.yMax/tex.height),
            //new Vector2(outer.xMin/tex.width,1.0f-outer.yMin/tex.height)};

            mesh.uv = new Vector2[]{new Vector2(outer.xMin/tex.width,1.0f-outer.yMax/tex.height),//0,1,1,0
            new Vector2(outer.xMax/tex.width,1.0f-outer.yMin/tex.height),
            new Vector2(outer.xMax/tex.width,1.0f-outer.yMax/tex.height),
            new Vector2(outer.xMin/tex.width,1.0f-outer.yMin/tex.height)};
        }
        else
        {
            //mesh.uv = new Vector2[]{new Vector2(outer.xMax/tex.width,1.0f-outer.yMax/tex.height),//1,0,0,1
            //new Vector2(outer.xMin/tex.width,1.0f-outer.yMin/tex.height),
            //new Vector2(outer.xMin/tex.width,1.0f-outer.yMax/tex.height),
            //new Vector2(outer.xMax/tex.width,1.0f-outer.xMin/tex.height)};

            mesh.uv = new Vector2[]{new Vector2(outer.xMax/tex.width,1.0f-outer.yMax/tex.height),//1,0,0,1
            new Vector2(outer.xMin/tex.width,1.0f-outer.yMin/tex.height),
            new Vector2(outer.xMin/tex.width,1.0f-outer.yMax/tex.height),
            new Vector2(outer.xMax/tex.width,1.0f-outer.yMin/tex.height)};    
        }
        float scale = (float)(Screen.height / 2.0f) / 5;
        transform.localScale = new Vector3((float)mSprite1.width / scale, (float)mSprite1.height / scale, 1.0f) * ScaleFactor;
    }


}
