using System.Collections.Generic;
namespace B_Star
{
    /// <summary>
    /// ��������ת������ת��int��bool ��float �Ȼ�������
    /// </summary>
	public static class BinaryConverMgr
	{
        #region ת�������
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
            //��ӽ���ת�����ֵ���
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
