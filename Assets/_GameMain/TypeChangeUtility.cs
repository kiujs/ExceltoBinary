using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace B_Star
{
    /// <summary>
    /// ���ж������ת���Ĺ�����
    /// </summary>
    public static class TypeChangeUtility
    {
        public static Type GetType(string typeName)
        {
            Type GetType = Type.GetType(typeName);
            if (GetType == null)
                GetType = Type.GetType("System." + typeName + ", mscorlib");
            if (GetType == null)
                GetType = Type.GetType("B_Star." + typeName + ", Assembly-CSharp");
            if (GetType == null)
                GetType = Type.GetType(typeName + ", Assembly-CSharp");
            if (GetType == null)
                GetType = Type.GetType("UnityEngine." + typeName + ", UnityEngine.CoreModule");
            if (GetType == null)
            {
                return null;
            }
            return GetType;
        }
    }
}
