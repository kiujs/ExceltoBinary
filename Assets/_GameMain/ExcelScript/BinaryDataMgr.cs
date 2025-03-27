using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

using B_Star;

/// <summary>
/// 2进制数据管理器
/// </summary>
public static class BinaryDataMgr
{

    /// <summary>
    /// 用于存储所有Excel表数据的容器
    /// </summary>
    private static Dictionary<string, object> tableDic = new Dictionary<string, object>();
    public static void LoadTable<T, K>()
    {
        if (tableDic.ContainsKey(typeof(T).Name))
        {
            tableDic.Remove(typeof(T).Name);
        }
        //Debug.Log("加载进来：：：" + SAVE_PATH + typeof(K).Name);
        //如果这个路径存在的话读取正常存储路径中的数据
        if (File.Exists(AssetUtility.DataTableNewBinaryData_Path + typeof(K).Name + ".txt"))
        {
            //Debug.Log("加载路径存在");
            //读取Excel表对应的二进制文件进行解析
            using (FileStream fs = File.Open(AssetUtility.DataTableNewBinaryData_Path + typeof(K).Name + ".txt", FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader reader = new BinaryReader(fs))
                {
                    int Count = reader.ReadInt32();
                    string keyName = reader.ReadString();


                    Type contaninerType = typeof(T);
                    object contaninerObj = Activator.CreateInstance(contaninerType);//通过类型创建实例         
                    Type ClassType = typeof(K);
                    //通过反射得到所有数据结构类 所有字段的信息 - 然后为每个字段赋值
                    FieldInfo[] infos = GetAllFields(ClassType);

                    for (int i = 0; i < Count; i++)
                    {
                        object dataobj = Activator.CreateInstance(ClassType);
                        foreach (FieldInfo info in infos)
                        {
                            //判断数组类型
                            if (BinaryConverAdditionMgr.GetContainsKey(info.FieldType.Name))
                            {
                                info.SetValue(dataobj, BinaryConverAdditionMgr.Parse(reader, info.FieldType.Name));
                            }
                            else if (BinaryConverMgr.BinaryConverter.ContainsKey(info.FieldType.Name))
                            {
                                info.SetValue(dataobj, BinaryConverMgr.BinaryConverter[info.FieldType.Name].Parse(reader));
                            }
                            else
                            {
                                info.SetValue(dataobj, reader.ReadInt32());
                            }
                        }
                        object dicObject = contaninerType.GetField("dataDic").GetValue(contaninerObj);
                        MethodInfo mInfo = dicObject.GetType().GetMethod("Add");
                        object keyValue = ClassType.GetField(keyName).GetValue(dataobj);
                        mInfo.Invoke(dicObject, new object[] { keyValue, dataobj });
                    }
                    //把读取完的表记录下来。
                    tableDic.Add(typeof(T).Name, contaninerObj);
                }
            }
        }
        else
        {
            Debug.Log("未找到指定二进制文件" + AssetUtility.DataTableNewBinaryData_Path + typeof(K).Name + ".txt");
            return;
        }
    }
    /// <summary>
    /// 加载配置表数据 -- >加载指定位置的东西
    /// </summary>
    public static void LoadInsConfigure()
    {

        if (!File.Exists(AssetUtility.DataTableNewBinaryData_Path + AssetUtility.BinConName))
        {
            Debug.Log("当前不存在 - ConloadName 文件" + " 路径 " + AssetUtility.DataTableNewBinaryData_Path + "|||" + AssetUtility.BinConName);
        }
        using (FileStream fs = File.Open(AssetUtility.DataTableNewBinaryData_Path + AssetUtility.BinConName, FileMode.Open, FileAccess.Read))
        {
            using (BinaryReader reader = new BinaryReader(fs))
            {
                int count = reader.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    //读取长度X2
                    string nameOne = reader.ReadString();
                    string nameTwo = reader.ReadString();
                    //加载东西 -->
                    Type Insnameone = Type.GetType(nameOne + ",Assembly-CSharp");
                    Type InsnameTwo = Type.GetType(nameTwo + ",Assembly-CSharp");

                    //通过反射调用 LoadTable方法 加载数据表
                    MethodInfo method = typeof(BinaryDataMgr).GetMethod("LoadTable");
                    //通过MakeGenericMethod()方法，传入两个类型变量，构建一个传入了泛型参数的MethodInfo
                    MethodInfo genericMethod = method.MakeGenericMethod(InsnameTwo, Insnameone);
                    //在执行这个最终已经得到类型参数的MethodInfo，执行方法的对象是instance也就是本类的单例对象，第二个参数传入null(因为本来就是无参方法)
                    genericMethod.Invoke(null, null);
                }
            }
        }
        //先读取一个整体长度
    }
    /// <summary>
    /// 获取对应类型的表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T GetTable<T>() where T : class
    {
        string tableName = typeof(T).Name;
        if (tableDic.ContainsKey(tableName))
        {
            return tableDic[tableName] as T;
        }
        return null;
    }
    //递归查找类型父类
    public static FieldInfo[] GetAllFields(Type type)
    {
        FieldInfo[] fields = type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        Type baseType = type.BaseType;
        if (baseType != null)
        {
            FieldInfo[] baseFields = GetAllFields(baseType);
            FieldInfo[] allFields = new FieldInfo[fields.Length + baseFields.Length];
            baseFields.CopyTo(allFields, 0);
            fields.CopyTo(allFields, baseFields.Length);
            return allFields;
        }
        else
        {
            return fields;
        }
    }
}