using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;


namespace B_Star
{
    /// <summary>
    /// 2023/7/3拓展,二进制生成类的继承 -- 由一个类继承另一个类
    /// </summary>
    public static class ExcelTool
    {
        /// <summary>
        /// 真正内容开始的行号
        /// </summary>
        public static int BEGIN_INDEX = 4;

        public static string ContainerClassName = "Container";
        #region 生成数据结构类
        /// <summary>
        /// 生成Excel表对应的 数据结构类
        /// </summary>
        /// <param name="table"></param>
        public static void GenerateExcelDataClass(ExcelWorksheet table)
        {
            //字段名行
            string[] rowName = GetVariableNameRow(table);
            //类型行
            string[] rowType = GetVariableTypeRow(table);
            //注释行
            string[] rowNote = GetVariableNoteRow(table);

            //判断路径是否存在，如果没有，则创建文件夹
            if (!Directory.Exists(AssetUtility.DataTableNewScript_Class_Path))
                Directory.CreateDirectory(AssetUtility.DataTableNewScript_Class_Path);
            string str = "/*____" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "______________________________________" + "\n" +
                         "___此脚本为自动生成，请勿修改_____B_Star___咩！___________________" + "\n" +
                         "_______________________________________________________________*/\n";
            str += "using UnityEngine;\n";
            str += "using System;\n";
            str += "using System.Collections.Generic;\n";
            str += "[System.Serializable]\n";
            //是否继承了其他类
            int isInheritID = GetInherit(table);
            bool isInherit = false;
            List<String> InherparentField = new List<string>();
            if (isInheritID == -1)
            {
                isInherit = false;
                str += "public class " + table.Name + "\n{\n";
            }
            else
            {
                isInherit = true;
                str += "public class " + table.Name + " :" + GetVariableTypeRow(table)[GetInherit(table)].ToString() + "\n{\n";
                //这里要获取父类的所有公开的字段 然后通过这其中的字段来判断， 削减下面的判断
                FieldInfo[] fields = Type.GetType(GetVariableTypeRow(table)[GetInherit(table)].ToString() + ",Assembly-CSharp").GetFields();
                foreach (var item in fields)
                {
                    InherparentField.Add(item.Name);
                }
            }
            //进行字符串拼接 遍历表中的列数
            for (int i = 0; i < table.Dimension.End.Column; i++)
            {
                if (isInherit)
                {
                    if (i == isInheritID)
                    {
                        continue;
                    }
                    bool isInher = false;
                    foreach (var item in InherparentField)
                    {
                        if (item == rowName[i].ToString())
                        {
                            isInher = true;
                        }
                    }
                    if (isInher)
                    {
                        continue;
                    }
                }
                //为字段添加注释
                str += "    /// <summary>\n";
                str += "    /// " + rowNote[i].ToString() + "\n";
                str += "    /// </summary>\n";
                str += "    " + "public " + rowType[i].ToString() + " " + rowName[i].ToString() + ";\n";
            }

            ////添加拷贝构造方法 -- 
            //str += "    public void " + "ClassCopy" + "(" + table.Name + " Data" + ") \n";
            //str += "    {\n";
            //int[][] ass = new int[table.Dimension.End.Column][];
            ////添加拷贝方法内部属性
            //for (int i = 0; i < table.Dimension.End.Column; i++)
            //{
            //    //如果其中包含数组，则不应当使用引用复制， 会出现复制问题
            //    if (rowName[i].ToString() == "继承")
            //    {
            //        continue;
            //    }
            //    else if (rowType[i].ToString().Contains("[][]"))
            //    {
            //        str += "        " + rowName[i].ToString() + " = " + "new " + rowType[i].ToString().Replace("[]", "") + "[" + "Data." + rowName[i].ToString() + ".Length" + "];\n";
            //        str += "        " + "Array.Copy(" + "Data." + rowName[i].ToString() + "," + rowName[i].ToString() + "," + "Data." + rowName[i].ToString() + ".Length" + ");\n";
            //    }
            //    else if (rowType[i].ToString().Contains("[]"))
            //    {
            //        str += "        " + rowName[i].ToString() + " = " + "new " + rowType[i].ToString().Replace("[]", "") + "[" + "Data." + rowName[i].ToString() + ".Length" + "];\n";
            //        str += "        " + "Array.Copy(" + "Data." + rowName[i].ToString() + "," + rowName[i].ToString() + "," + "Data." + rowName[i].ToString() + ".Length" + ");\n";
            //    }
            //    else if (rowType[i].ToString().Contains("Data"))
            //    {
            //        str += "        " + rowName[i].ToString() + " = " + "new " + rowType[i].ToString() + "(" + "Data." + rowName[i].ToString() + ")" + ";\n";
            //        //然后复制
            //        str += "        " + rowName[i].ToString() + ".DataSetUp();\n";
            //    }
            //    else
            //    {
            //        if (GetInherit(table) != -1 && rowType[i].ToString() == GetVariableTypeRow(table)[GetInherit(table)].ToString())
            //            continue;
            //        str += "        " + rowName[i].ToString() + " = " + "Data." + rowName[i].ToString() + ";\n";
            //    }
            //}
            //str += "    }\n";
            str += "}";
            //把拼接好的字符串存到指定文件中去
            File.WriteAllText(AssetUtility.DataTableNewScript_Class_Path + table.Name + ".cs", str);
            //刷新方法
            AssetDatabase.Refresh();
        }
        /// <summary>
        /// 获取变量名所在行
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        private static string[] GetVariableNameRow(ExcelWorksheet table)
        {
            return table.GetExcelWorkSheetRow(1);
        }
        /// <summary>
        /// 获取变量类型所在行
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        private static string[] GetVariableTypeRow(ExcelWorksheet table)
        {
            return table.GetExcelWorkSheetRow(2);
        }
        /// <summary>
        /// 获取注释所在行
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        private static string[] GetVariableNoteRow(ExcelWorksheet table)
        {
            return table.GetExcelWorkSheetRow(4);
        }
        /// <summary>
        /// 获取某行信息
        /// </summary>
        /// <param name="table"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        private static string[] GetExcelWorkSheetRow(this ExcelWorksheet table, int rowID)
        {
            string[] row = new string[table.Dimension.End.Column];
            for (int i = 1; i <= table.Dimension.End.Column; i++)
            {
                row[i - 1] = table.Cells[rowID, i].Value.ToString();
            }
            return row;
        }
        #endregion
        #region 生成容器类
        /// <summary>
        /// 生成Excel表对应的数据容器类
        /// </summary>
        /// <param name="table"></param>
        public static void GenerateExcelContainer(ExcelWorksheet table)
        {
            Debug.Log("执行生成容器类 路径" + AssetUtility.DataTableNewScript_Container_Path);
            //得到主键索引
            int keyIndex = GetKeyIndex(table);
            //得到字段类型
            string[] rowType = GetVariableTypeRow(table);
            //创建容器类信息 -- 判断文件夹是否存在，如果不存在，则创建这个文件夹
            if (!Directory.Exists(AssetUtility.DataTableNewScript_Container_Path))
                Directory.CreateDirectory(AssetUtility.DataTableNewScript_Container_Path);
            string str = "/*____" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "______________________________________" + "\n" +
                         "___此脚本为自动生成，请勿修改_____B_Star___咩！___________________" + "\n" +
                         "_______________________________________________________________*/\n";
            str += "using System.Collections.Generic;\n";
            str += "using UnityEngine;\n";
            str += "public class " + table.Name + ContainerClassName + "\n{\n";
            str += "    ";
            str += "public Dictionary<" + rowType[keyIndex].ToString() + ", " + table.Name.ToString() + ">";
            str += " dataDic = new Dictionary<" + rowType[keyIndex].ToString() + ", " + table.Name.ToString() + ">" + "();\n";
            str += "}";
            File.WriteAllText(AssetUtility.DataTableNewScript_Container_Path + table.Name.ToString() + "Container.cs", str);
            AssetDatabase.Refresh();
        }
        /// <summary>
        /// 获取主键索引
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        private static int GetKeyIndex(ExcelWorksheet table)
        {
            string[] row = new string[table.Dimension.End.Column];

            for (int i = 1; i <= table.Dimension.End.Column; i++)
            {
                row[i - 1] = table.Cells[3, i].Value == null ? "" : table.Cells[3, i].Value.ToString();
            }

            for (int i = 1; i <= table.Dimension.End.Column; i++)
            {
                if (row[i - 1].ToString() == "key" || row[i].ToString() == "Key")
                {
                    return i - 1;
                }
            }
            return 0;
        }
        /// <summary>
        /// 获取继承
        /// </summary>
        /// <returns></returns>
        private static int GetInherit(ExcelWorksheet table)
        {
            string[] row = new string[table.Dimension.End.Column];

            for (int i = 1; i <= table.Dimension.End.Column; i++)
            {
                Debug.Log(row[i - 1]);
                Debug.Log(table.Cells[3, i].Value);
                row[i - 1] = table.Cells[3, i].Value == null ? "" : table.Cells[3, i].Value.ToString();
            }

            for (int i = 0; i < row.Length; i++)
            {
                if (row[i].ToString() == "继承" || row[i].ToString() == "Inherit")
                {
                    return i;
                }
            }
            return -1;
        }
        #endregion
        #region 生成二进制文件
        /// <summary>
        /// 生成二进制文件
        /// </summary>
        /// <param name="Data"></param>
        public static void GenerateExcelBinary(ExcelWorksheet table, string Path)
        {
            String LoadPath = Path;
            if (!Directory.Exists(LoadPath))
            {
                Debug.LogError("路径不存在，无法生成二进制文件");
                Directory.CreateDirectory(LoadPath);
            }
            //创建一个二进制文件进行写入
            using (FileStream fs = new FileStream(LoadPath + table.Name + ".txt", FileMode.OpenOrCreate, FileAccess.Write))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    //存储具体的excel对应的二进制信息
                    //1，先存储需要写多少行数据，方便后续读取
                    //-4的原因是因为 前面4行是配置规则 并不是我们需要记录的数据内容
                    bw.Write(table.Dimension.End.Row - BEGIN_INDEX);

                    //判断继承！！！
                    int Inherit = -1;
                    if (GetInherit(table) != -1)
                        Inherit = GetInherit(table);
                    //2，存储主键的变量名
                    string keyName = GetVariableNameRow(table)[GetKeyIndex(table)].ToString();
                    byte[] bytes = Encoding.UTF8.GetBytes(keyName);
                    //写入主键的变量名
                    bw.Write(keyName);
                    //遍历所有内容的行 进行二进制的写入
                    string[] row;
                    //得到类型行，根据类型来决定如何写入数据
                    string[] rowType = GetVariableTypeRow(table);
                    for (int i = BEGIN_INDEX + 1; i <= table.Dimension.End.Row; i++)
                    {
                        //得到一行的数据
                        row = table.GetExcelWorkSheetRow(i);
                        for (int j = 0; j < table.Dimension.End.Column; j++)
                        {
                            if (j == Inherit)//如果处于继承状态的话
                            {
                                continue;
                            }
                            try
                            {
                                //感觉需要一个拓展器进行额外附加类型的转换，例如List<> 数组等等的附加嵌套类型。
                                if (BinaryConverAdditionMgr.GetContainsKey(rowType[j].ToString()))
                                {
                                    BinaryConverAdditionMgr.ConvertToBinary(bw, row[j].ToString(), rowType[j].ToString());
                                }
                                else if (BinaryConverMgr.BinaryConverter.ContainsKey(rowType[j].ToString()))
                                {
                                    BinaryConverMgr.BinaryConverter[rowType[j].ToString()].ConvertToBinary(bw, row[j].ToString());
                                }
                                //如果没有对应的转换器，则默认为枚举类型 - 也就是所谓的int   
                                else
                                {
                                    fs.Write(BitConverter.GetBytes(int.Parse(row[j].ToString())), 0, 4);
                                }
                            }
                            catch (Exception e)
                            {
                                Debug.LogError("报错，表" + table.Name + "此类型" + rowType[j].ToString() + "出现问题" + row[j].ToString() + "行坐标" + i.ToString() + "列坐标" + j.ToString() + "数值信息" + row[j].ToString() + "错误信息 ：" + e.Message);
                            }
                        }
                    }
                }
            }
            AssetDatabase.Refresh();
        }
        #endregion
        #region 自动生成用于加载的配置表，供给其他脚本进行加载
        public static void InsConfigure(List<ExcelWorksheet> tableConllection, string Path)
        {
            String LoadPath = Path;
            if (!Directory.Exists(LoadPath))
            {
                Directory.CreateDirectory(LoadPath);
            }
            //使用二进制流进行写入
            using (FileStream fs = new FileStream(LoadPath + AssetUtility.BinConName, FileMode.OpenOrCreate, FileAccess.Write))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    bw.Write(tableConllection.Count);
                    //开始写入具体字符串
                    foreach (ExcelWorksheet table in tableConllection)
                    {
                        bw.Write(table.Name);
                        bw.Write(table.Name + ContainerClassName);
                    }
                }
            }
            AssetDatabase.Refresh();
        }
        #endregion
    }
}