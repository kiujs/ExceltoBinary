using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.InteropServices;
namespace B_Star
{
    public class BinAddConverArray : IBInaryConverterAdd
    {
        /// <summary>
        /// �޸�����ű�Ϊͬʱ֧�ֶ�ά���飿��
        /// </summary>
        public string[] PropertyNames { get; } = new string[] { "[]", "[][]" };

        private string l_Name;
        public void ConvertToBinary(BinaryWriter fs, object value, string Name)
        {
            l_Name = Name;
            if (l_Name.Contains("[][]"))
            {
                L_MultConvertoBinary(fs, value, Name);
            }
            else if (l_Name.Contains("[]"))
            {
                L_ConvertoBinary(fs, value, Name);
            }
        }
        /// <summary>
        /// һά���飬
        /// </summary>
        private void L_ConvertoBinary(BinaryWriter fs, object value, string Name)
        {
            //�жϲ����Ƴ��ַ���

            l_Name.Contains("[]");
            l_Name = l_Name.Replace("[]", "");
            //����ָ�����Ž����и�
            string[] strs = value.ToString().Split("|");
            //д�����鳤��
            fs.Write(strs.Length);
            //Ȼ�����д����������Ԫ��
            foreach (string str in strs)
            {
                if (BinaryConverMgr.BinaryConverter.ContainsKey(l_Name))
                {
                    BinaryConverMgr.BinaryConverter[l_Name].ConvertToBinary(fs, str);
                }
            }
        }
        /// <summary>
        /// ��ά����
        /// </summary>
        private void L_MultConvertoBinary(BinaryWriter fs, object value, string Name)
        {
            l_Name.Contains("[][]");
            l_Name = l_Name.Replace("[][]", "");
            //����ָ�����Ž����и�
            string[] strs = value.ToString().Split("|");
            fs.Write(strs.Length);
            for (int i = 0; i < strs.Length; i++)
            {
                string[] ChildStr = strs[i].Split("-");
                fs.Write(ChildStr.Length);
                for (int j = 0; j < ChildStr.Length; j++)
                {
                    if (BinaryConverMgr.BinaryConverter.ContainsKey(l_Name))
                    {
                        BinaryConverMgr.BinaryConverter[l_Name].ConvertToBinary(fs, ChildStr[j]);
                    }
                }
            }
        }



        public object Parse(BinaryReader BinaryReader, string Name)
        {
            l_Name = Name;
            if (l_Name.Contains("[][]"))
            {
                return l_MultParse(BinaryReader, Name);
            }
            else if (l_Name.Contains("[]"))
            {
                return l_parse(BinaryReader, Name);
            }
            return null;
        }

        private object l_parse(BinaryReader BinaryReader, string Name)
        {
            l_Name.Contains("[]");
            l_Name = l_Name.Replace("[]", "");

            int Length = BinaryReader.ReadInt32();
            Type arraytype = TypeChangeUtility.GetType(l_Name);
            //ͨ�����䴴���������
            Array array = Array.CreateInstance(arraytype, Length);
            for (int i = 0; i < Length; i++)
            {
                if (BinaryConverMgr.BinaryConverter.ContainsKey(l_Name))
                {
                    object obj = BinaryConverMgr.BinaryConverter[l_Name].Parse(BinaryReader);
                    array.SetValue(obj, i);
                }
            }
            return array;
        }

        private object l_MultParse(BinaryReader BinaryReader, string Name)
        {
            l_Name.Contains("[][]");
            l_Name = l_Name.Replace("[][]", "");

            int Length = BinaryReader.ReadInt32();

            Type arraytype = TypeChangeUtility.GetType(l_Name + "[]");
            //ͨ�����䴴���������
            Array array = Array.CreateInstance(arraytype, Length);
            for (int i = 0; i < Length; i++)
            {
                int ChildLength = BinaryReader.ReadInt32();
                Type ChildType = TypeChangeUtility.GetType(l_Name);
                Array ChildArray = Array.CreateInstance(ChildType, ChildLength);
                for (int j = 0; j < ChildLength; j++)
                {
                    if (BinaryConverMgr.BinaryConverter.ContainsKey(l_Name))
                    {
                        object obj = BinaryConverMgr.BinaryConverter[l_Name].Parse(BinaryReader);
                        ChildArray.SetValue(obj, j);
                    }
                }
                array.SetValue(ChildArray, i);
            }
            return array;
        }
    }
}
