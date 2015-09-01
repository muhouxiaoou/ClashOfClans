using UnityEngine;
using System.Collections;

//继承ScriptableObject说明soCsv只是一个脚本,而且脚本可以被序列化
/// <summary>
/// 
/// </summary>
public class soCsv : ScriptableObject {
    public string fileName;
    public byte[] content;
}
