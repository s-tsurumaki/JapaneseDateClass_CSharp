using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapaneseDateClass.Test
{
    #region TestClass
    /// <summary>
    /// 
    /// </summary>
    public class TestItem
    {
        /// <summary>
        /// 変換されるべき文字列
        /// </summary>
        /// <remarks>
        /// 平成28年1月1日など
        /// </remarks>
        public string Answer { set; get; }

        /// <summary>
        /// 文字列の日付
        /// </summary>
        /// <remarks>
        /// 2015/01/01
        /// </remarks>
        public string ToDateString_Slash { set; get; }

        /// <summary>
        /// 文字列の日付
        /// </summary>
        /// <remarks>
        /// 2015.01.01
        /// </remarks>
        public string ToDateString_Dot { set; get; }

        /// <summary>
        /// 年度のアルファベット
        /// </summary>
        /// <remarks>
        /// H19.01.01
        /// </remarks>
        public string ToCharAlphabet_Dot { set; get; }

        /// <summary>
        /// 年度のアルファベット
        /// </summary>
        /// <remarks>
        /// H19/01/01
        /// </remarks>
        public string ToCharAlphabet_Slash { set; get; }

        /// <summary>
        /// 数字7文字の日付
        /// </summary>
        /// <remarks>
        /// 4280101
        /// </remarks>
        public int ToInt { set; get; }
    }

    /// <summary>
    /// ArrayExtension
    /// </summary>
    public static class ArrayExtension
    {

        public static void ForEach<T>(this T[] TestItems, Action<T> action)
        {
            Array.ForEach(TestItems, action);
        }
    }
    #endregion

    public class TestUnitItem
    {
        #region TestItemUnit
        /// <summary>
        /// 年度の境界線をチェックします。
        /// </summary>
        /// <remarks>
        /// </remarks>
        List<TestItem> TestUnit_NendoLimit = new List<TestItem>
        {
            new TestItem { Answer= "明治元年1月25日" , ToDateString_Slash = "1868/01/25" , ToDateString_Dot = "1868.01.25" , ToCharAlphabet_Dot = "M1.01.25" , ToCharAlphabet_Slash = "M1/01/25" , ToInt = 18680125 }
           ,new TestItem { Answer= "19120729" , ToDateString_Slash = "1912/07/29" , ToDateString_Dot = "1912.07.29" , ToCharAlphabet_Dot = "M1.07.29" , ToCharAlphabet_Slash = "M1/07/29" , ToInt = 19120729 }
           ,new TestItem { Answer= "大正元年7月30日" , ToDateString_Slash = "1912/07/30" , ToDateString_Dot = "1912.07.30" , ToCharAlphabet_Dot = "T1.07.30" , ToCharAlphabet_Slash = "T1/07/30" , ToInt = 19120730 }
           ,new TestItem { Answer= "19261224" , ToDateString_Slash = "1926/12/24" , ToDateString_Dot = "1926.12.24" , ToCharAlphabet_Dot = "T1.12.24" , ToCharAlphabet_Slash = "T1/12/24" , ToInt = 19261224 }
           ,new TestItem { Answer= "19261225" , ToDateString_Slash = "1926/12/25" , ToDateString_Dot = "1926.12.25" , ToCharAlphabet_Dot = "S1.12.25" , ToCharAlphabet_Slash = "S1/12/25" , ToInt = 19261225 }
           ,new TestItem { Answer= "19890107" , ToDateString_Slash = "1989/01/07" , ToDateString_Dot = "1989.01.07" , ToCharAlphabet_Dot = "S1.01.07" , ToCharAlphabet_Slash = "S1/01/07" , ToInt = 19890107 }
           ,new TestItem { Answer= "19890108" , ToDateString_Slash = "1989/01/08" , ToDateString_Dot = "1989.01.08" , ToCharAlphabet_Dot = "H1.01.08" , ToCharAlphabet_Slash = "H1/01/08" , ToInt = 19890108 }
        };




        #endregion

    }


}
