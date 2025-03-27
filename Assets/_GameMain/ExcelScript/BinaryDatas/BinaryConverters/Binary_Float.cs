using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace B_Star
{
    public class Binary_Float : IBinaryConverter
    {
        public string[] PropertyNames { get; } = { "Float", "float", "Single" };

        public void ConvertToBinary(BinaryWriter fs, object value)
        {
            if (value == null)
            {
                fs.Write(System.BitConverter.GetBytes(0f), 0, 4);
            }
            fs.Write(System.BitConverter.GetBytes(float.Parse(value.ToString())), 0, 4);
        }

        public object Parse(BinaryReader BinaryReader)
        {
            return BinaryReader.ReadSingle();
        }
    }
}
