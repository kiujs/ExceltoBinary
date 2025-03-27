using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
/// <summary>
/// 通过反射来获取应该调用的类型父类
/// </summary>
public class BinaryDataBase
{

    /// <summary>
    /// 读取二进制
    /// </summary>
    public virtual BinaryDataBase OnReadBinary(byte[] bytes ,ref int index)
    {
        return this;
    }
    /// <summary>
    /// 写入二进制
    /// </summary>
    public virtual byte[] OnWriteBinary(string Data = null, FileStream fileStream = null)
    {
        return null;
    }
}