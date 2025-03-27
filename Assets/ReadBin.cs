using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace B_Star
{
	public class ReadBin : MonoBehaviour
	{
    	// Start is called before the first frame update
    	void Start()
    	{
            GameSettingContainer gameSettingContainer = BinaryDataMgr.GetTable<GameSettingContainer>();
            foreach (var item in gameSettingContainer.dataDic)
            {
                Debug.Log(item.Key + "|" + item.Value.ID + "|" + item.Value.Str);

                Debug.Log("数组遍历开始");
                foreach (var item2 in item.Value.M_IntArray)
                {
                    Debug.Log("Array" + item2);
                }
                Debug.Log("二维数组遍历开始----------------");
                foreach (var item3 in item.Value.M_IntArray_Array)
                {
                    foreach (var item4 in item3)
                    {
                        Debug.Log("二维数组" + item4);
                    }
                }
            }
    	}
	
    	// Update is called once per frame
    	void Update()
    	{
       	 
    	}
	}
}
