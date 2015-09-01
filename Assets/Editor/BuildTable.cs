using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class BuildTable : Editor
{
    [MenuItem("BuildAssetBundle/Build Csv")]
    static void BuildCsv()
    {
        string applicationPath = Application.dataPath;
        string saveDir = applicationPath + "/StreamingAssets/";
        string savePath = saveDir + "csv.assetbundle";

        Object[] selections = Selection.GetFiltered(typeof(object), SelectionMode.DeepAssets);
        List<Object> outs = new List<Object>();
        for (int i = 0, max = selections.Length; i < max; i++)
        {
            Object obj = selections[i];
            string fileAssetPath = AssetDatabase.GetAssetPath(obj);
            if (fileAssetPath.Substring(fileAssetPath.LastIndexOf(".") + 1) != "csv")
                continue;
            string fileWholePath = applicationPath + "/" + fileAssetPath.Substring(fileAssetPath.IndexOf("/"));//总路径

            soCsv csv = ScriptableObject.CreateInstance<soCsv>();

            csv.fileName = obj.name;
            csv.content = File.ReadAllBytes(fileWholePath);

            string assetPathTemp = "Assets/Resource_Local/Temp/" + obj.name + ".asset";
            AssetDatabase.CreateAsset(csv, assetPathTemp);

            Object outObj = AssetDatabase.LoadAssetAtPath(assetPathTemp, typeof(soCsv));

            Debug.Log("package: " + outObj.name);

            outs.Add(outObj);

        }

        Object[] outObjs = outs.ToArray();
        //BuildAssetBundleOptions.CollectDependencies相关联的内容都打包进去/CompleteAssets把整个包打在一起，5.0默认enable
        //UncompressedAssetBundle不压缩的打包，读取快，包比较大、DisableWriteTypeTree打包小，查找慢、DeterministicAssetBundle自定义打包方式，用不到
        if (BuildPipeline.BuildAssetBundle(null, outs.ToArray(), savePath, BuildAssetBundleOptions.CollectDependencies, BuildTarget.Android))
            EditorUtility.DisplayDialog("OK", "build" + savePath + "success,length=" + outObjs.Length, "OK");
        else
            Debug.LogWarning("Build" + savePath + "failed");

        AssetDatabase.Refresh();
    }
}
