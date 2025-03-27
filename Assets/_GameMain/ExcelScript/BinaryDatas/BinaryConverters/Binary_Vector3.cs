using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;
namespace B_Star
{
    public class Binary_Vector3 : IBinaryConverter
    {
        public string[] PropertyNames { get; } = new string[] { "vector3", "Vector3" };

        public void ConvertToBinary(BinaryWriter fs, object value)
        {
            if (value == null)
            {
                fs.Write(System.BitConverter.GetBytes(0), 0, 4);
                fs.Write(System.BitConverter.GetBytes(0), 0, 4);
                fs.Write(System.BitConverter.GetBytes(0), 0, 4);
                return;
            }
            else
            {
                string[] strs = value.ToString().Split(",");
                fs.Write(System.BitConverter.GetBytes(float.Parse(strs[0])), 0, 4);
                fs.Write(System.BitConverter.GetBytes(float.Parse(strs[1])), 0, 4);
                fs.Write(System.BitConverter.GetBytes(float.Parse(strs[2])), 0, 4);
            }
        }
        

        public object Parse(BinaryReader BinaryReader)
        {
            float x = BinaryReader.ReadSingle();
            float y = BinaryReader.ReadSingle();
            float z = BinaryReader.ReadSingle();
            return new Vector3(x, y, z);
        }
    }
}
