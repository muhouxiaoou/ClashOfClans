using UnityEngine;
using System.Collections;

public class GridOverview : MonoBehaviour {

    /// <summary> 地形定位 </summary>
    public GameObject plane;
    /// <summary> 网格位置数组 </summary>
    public GameObject[] obj;

    /// <summary> 是否要显示主网格线 </summary>
    public bool showMain = true;
    /// <summary> 是否显示小格子 </summary>
    public bool showSub = false;
    /// <summary> 是否显示对象 </summary>
    public bool showObj = false;

    /// <summary> 格子X大小 </summary>
    public int gridSizeX;
    /// <summary> 格子Y大小 </summary>
    public int gridSizeY;
    /// <summary> 格子Z大小 </summary>
    public int gridSizeZ;

    /// <summary> 一个小格子的大小 </summary>
    public float smallStep;
    /// <summary> 一个大格子的大小 </summary>
    public float largeStep;

    /// <summary> 对象占用空间X大小 </summary>
    public int gridObjSizeX;
    /// <summary> 对象占用空间Y大小 </summary>
    public int gridObjSizeY;
    /// <summary> 对象占用空间Z大小 </summary>
    public int gridObjSizeZ;

    /// <summary> 起始点X坐标 </summary>
    public float startX;
    /// <summary> 起始点Y坐标 </summary>
    public float startY;
    /// <summary> 起始点Z坐标 </summary>
    public float startZ;

    /// <summary> 对象起始点X坐标 </summary>
    public float startObjX;
    /// <summary> 对象起始点Y坐标 </summary>
    public float startObjY;
    /// <summary> 对象起始点Z坐标 </summary>
    public float startObjZ;

    //偏移值
    private float offsetY = 0f;
    private float ScroRate = 0.1f;
    private float lastScroll = 0f;

    private Material lineMaterial;

    private Color mainColor = new Color(0f,1f,0f,1f);//绿色
    private Color subColor = new Color(0f, 0.5f, 0f, 1f);
    private Color objColor = new Color(1f, 0f, 0f, 1f);//红色


    ///// <summary>
    ///// 设置测试值，临时用的
    ///// </summary>
    //void SetDefaultValue()
    //{
    //    showSub = true;
    //    gridSizeX = 35;
    //    gridSizeZ = 25;
    //    smallStep = 1;
    //    largeStep = 10;
    //    gridObjSizeY = 1;
    //    gridObjSizeZ = 8;
    //    startZ = 4;
    //    startObjX = -2;
    //    startObjZ = -2;
    //}

    //void Awake()
    //{
    //    SetDefaultValue();
    //}

    /// <summary>
    /// 创建格子Shader
    /// </summary>
    void CreateLineMaterial()
    {
        if(!lineMaterial)
        {
            //直接在代码里创建Shader
            lineMaterial = new Material("Shader \"Lines/Colored Blended\" {" +
                                        "subShader{Pass {" +
                                        "   Blend SrcAlpha OneMinusSrcalpha " +
                                        "   Zwrite Off Cull Off Fog{ Mode Off} " +
                                        "   BindChannels {" +
                                        "   Bind \"vertex\",vertex Bind \"color\" , color }" +
                                        "} } } ");
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            lineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
        }
    }


    //在渲染后处理，附加后期处理
    //这个函数每帧都会执行，不停渲染
    /// <summary>
    /// Unity自带函数，在摄像机渲染后执行
    /// </summary>
    void OnPostRender()
    {
        CreateLineMaterial();
        lineMaterial.SetPass(0);//通道设置为0
        GL.Begin(GL.LINES);//画线
        if(showSub)//如果是绘制小网格
        {
            GL.Color(subColor);//设置一种颜色
            for (float j = 0; j <= gridSizeY; j+=smallStep)//先绘制列(移动X)
            {
                for(float i=0;i<= gridSizeZ-startZ;i+=smallStep)
                {
                    GL.Vertex3(startX, j + offsetY, startZ + i);
                    GL.Vertex3(gridSizeX, j + offsetY, startZ + i);
                }
                for (float i = 0; i <= gridSizeX - startX; i += smallStep)
                {
                    GL.Vertex3(startX + i, j + offsetY, startZ);
                    GL.Vertex3(startX + i, j + offsetY, gridSizeZ);
                }
            }
            for(float i=0;i<=gridSizeZ-startZ;i+=smallStep)
            {
                for(float k=0;k<=gridSizeX-startX;k+=smallStep)
                {
                    GL.Vertex3(startX + k, startY + offsetY, startZ + i);
                    GL.Vertex3(startX + k, gridSizeY + offsetY, startZ + i);
                }
            }
        }
        if(showMain)//制作大格子
        {
            GL.Color(mainColor);//设置一种颜色
            for (float j = 0; j <= gridSizeY; j += largeStep)//先绘制列(移动X)
            {
                for (float i = 0; i <= gridSizeZ - startZ; i += largeStep)
                {
                    GL.Vertex3(startX, j + offsetY, startZ + i);
                    GL.Vertex3(gridSizeX, j + offsetY, startZ + i);
                }
                for (float i = 0; i <= gridSizeX - startX; i += largeStep)
                {
                    GL.Vertex3(startX + i, j + offsetY, startZ);
                    GL.Vertex3(startX + i, j + offsetY, gridSizeZ);
                }
            }
            for (float i = 0; i <= gridSizeZ - startZ; i += largeStep)
            {
                for (float k = 0; k <= gridSizeX - startX; k += largeStep)
                {
                    GL.Vertex3(startX + k, startY + offsetY, startZ + i);
                    GL.Vertex3(startX + k, gridSizeY + offsetY, startZ + i);
                }
            }
        }
        GL.End();
    }
}
