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
    /// 2023/7/3��չ,������������ļ̳� -- ��һ����̳���һ����
    /// </summary>
    public static class ExcelTool
    {
        /// <summary>
        /// �������ݿ�ʼ���к�
        /// </summary>
        public static int BEGIN_INDEX = 4;

        public static string ContainerClassName = "Container";
        #region �������ݽṹ��
        /// <summary>
        /// ����Excel���Ӧ�� ���ݽṹ��
        /// </summary>
        /// <param name="table"></param>
        public static void GenerateExcelDataClass(ExcelWorksheet table)
        {
            //�ֶ�����
            string[] rowName = GetVariableNameRow(table);
            //������
            string[] rowType = GetVariableTypeRow(table);
            //ע����
            string[] rowNote = GetVariableNoteRow(table);

            //�ж�·���Ƿ���ڣ����û�У��򴴽��ļ���
            if (!Directory.Exists(AssetUtility.DataTableNewScript_Class_Path))
                Directory.CreateDirectory(AssetUtility.DataTableNewScript_Class_Path);
            string str = "/*____" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "______________________________________" + "\n" +
                         "___�˽ű�Ϊ�Զ����ɣ������޸�_____B_Star___�㣡___________________" + "\n" +
                         "_______________________________________________________________*/\n";
            str += "using UnityEngine;\n";
            str += "using System;\n";
            str += "using System.Collections.Generic;\n";
            str += "[System.Serializable]\n";
            //�Ƿ�̳���������
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
                //����Ҫ��ȡ��������й������ֶ� Ȼ��ͨ�������е��ֶ����жϣ� ����������ж�
                FieldInfo[] fields = Type.GetType(GetVariableTypeRow(table)[GetInherit(table)].ToString() + ",Assembly-CSharp").GetFields();
                foreach (var item in fields)
                {
                    InherparentField.Add(item.Name);
                }
            }
            //�����ַ���ƴ�� �������е�����
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
                //Ϊ�ֶ����ע��
                str += "    /// <summary>\n";
                str += "    /// " + rowNote[i].ToString() + "\n";
                str += "    /// </summary>\n";
                str += "    " + "public " + rowType[i].ToString() + " " + rowName[i].ToString() + ";\n";
            }

            ////��ӿ������췽�� -- 
            //str += "    public void " + "ClassCopy" + "(" + table.Name + " Data" + ") \n";
            //str += "    {\n";
            //int[][] ass = new int[table.Dimension.End.Column][];
            ////��ӿ��������ڲ�����
            //for (int i = 0; i < table.Dimension.End.Column; i++)
            //{
            //    //������а������飬��Ӧ��ʹ�����ø��ƣ� ����ָ�������
            //    if (rowName[i].ToString() == "�̳�")
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
            //        //Ȼ����
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
            //��ƴ�Ӻõ��ַ����浽ָ���ļ���ȥ
            File.WriteAllText(AssetUtility.DataTableNewScript_Class_Path + table.Name + ".cs", str);
            //ˢ�·���
            AssetDatabase.Refresh();
        }
        /// <summary>
        /// ��ȡ������������
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        private static string[] GetVariableNameRow(ExcelWorksheet table)
        {
            return table.GetExcelWorkSheetRow(1);
        }
        /// <summary>
        /// ��ȡ��������������
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        private static string[] GetVariableTypeRow(ExcelWorksheet table)
        {
            return table.GetExcelWorkSheetRow(2);
        }
        /// <summary>
        /// ��ȡע��������
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        private static string[] GetVariableNoteRow(ExcelWorksheet table)
        {
            return table.GetExcelWorkSheetRow(4);
        }
        /// <summary>
        /// ��ȡĳ����Ϣ
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
        #region ����������
        /// <summary>
        /// ����Excel���Ӧ������������
        /// </summary>
        /// <param name="table"></param>
        public static void GenerateExcelContainer(ExcelWorksheet table)
        {
            Debug.Log("ִ������������ ·��" + AssetUtility.DataTableNewScript_Container_Path);
            //�õ���������
            int keyIndex = GetKeyIndex(table);
            //�õ��ֶ�����
            string[] rowType = GetVariableTypeRow(table);
            //������������Ϣ -- �ж��ļ����Ƿ���ڣ���������ڣ��򴴽�����ļ���
            if (!Directory.Exists(AssetUtility.DataTableNewScript_Container_Path))
                Directory.CreateDirectory(AssetUtility.DataTableNewScript_Container_Path);
            string str = "/*____" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "______________________________________" + "\n" +
                         "___�˽ű�Ϊ�Զ����ɣ������޸�_____B_Star___�㣡___________________" + "\n" +
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
        /// ��ȡ��������
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
        /// ��ȡ�̳�
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
                if (row[i].ToString() == "�̳�" || row[i].ToString() == "Inherit")
                {
                    return i;
                }
            }
            return -1;
        }
        #endregion
        #region ���ɶ������ļ�
        /// <summary>
        /// ���ɶ������ļ�
        /// </summary>
        /// <param name="Data"></param>
        public static void GenerateExcelBinary(ExcelWorksheet table, string Path)
        {
            String LoadPath = Path;
            if (!Directory.Exists(LoadPath))
            {
                Debug.LogError("·�������ڣ��޷����ɶ������ļ�");
                Directory.CreateDirectory(LoadPath);
            }
            //����һ���������ļ�����д��
            using (FileStream fs = new FileStream(LoadPath + table.Name + ".txt", FileMode.OpenOrCreate, FileAccess.Write))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    //�洢�����excel��Ӧ�Ķ�������Ϣ
                    //1���ȴ洢��Ҫд���������ݣ����������ȡ
                    //-4��ԭ������Ϊ ǰ��4�������ù��� ������������Ҫ��¼����������
                    bw.Write(table.Dimension.End.Row - BEGIN_INDEX);

                    //�жϼ̳У�����
                    int Inherit = -1;
                    if (GetInherit(table) != -1)
                        Inherit = GetInherit(table);
                    //2���洢�����ı�����
                    string keyName = GetVariableNameRow(table)[GetKeyIndex(table)].ToString();
                    byte[] bytes = Encoding.UTF8.GetBytes(keyName);
                    //д�������ı�����
                    bw.Write(keyName);
                    //�����������ݵ��� ���ж����Ƶ�д��
                    string[] row;
                    //�õ������У������������������д������
                    string[] rowType = GetVariableTypeRow(table);
                    for (int i = BEGIN_INDEX + 1; i <= table.Dimension.End.Row; i++)
                    {
                        //�õ�һ�е�����
                        row = table.GetExcelWorkSheetRow(i);
                        for (int j = 0; j < table.Dimension.End.Column; j++)
                        {
                            if (j == Inherit)//������ڼ̳�״̬�Ļ�
                            {
                                continue;
                            }
                            try
                            {
                                //�о���Ҫһ����չ�����ж��⸽�����͵�ת��������List<> ����ȵȵĸ���Ƕ�����͡�
                                if (BinaryConverAdditionMgr.GetContainsKey(rowType[j].ToString()))
                                {
                                    BinaryConverAdditionMgr.ConvertToBinary(bw, row[j].ToString(), rowType[j].ToString());
                                }
                                else if (BinaryConverMgr.BinaryConverter.ContainsKey(rowType[j].ToString()))
                                {
                                    BinaryConverMgr.BinaryConverter[rowType[j].ToString()].ConvertToBinary(bw, row[j].ToString());
                                }
                                //���û�ж�Ӧ��ת��������Ĭ��Ϊö������ - Ҳ������ν��int   
                                else
                                {
                                    fs.Write(BitConverter.GetBytes(int.Parse(row[j].ToString())), 0, 4);
                                }
                            }
                            catch (Exception e)
                            {
                                Debug.LogError("������" + table.Name + "������" + rowType[j].ToString() + "��������" + row[j].ToString() + "������" + i.ToString() + "������" + j.ToString() + "��ֵ��Ϣ" + row[j].ToString() + "������Ϣ ��" + e.Message);
                            }
                        }
                    }
                }
            }
            AssetDatabase.Refresh();
        }
        #endregion
        #region �Զ��������ڼ��ص����ñ����������ű����м���
        public static void InsConfigure(List<ExcelWorksheet> tableConllection, string Path)
        {
            String LoadPath = Path;
            if (!Directory.Exists(LoadPath))
            {
                Directory.CreateDirectory(LoadPath);
            }
            //ʹ�ö�����������д��
            using (FileStream fs = new FileStream(LoadPath + AssetUtility.BinConName, FileMode.OpenOrCreate, FileAccess.Write))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    bw.Write(tableConllection.Count);
                    //��ʼд������ַ���
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