using UnityEngine;

namespace B_Star
{
    public static class AssetUtility
    {
        /// <summary>
        /// 数据表加载路径
        /// </summary>
        public static string DataTablePath = "Assets/GameData/DataTables/";
        /// <summary>
        /// 生成脚本数据路径
        /// </summary>
        public static string DataTableNewScriptPath = "Assets/GameData/DataTableNewScripts/";
        /// <summary>
        /// 具体生成脚本路径
        /// </summary>
        public static string DataTableNewScript_Class_Path = "Assets/GameData/DataTableNewScripts/DataClass/";
        /// <summary>
        /// 生成的附属脚本路径
        /// </summary>
        public static string DataTableNewScript_Container_Path = "Assets/GameData/DataTableNewScripts/DataContainer/";
        /// <summary>
        /// 生成的二进制数据路径
        /// </summary>
        public static string DataTableNewBinaryData_Path = "Assets/GameData/DataTableNewScripts/BinaryData/Resources/";
        /// <summary>
        /// 二进制数据管理器名称
        /// </summary>
        public static string BinConName = "Configure.txt";
        /// <summary>
        /// 获取基本存档路径
        /// </summary>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public static string GetSaveDataPath(string assetName)
        {
            return Application.persistentDataPath + "/B_Star/Save/" + assetName;
        }
        /// <summary>
        /// 获取基本设置路径
        /// </summary>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public static string GetSaveSettingPath(string assetName)
        {
            return Application.persistentDataPath + "/B_Star/Setting/" + assetName;
        }
    }
}
