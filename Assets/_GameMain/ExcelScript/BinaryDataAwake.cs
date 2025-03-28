using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinaryDataAwake : MonoBehaviour
{
    private void Awake()
    {
        BinaryDataMgr.LoadInsConfigure();
    }
}