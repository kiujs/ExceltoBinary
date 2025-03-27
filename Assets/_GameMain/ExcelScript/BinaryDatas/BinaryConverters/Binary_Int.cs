using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace B_Star
{
    /// <summary>
    /// intÀàÐÍ×ª»»Æ÷
    /// </summary>
    public class Binary_Int : IBinaryConverter
    {
        public string[] PropertyNames { get; } = { "int", "Int", "INT", "Int32" };

        public void ConvertToBinary(BinaryWriter fs, object value)
        {
            if (value == null)
                fs.Write(System.BitConverter.GetBytes(0), 0, 4);
            fs.Write(System.BitConverter.GetBytes(int.Parse(value.ToString())), 0, 4);
        }

        public object Parse(BinaryReader BinaryReader)
        {
            return BinaryReader.ReadInt32();
        }
    }
}