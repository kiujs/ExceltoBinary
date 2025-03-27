using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace B_Star
{
    public class Binary_Bool : IBinaryConverter
    {
        public string[] PropertyNames { get; }= new string[] { "Bool","bool","boolean","Boolean"};

        public void ConvertToBinary(BinaryWriter fs, object value)
        {
            if (value == null)
                fs.Write(false);
            fs.Write(System.BitConverter.GetBytes(bool.Parse(value.ToString())), 0, 1);
        }

        public object Parse(BinaryReader BinaryReader)
        {
            return BinaryReader.ReadBoolean();
        }
    }
}