using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
namespace B_Star
{
    /// <summary>
    /// 用作附加类型的转换器 转换 类似数组，队列等额外类型
    /// </summary>
    public static class BinaryConverAdditionMgr
    {
        #region 基本转换机添加
        public static Dictionary<string, IBInaryConverterAdd> BinaryConverAddition = new Dictionary<string, IBInaryConverterAdd>();
        private static bool IsConverter = false;


        public static bool GetContainsKey(string Name)
        {
            foreach (var item in BinaryConverAddition)
            {
                if (Name.Contains(item.Key))
                {
                    return true;
                }
            }
            return false;
        }
        public static void ConvertToBinary(BinaryWriter fs, object value, string Name)
        {
            foreach (var item in BinaryConverAddition)
            {
                if (Name.Contains(item.Key))
                {
                    item.Value.ConvertToBinary(fs, value, Name);
                    return;
                }
            }
        }

        public static object Parse(BinaryReader fs, string Name)
        {
            foreach (var item in BinaryConverAddition)
            {
                if (Name.Contains(item.Key))
                {
                    return item.Value.Parse(fs, Name);
                }
            }
            return false;
        }

        static BinaryConverAdditionMgr()
        {
            AddAllBinConver();
        }
        /// <summary>
        /// 添加所有附加转换器
        /// </summary>
        private static void AddAllBinConver()
        {
            if (IsConverter) return;
            AddBinConverAddition(new BinAddConverArray());
            IsConverter = true;
        }

        private static void AddBinConverAddition(IBInaryConverterAdd ConverAdd)
        {
            foreach (var item in ConverAdd.PropertyNames)
            {
                if (!BinaryConverAddition.ContainsKey(item))
                {
                    BinaryConverAddition.Add(item, ConverAdd);
                }
            }
        }
        #endregion
    }
}
