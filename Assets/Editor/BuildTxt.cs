using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

public class BuildTxt : MonoBehaviour {

	[MenuItem("BuildAssetBundle/Build Txt")]
    static void BuildTxtMethod()
    {
        string applicationPath = Application.dataPath;
        string saveDir = applicationPath + "/streamingAssets/";
        string savePath = saveDir + "txt.assetbundle";
        List<Object> outs = new List<Object>();

        Object[] selections = Selection.GetFiltered(typeof(object), SelectionMode.DeepAssets);
        for (int i = 0, max = selections.Length; i < max; i++)
        {
            Object obj = selections[i];
            string fileAssetPath = AssetDatabase.GetAssetPath(obj);
            if (fileAssetPath.Substring(fileAssetPath.LastIndexOf(".") + 1) != "txt")
                continue;
            string fileWholePath = applicationPath + "/" + fileAssetPath.Substring(fileAssetPath.IndexOf("/"));//总路径
            outs.Add(obj);
        }
        if (BuildPipeline.BuildAssetBundle(null,outs.ToArray(),savePath,BuildAssetBundleOptions.CollectDependencies|BuildAssetBundleOptions.CompleteAssets,BuildTarget.Android))
            EditorUtility.DisplayDialog("OK", "build" + savePath + "success,length=" + outs.Count, "OK");
        else
            Debug.LogWarning("Build" + savePath + "failed");
    }
}
