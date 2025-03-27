using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
namespace B_Star
{
	public interface IBInaryConverterAdd
	{
		public string[] PropertyNames { get; }

		

		public void ConvertToBinary(BinaryWriter fs, object value,string Name);

		public object Parse(BinaryReader BinaryReader,string Name);
	}
}