using System.Collections.Generic;
namespace B_Star
{
    /// <summary>
    /// 基本数据转换器，转换int，bool ，float 等基本类型
    /// </summary>
	public static class BinaryConverMgr
	{
        #region 转换器添加
        public static Dictionary<string, IBinaryConverter> BinaryConverter = new Dictionary<string, IBinaryConverter>();
        private static bool IsConverter = false;

         static BinaryConverMgr()
        {
            AddAllBinConver();
        }

        private static void AddAllBinConver()
        {
            if (IsConverter) return;
            AddBinConverTer(new Binary_Bool());
            AddBinConverTer(new Binary_Float());
            AddBinConverTer(new Binary_Int());
            AddBinConverTer(new Binary_String());
            AddBinConverTer(new Binary_Vector2());
            AddBinConverTer(new Binary_Vector3());
            AddBinConverTer(new Binary_Vector4());
            
            IsConverter = true;
        }
        private static void AddBinConverTer(IBinaryConverter converter)
        {
            //添加进入转换器字典中
            foreach (string name in converter.PropertyNames)
            {
                if (BinaryConverter.ContainsKey(name))
                {
                    continue;
                }
                BinaryConverter.Add(name, converter);
            }
        }
        #endregion
    }
}
