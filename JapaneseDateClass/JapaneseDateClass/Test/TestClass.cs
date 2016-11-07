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
        public int To7Int { set; get; }

        /// <summary>
        /// 数字8文字の日付
        /// </summary>
        /// <remarks>
        /// 20160101
        /// </remarks>
        public int To8Int { set; get; }

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
        public List<TestItem> TestUnit_NendoLimit = new List<TestItem>
        {
            //new TestItem { Answer= "1868年1月24日"    , ToDateString_Slash = "1868/01/24" , ToDateString_Dot = "1868.01.24" , ToCharAlphabet_Dot = "1868.01.24" , ToCharAlphabet_Slash = "M1/01/24" , ToInt = 18680124 }
            new TestItem { Answer= "明治元年1月25日"  , ToDateString_Slash = "1868/01/25" , ToDateString_Dot = "1868.01.25" , ToCharAlphabet_Dot = "M1.01.25"  , ToCharAlphabet_Slash = "M1/01/25"  , To7Int = 18680125 , To8Int = 18680125 }
           ,new TestItem { Answer= "明治45年7月29日"  , ToDateString_Slash = "1912/07/29" , ToDateString_Dot = "1912.07.29" , ToCharAlphabet_Dot = "M45.07.29" , ToCharAlphabet_Slash = "M45/07/29" , To7Int = 19120729 , To8Int = 19120729 }
           ,new TestItem { Answer= "大正元年7月30日"  , ToDateString_Slash = "1912/07/30" , ToDateString_Dot = "1912.07.30" , ToCharAlphabet_Dot = "T1.07.30"  , ToCharAlphabet_Slash = "T1/07/30"  , To7Int = 19120730 , To8Int = 19120730 }
           ,new TestItem { Answer= "大正15年12月24日" , ToDateString_Slash = "1926/12/24" , ToDateString_Dot = "1926.12.24" , ToCharAlphabet_Dot = "T15.12.24" , ToCharAlphabet_Slash = "T15/12/24" , To7Int = 19261224 , To8Int = 19261224 }
           ,new TestItem { Answer= "昭和元年12月25日" , ToDateString_Slash = "1926/12/25" , ToDateString_Dot = "1926.12.25" , ToCharAlphabet_Dot = "S1.12.25"  , ToCharAlphabet_Slash = "S1/12/25"  , To7Int = 19261225 , To8Int = 19261225 }
           ,new TestItem { Answer= "昭和64年1月7日"   , ToDateString_Slash = "1989/01/07" , ToDateString_Dot = "1989.01.07" , ToCharAlphabet_Dot = "S64.01.07" , ToCharAlphabet_Slash = "S64/01/07" , To7Int = 19890107 , To8Int = 19890107 }
           ,new TestItem { Answer= "平成元年1月8日"   , ToDateString_Slash = "1989/01/08" , ToDateString_Dot = "1989.01.08" , ToCharAlphabet_Dot = "H1.01.08"  , ToCharAlphabet_Slash = "H1/01/08"  , To7Int = 19890108 , To8Int = 19890108 }
        };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ans"></param>
        /// <param name="conv"></param>
        public void ConsoleWriteLineComparisonData(string baseitem ,string ans ,string conv)
        {
            if (ans == conv)
            {
                Console.WriteLine("Success:{0} => {1}", baseitem, conv);
            }
            else
            {
                Console.WriteLine("ErrorDate");
                Console.WriteLine("Base  :{0}", baseitem);
                Console.WriteLine("Answer:{0}", ans);
                Console.WriteLine("JpDate:{0}", conv);
            }
        }


        #endregion
    }


}
