using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace B_Star
{
    public interface IBinaryConverter
    {
        /// <summary>
        /// 转换器的属性名称 - 可多选
        /// </summary>
        public string[] PropertyNames { get; }
        /// <summary>
        /// 将数据转换为二进制数据
        /// </summary>
        /// <param name="fs"></param>
        /// <param name="value"></param>
        public void ConvertToBinary(BinaryWriter fs, object value);
        /// <summary>
        /// 从二进制数据中解析数据
        /// </summary>
        public object Parse(BinaryReader BinaryReader);
    }
}