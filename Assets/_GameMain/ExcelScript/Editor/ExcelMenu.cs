using OfficeOpenXml;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace B_Star
{
    public static class ExcelMenu
    {
        [MenuItem("B_Star/Excel/GenerateExcelInfo")]
        private static void GenerateExcelInfo()
        {
            //�ж�ɾ���洢·���µ��ļ� ��Ϊ�ļ��Զ��洢��������� -- ���·�����ڣ���ֱ��ɾ��
            if (Directory.Exists(AssetUtility.DataTableNewScriptPath))
            {
                //ǿ��ɾ���ļ����Լ����������ļ�
                Directory.Delete(AssetUtility.DataTableNewScriptPath, true);
            }
            //����ָ��·���е�����Excel�ļ������ɶ�Ӧ���ļ�
            DirectoryInfo dInfo = Directory.CreateDirectory(AssetUtility.DataTablePath);
            FileInfo[] file = dInfo.GetFiles();//��ȡ��·���µ������ļ�
            List<ExcelWorksheet> inscons = new List<ExcelWorksheet>();
            for (int i = 0; i < file.Length; i++)
            {
                if (file[i].Extension != ".xlsx") continue;
                //��ȡExcel�ļ�
                using (ExcelPackage excelpackage = new ExcelPackage(file[i]))
                {
                    ExcelWorksheets ExcelSheet = excelpackage.Workbook.Worksheets;
                    foreach (ExcelWorksheet sheet in ExcelSheet)
                    {
                        inscons.Add(sheet);
                        ExcelTool.GenerateExcelDataClass(sheet);
                        ExcelTool.GenerateExcelContainer(sheet);
                        ExcelTool.GenerateExcelBinary(sheet, AssetUtility.DataTableNewBinaryData_Path);
                    }
                }
            }
            //�������ñ���Ϣ - ����ͳһ����
            ExcelTool.InsConfigure(inscons, AssetUtility.DataTableNewBinaryData_Path);
        }
    }
}