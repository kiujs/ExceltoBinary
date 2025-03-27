using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinaryDataAwake : MonoBehaviour
{
    private void Awake()
    {
        BinaryDataMgr.LoadInsConfigure();
    }
    /// <summary>
    /// 退出时存储数据
    /// </summary>
    private void OnApplicationQuit()
    {

    }
}