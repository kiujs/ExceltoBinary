using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
/// <summary>
/// ͨ����������ȡӦ�õ��õ����͸���
/// </summary>
public class BinaryDataBase
{

    /// <summary>
    /// ��ȡ������
    /// </summary>
    public virtual BinaryDataBase OnReadBinary(byte[] bytes ,ref int index)
    {
        return this;
    }
    /// <summary>
    /// д�������
    /// </summary>
    public virtual byte[] OnWriteBinary(string Data = null, FileStream fileStream = null)
    {
        return null;
    }
}