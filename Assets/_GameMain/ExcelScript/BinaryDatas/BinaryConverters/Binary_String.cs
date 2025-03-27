using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
namespace B_Star
{
    public class Binary_String : IBinaryConverter
    {
        public string[] PropertyNames { get; } = new string[] { "string", "String" };

        public void ConvertToBinary(BinaryWriter fs, object value)
        {
            fs.Write((string)value);
        }
        public object Parse(BinaryReader BinaryReader)
        {
            return BinaryReader.ReadString();
        }
    }
}
