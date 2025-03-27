using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace B_Star
{
    public interface IBinaryConverter
    {
        /// <summary>
        /// ת�������������� - �ɶ�ѡ
        /// </summary>
        public string[] PropertyNames { get; }
        /// <summary>
        /// ������ת��Ϊ����������
        /// </summary>
        /// <param name="fs"></param>
        /// <param name="value"></param>
        public void ConvertToBinary(BinaryWriter fs, object value);
        /// <summary>
        /// �Ӷ����������н�������
        /// </summary>
        public object Parse(BinaryReader BinaryReader);
    }
}