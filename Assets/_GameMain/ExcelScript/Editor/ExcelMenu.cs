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
            //判断删除存储路径下的文件 因为文件自动存储会产生问题 -- 如果路径存在，则直接删除
            if (Directory.Exists(AssetUtility.DataTableNewScriptPath))
            {
                //强制删除文件夹以及其中所有文件
                Directory.Delete(AssetUtility.DataTableNewScriptPath, true);
            }
            //加载指定路径中的所有Excel文件，生成对应的文件
            DirectoryInfo dInfo = Directory.CreateDirectory(AssetUtility.DataTablePath);
            FileInfo[] file = dInfo.GetFiles();//获取该路径下的所有文件
            List<ExcelWorksheet> inscons = new List<ExcelWorksheet>();
            for (int i = 0; i < file.Length; i++)
            {
                if (file[i].Extension != ".xlsx") continue;
                //读取Excel文件
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
            //生成配置表信息 - 用于统一加载
            ExcelTool.InsConfigure(inscons, AssetUtility.DataTableNewBinaryData_Path);
        }
    }
}