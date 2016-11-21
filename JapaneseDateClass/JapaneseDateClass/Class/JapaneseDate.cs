using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace JapaneseDateClass.Class
{
    /// <summary>
    /// 値から日付型に変換します。
    /// </summary>
    /// <remarks>
    /// 命名規則的なもの
    /// Era：日本の元号
    /// Nendo：年度の概念
    /// </remarks>
    public class JapaneseDate
    {
        // 定数
        #region 定数

        // 元号の設定情報です。
        #region 元号の設定情報
        /// <summary>
        /// 元号の開始年月日です
        /// </summary>
        public static readonly ReadOnlyCollection<int> GengoStartYear = Array.AsReadOnly(new int[] { 0, 18680125, 19120730, 19261225, 19890108 });
        /// <summary>
        /// 元号の終了年月日です
        /// </summary>
        public static readonly ReadOnlyCollection<int> GengoEndYear = Array.AsReadOnly(new int[] { 0, 19120729, 19261224, 19890107, 99999999 });
        /// <summary>
        /// 元号の年度が開始した西暦です。
        /// </summary>
        public static readonly ReadOnlyCollection<int> GengoStartNendo = Array.AsReadOnly(new int[] { 0, 1868, 1912, 1926, 1989 });
        /// <summary>
        /// 元号の文字を1文字で表します。
        /// </summary>
        public static readonly ReadOnlyCollection<string> GengoCharAlphabet = Array.AsReadOnly(new string[] { "", "M", "T", "S", "H" });
        /// <summary>
        /// 元号の文字を数字1文字で表します。
        /// </summary>
        public static readonly ReadOnlyCollection<string> GengoCharNum = Array.AsReadOnly(new string[] { "0", "1", "2", "3", "4" });
        /// <summary>
        /// 元号を名称で表します。
        /// </summary>
        public static readonly ReadOnlyCollection<string> GengoName = Array.AsReadOnly(new string[] { "西暦", "明治", "大正", "昭和", "平成" });
        /// <summary>
        /// 元号を略称で表します。
        /// </summary>
        public static readonly ReadOnlyCollection<string> GengoAbbreviation = Array.AsReadOnly(new string[] { "西暦", "明", "大", "昭", "平" });
        /// <summary>
        /// 元号の正規表現です。
        /// </summary>
        public static readonly string NendoRegex = "(M|1|明治|明|T|2|大正|大|S|3|昭和|昭|H|4|平成|平)[0-9]{1,2}(年|/|.)[0-1]?[1-9]{1}(月|/|.)[0-3]?[1-9]{1}(日)?";
        /// <summary>
        /// 元号の正規表現ベースです。
        /// </summary>
        public static readonly string NendoRegexBase = @"(【元号】)[0-9]{1,2}(年|/|.)[0-1]?[1-9]{1}(月|/|.)[0-3]?[1-9]{1}(日)?";
        #endregion

        // データベースのDateTime型の値です。
        // このクラスはデータベースにデータを格納する事を想定しています。
        // 上限値、下限値の値はDateTime型が格納できる範囲としています。
        // DateTime型はグレゴリオ暦なので下限値を1753年1月1日としています。
        #region DataBaseDateTime (データベースの最大、最小値)
        /// <summary>
        /// データベースを扱う日付の最小値です。
        /// </summary>
        /// <remarks>T-SQL DateTimeのDateTimeの最小値です。</remarks>
        public static int MinValueDateToDataBaseInt = 17530101;
        /// <summary>
        /// データベースを扱う日付の年の最小値です。
        /// </summary>
        /// <remarks>T-SQL DateTimeのDateTimeの最小値です。</remarks>
        public static int MinValueYearToDataBaseInt = 1753;
        /// <summary>
        /// データベースを扱う日付の月の最小値です。
        /// </summary>
        /// <remarks>T-SQL DateTimeのDateTimeの最小値です。</remarks>
        public static int MinValueMonthToDataBaseInt = 1;
        /// <summary>
        /// データベースを扱う日付の日の最小値です。
        /// </summary>
        /// <remarks>T-SQL DateTimeのDateTimeの最小値です。</remarks>
        public static int MinValueDayToDataBaseInt = 1;
        /// <summary>
        /// データベースを扱う日付の最大値です。
        /// </summary>
        public static int MaxValueDateToDataBaseInt = 99991231;
        /// <summary>
        /// データベースを扱う日付の年の最大値です。
        /// </summary>
        public static int MaxValueYearToDataBaseInt = 9999;
        /// <summary>
        /// データベースを扱う日付の月の最大値です。
        /// </summary>
        public static int MaxValueMonthToDataBaseInt = 12;
        /// <summary>
        /// データベースを扱う日付の日の最大値です。
        /// </summary>
        public static int MaxValueDayToDataBaseInt = 31;
        /// <summary>
        /// データベースを扱う日付の最小値です。
        /// </summary>
        public static DateTime MinValueToDataBaseDate = new DateTime(1753, 1, 1);
        /// <summary>
        /// データベースを扱う日付の最大値です。
        /// </summary>
        public static DateTime MaxValueToDataBaseDate = new DateTime(9999, 12, 31);
        #endregion

        // メソッドで利用している定数
        #region メソッドで利用している定数
        // GetJapaneseDateToStringで利用する出力種別です。
        #region GengoOutputType (GetJapaneseDateToStringで利用する元号の出力種別です。)
        /// <summary>
        /// 元号の出力種別です。
        /// </summary>
        public enum GengoOutputType
        {
            /// <summary>
            /// 元号を出力しません。
            /// </summary>
            None,
            /// <summary>
            /// 元号を正式名称で表します。
            /// </summary>
            WarekiName,
            /// <summary>
            /// 元号をアルファベット1文字で表します。
            /// </summary>
            WarekiAlphabet,
            /// <summary>
            /// 元号を略称で表します。
            /// </summary>
            WarekiAbbreviation,
            /// <summary>
            /// 元号をすべて西暦と表記します。
            /// </summary>
            Seireki,
        }
        #endregion
        #region SplitOutputType (GetJapaneseDateToStringで利用する区切り文字種類)
        /// <summary>
        /// 区切り文字種類
        /// </summary>
        public enum SplitOutputType
        {
            /// <summary>
            /// なし
            /// </summary>
            None,
            /// <summary>
            ///　元号99年99月99日
            /// </summary>
            Kanji,
            /// <summary>
            ///　元号99/99/99
            /// </summary>
            Slash,
            /// <summary>
            ///　元号99.99.99
            /// </summary>
            Dot,
        }
        #endregion
        #region YearOutputType (年を表示するか指定します。)
        /// <summary>
        /// 年を表示するか指定します。
        /// </summary>
        public enum YearOutputType
        {
            /// <summary>
            /// 値を表示します。
            /// </summary>
            Display,
            /// <summary>
            /// 値を非表示にします。
            /// </summary>
            Hide,
        }
        #endregion
        #region MonthOutputType (月を表示するか指定します。)
        /// <summary>
        /// 月を表示するか指定します。
        /// </summary>
        public enum MonthOutputType
        {
            /// <summary>
            /// 値を表示します。
            /// </summary>
            Display,
            /// <summary>
            /// 値を非表示にします。
            /// </summary>
            Hide,
        }
        #endregion
        #region DayOutputType (日を表示するか指定します。)
        /// <summary>
        /// 日を表示するか指定します。
        /// </summary>
        public enum DayOutputType
        {
            /// <summary>
            /// 値を表示します。
            /// </summary>
            Display,
            /// <summary>
            /// 値を非表示にします。
            /// </summary>
            Hide,
        }
        #endregion
        #region NendoConv (出力する日付文字列を年度として出力するか指定します。)
        /// <summary>
        /// 出力する日付文字列を年度として出力するか指定します。
        /// </summary>
        public enum NendoConv
        {
            /// <summary>
            /// 和暦として出力します。
            /// </summary>
            No,
            /// <summary>
            /// 年度として出力します。
            /// </summary>
            Yes,
        }
        #endregion
        #region MonthDayZeroPadding (月と日の2桁目の文字をゼロ埋めするか指定します。)
        /// <summary>
        /// 月と日の2桁目の文字をゼロ埋めするか指定します。
        /// </summary>
        public enum MonthDayZeroPadding
        {
            /// <summary>
            /// ゼロ埋めしません。
            /// </summary>
            No,
            /// <summary>
            /// ゼロ埋めします。
            /// </summary>
            Yes,
        }
        #endregion
        #region GanNenType (元年(元年度)の表記方法を指定します。)
        /// <summary>
        /// 元年(元年度)の表記方法を指定します。
        /// </summary>
        public enum GanNenType
        {
            /// <summary>
            /// 元年と表記します。
            /// </summary>
            Kanji,
            /// <summary>
            /// 文字列「1」を出力します。
            /// </summary>
            Num,
            /// <summary>
            /// 文字列「01」を出力します。
            /// </summary>
            NumZeroPadding,
        }
        #endregion

        // GetGengoStateで利用する出力種別です。
        #region Gengo (元号)
        /// <summary>
        /// 元号
        /// </summary>
        public enum Gengo
        {
            /// <summary>
            /// 初期値
            /// </summary>
            None = 0,
            /// <summary>
            /// 最小値
            /// </summary>
            Min = 1,
            /// <summary>
            /// 明治
            /// </summary>
            Meiji = 1,
            /// <summary>
            /// 大正
            /// </summary>
            Taisho = 2,
            /// <summary>
            /// 昭和
            /// </summary>
            Syouwa = 3,
            /// <summary>
            /// 平成
            /// </summary>
            Heisei = 4,
            /// <summary>
            /// 最大値
            /// </summary>
            Max = 4,
        }
        #endregion
        #region DataType (データ取得タイプです。)
        /// <summary>
        /// データ取得タイプです。
        /// </summary>
        public enum DataType
        {
            /// <summary>
            /// 元号の開始年月日です。
            /// </summary>
            GengoStartYear,
            /// <summary>
            /// 元号の終了年月日です。
            /// </summary>
            GengoEndYear,
            /// <summary>
            /// 元号の年度が開始した西暦です。
            /// </summary>
            GengoStartNendo,
            /// <summary>
            /// 元号の文字を1文字で表します。
            /// </summary>
            GengoCharAlphabet,
            /// <summary>
            /// 元号の文字を数字1文字で表します。
            /// </summary>
            GengoCharNum,
            /// <summary>
            /// 元号を名称で表します。
            /// </summary>
            GengoName,
            /// <summary>
            /// 元号を略称で表します。
            /// </summary>
            GengoAbbreviation,
        }
        #endregion
        #endregion

        // 
        #region DateStatus (入力された日付変換)
        /// <summary>
        /// 日付変換で返す返却値です。
        /// </summary>
        public enum DateStatus
        {
            /// <summary>
            /// 初期値
            /// </summary>
            None,
            /// <summary>
            /// 変換に成功しました
            /// </summary>
            Success,
            /// <summary>
            /// 正規表現にマッチしないデータです。
            /// </summary>
            RegexIsMatchError,
            /// <summary>
            /// 変換不可能な日付です。
            /// </summary>
            ConversionImpossible,
            /// <summary>
            /// 元号と西暦の範囲が異常です。
            /// </summary>
            Error_NendoRenge,
            /// <summary>
            /// 不明なエラー
            /// </summary>
            Error_Fatal,
        }
        #endregion

        // 和暦変換種別です。
        #region WarekiFormat (和暦変換種別です。)
        /// <summary>
        /// 和暦変換種別です。
        /// </summary>
        public enum WarekiFormat
        {
            None,
            /// <summary>
            /// yyyy年M月d日
            /// </summary>
            Seireki_JP,
            /// <summary>
            /// yyyy年MM月dd日
            /// </summary>
            Seireki_Zero_JP,
            /// <summary>
            /// 元号ee年M月d日
            /// </summary>
            Wareki_JP,
            /// <summary>
            /// 元号ee年MM月dd日
            /// </summary>
            Wareki_Zero_JP,
            /// <summary>
            /// 略称された元号ee年M月d日
            /// </summary>
            Wareki_Abbreviation_JP,
            /// <summary>
            /// 略称された元号ee年MM月dd日
            /// </summary>
            Wareki_Abbreviation_Zero_JP,
            /// <summary>
            /// アルファベットの元号ee年M月d日
            /// </summary>
            Wareki_Alphabet_JP,
            /// <summary>
            /// アルファベットの元号ee年MM月dd日
            /// </summary>
            Wareki_Alphabet_Zero_JP,
            /// <summary>
            /// yyyy.M.d
            /// </summary>
            Seireki_Dot,
            /// <summary>
            /// yyyy.MM.dd
            /// </summary>
            Seireki_Zero_Dot,
            /// <summary>
            /// yyyy/M/d
            /// </summary>
            Seireki_Slash,
            /// <summary>
            /// yyyy/MM/dd
            /// </summary>
            Seireki_Zero_Slash,
        }
        #endregion

        #endregion

        #region フィールド
        /// <summary>
        /// 日付
        /// </summary>
        /// <remarks>
        /// 入力した値が日付として変換できた場合、有効な値が入ります。
        /// </remarks>
        public DateTime InputDate { get; private set; }
        /// <summary>
        /// 元号
        /// </summary>
        /// <remarks>
        /// 入力した値が日付として変換できた場合、有効な値が入ります。
        /// また、元号の範囲外の場合は初期値が入ります。
        /// </remarks>
        public Gengo TargetGengo { get; private set; }
        /// <summary>
        /// 年度
        /// </summary>
        /// <remarks>
        /// 年度の概念は4月1日から翌年の3月31日までの範囲を年度とします。<br>
        /// 登録した日付が平成28年1月2日の場合、27を返します。
        /// </remarks>
        public int Nendo { get; private set; }
        /// <summary>
        /// 西暦
        /// </summary>
        /// <remarks>
        /// 登録した日付が平成28年1月2日の場合、2017を返します。
        /// </remarks>
        public int Year { get; private set; }
        /// <summary>
        /// 和暦の年です。
        /// </summary>
        /// <remarks>
        /// 登録した日付が平成28年1月1日の場合、JapaneseEraYearは28を返します。
        /// </remarks>
        public int EraYear { get; private set; }
        /// <summary>
        /// 月
        /// </summary>
        /// <remarks>
        /// 登録した日付が平成28年1月2日の場合、1を返します。
        /// </remarks>
        public int Month { get; private set; }
        /// <summary>
        /// 日
        /// </summary>
        /// <remarks>
        /// 登録した日付が平成28年1月2日の場合、2を返します。
        /// </remarks>
        public int Day { get; private set; }
        /// <summary>
        /// 元号を返します。
        /// </summary>
        /// <remarks>
        /// 明治、大正、昭和、平成
        /// </remarks>
        public string Era { get; private set; }
        /// <summary>
        /// 元号を返します。
        /// </summary>
        /// <remarks>
        /// M、T、S、H
        /// </remarks>
        public string EraAlphabet { get; private set; }
        /// <summary>
        /// 元号の略称です。
        /// </summary>
        /// <remarks>
        /// 明、大、昭、平
        /// </remarks>
        public string EraAbbreviation { get; private set; }
        /// <summary>
        /// 入力した日付文字列が日付として変換できたか示す値です。
        /// </summary>
        public DateStatus ConversionStatus { get; private set; }
        /// <summary>
        /// JapaneseDateで表現する元号の種類です。 
        /// </summary>
        private WarekiFormat WarekiFormatType { get; set; }

        #endregion
        
        #region Publicメソッド

        // コンストラクタ
        #region コンストラクタ
        /// <summary>
        /// 日付文字列のコンストラクタです。
        /// </summary>
        /// <param name="data">日付の文字列です</param>
        public JapaneseDate(string data)
        {
            this.ConversionStatus = this.SetData(data);
        }

        /// <summary>
        /// 日付のコンストラクタです。
        /// </summary>
        /// <param name="data">日付</param>
        public JapaneseDate(DateTime data)
        {
            this.ConversionStatus = this.SetData(data);
        }

        /// <summary>
        /// 数値のコンストラクタです。
        /// </summary>
        /// <param name="data">日付数値</param>
        public JapaneseDate(int data)
        {
            this.ConversionStatus = this.SetData(data);
        }
        #endregion

        // 日付文字列から日付型に変換します。
        #region SetData (日付となる値をセットします。)
        // 日付文字入力
        #region SetData DateTime (日付となる値をセットします。)
        /// <summary>
        /// 日付となる値をセットします。
        /// </summary>
        /// <param name="convData">変換対象となる日付データ</param>
        /// <returns>変換結果ステータス</returns>
        public DateStatus SetData(DateTime convData)
        {
            this.ConversionStatus = this.SetDataAssort(convData.ToString());
            return this.ConversionStatus;
        }
        #endregion
        #region SetData int (日付となる値をセットします。)
        /// <summary>
        /// 日付となる値をセットします。
        /// </summary>
        /// <param name="convData">変換対象となる日付データ</param>
        /// <returns>変換結果ステータス</returns>
        public DateStatus SetData(int convData)
        {
            this.ConversionStatus = this.SetDataAssort(convData.ToString());
            return this.ConversionStatus;
        }
        #endregion
        #region SetData string (日付となる値をセットします。)
        /// <summary>
        /// 日付となる値をセットします。
        /// </summary>
        /// <param name="convData">変換対象となる日付データ</param>
        /// <returns>変換結果ステータス</returns>
        public DateStatus SetData(string convData)
        {
            this.ConversionStatus = this.SetDataAssort(convData.ToString());
            return this.ConversionStatus;
        }
        #endregion
        // 日付文字入力(デフォルト)
        #region SetDataDateTimeNow (DateTime.Nowで取得した値をセットします。)
        /// <summary>
        /// DateTime.Nowで取得した値をセットします。
        /// </summary>
        /// <returns>変換結果ステータス</returns>
        public DateStatus SetDataDateTimeNow()
        {
            this.ConversionStatus = this.SetDataAssort(DateTime.Now.ToString());
            return this.ConversionStatus;
        }
        #endregion
        #region SetDataDateMin (データベースを扱う日付の最小値をセットします。)
        /// <summary>
        /// データベースを扱う日付の最小値をセットします。
        /// </summary>
        /// <returns>変換結果ステータス</returns>
        public DateStatus SetDataDateMin()
        {
            this.ConversionStatus = this.SetDataAssort(MinValueToDataBaseDate.ToString());
            return this.ConversionStatus;
        }
        #endregion
        #region SetDataDateMax (データベースを扱う日付の最大値をセットします。)
        /// <summary>
        /// データベースを扱う日付の最大値をセットします。
        /// </summary>
        /// <returns>変換結果ステータス</returns>
        public DateStatus SetDataDateMax()
        {
            this.ConversionStatus = this.SetDataAssort(MaxValueToDataBaseDate.ToString());
            return this.ConversionStatus;
        }
        #endregion
        #endregion

        // 和暦文字列を返します。
        #region ToEraString (このインスタンスで表される日付の日コンポーネントを取得します。)
        /// <summary>
        /// このインスタンスで表される日付の日コンポーネントを取得します。
        /// </summary>
        /// <returns>日</returns>
        [Description("このインスタンスで表される日付の日コンポーネントを取得します。")]
        public string ToEraString
        {
            get
            {
                string ret;

                switch (WarekiFormatType)
                {
                    case WarekiFormat.None:
                        ret = this.GetJapaneseDateToString(GengoOutputType.None,
                                                           SplitOutputType.None,
                                                           YearOutputType.Display,MonthOutputType.Display,DayOutputType.Display,
                                                           NendoConv.No,
                                                           GanNenType.Kanji,
                                                           MonthDayZeroPadding.No);
                        break;
                    case WarekiFormat.Seireki_JP:
                        ret = this.GetJapaneseDateToString(GengoOutputType.Seireki,
                                                           SplitOutputType.Kanji,
                                                           YearOutputType.Display, MonthOutputType.Display, DayOutputType.Display,
                                                           NendoConv.No,
                                                           GanNenType.Kanji,
                                                           MonthDayZeroPadding.No);
                        break;
                    case WarekiFormat.Seireki_Zero_JP:
                        ret = this.GetJapaneseDateToString(GengoOutputType.Seireki,
                                                           SplitOutputType.Kanji,
                                                           YearOutputType.Display, MonthOutputType.Display,DayOutputType.Display,
                                                           NendoConv.No,
                                                           GanNenType.Kanji,
                                                           MonthDayZeroPadding.Yes);
                        break;
                    case WarekiFormat.Wareki_JP:
                        ret = this.GetJapaneseDateToString(GengoOutputType.WarekiName,
                                                           SplitOutputType.Kanji,
                                                           YearOutputType.Display, MonthOutputType.Display, DayOutputType.Display,
                                                           NendoConv.No,
                                                           GanNenType.Kanji,
                                                           MonthDayZeroPadding.No);
                        break;
                    case WarekiFormat.Wareki_Zero_JP:
                        ret = this.GetJapaneseDateToString(GengoOutputType.WarekiName,
                                                           SplitOutputType.Kanji,
                                                           YearOutputType.Display, MonthOutputType.Display, DayOutputType.Display,
                                                           NendoConv.No,
                                                           GanNenType.Kanji,
                                                           MonthDayZeroPadding.Yes);
                        break;
                    case WarekiFormat.Wareki_Abbreviation_JP:
                        ret = this.GetJapaneseDateToString(GengoOutputType.WarekiAbbreviation,
                                                           SplitOutputType.Kanji,
                                                           YearOutputType.Display, MonthOutputType.Display, DayOutputType.Display,
                                                           NendoConv.No,
                                                           GanNenType.Kanji,
                                                           MonthDayZeroPadding.No);
                        break;
                    case WarekiFormat.Wareki_Abbreviation_Zero_JP:
                        ret = this.GetJapaneseDateToString(GengoOutputType.WarekiAbbreviation,
                                                           SplitOutputType.Kanji,
                                                           YearOutputType.Display, MonthOutputType.Display, DayOutputType.Display,
                                                           NendoConv.No,
                                                           GanNenType.Kanji,
                                                           MonthDayZeroPadding.Yes);
                        break;
                    case WarekiFormat.Wareki_Alphabet_JP:
                        ret = this.GetJapaneseDateToString(GengoOutputType.WarekiAlphabet,
                                                           SplitOutputType.Kanji,
                                                           YearOutputType.Display, MonthOutputType.Display, DayOutputType.Display,
                                                           NendoConv.No,
                                                           GanNenType.Kanji,
                                                           MonthDayZeroPadding.No);
                        break;
                    case WarekiFormat.Wareki_Alphabet_Zero_JP:
                        ret = this.GetJapaneseDateToString(GengoOutputType.WarekiAlphabet,
                                                           SplitOutputType.Kanji,
                                                           YearOutputType.Display, MonthOutputType.Display, DayOutputType.Display,
                                                           NendoConv.No,
                                                           GanNenType.Kanji,
                                                           MonthDayZeroPadding.Yes);
                        break;
                    case WarekiFormat.Seireki_Dot:
                        ret = this.GetJapaneseDateToString(GengoOutputType.Seireki,
                                                           SplitOutputType.Dot,
                                                           YearOutputType.Display, MonthOutputType.Display, DayOutputType.Display,
                                                           NendoConv.No,
                                                           GanNenType.Kanji,
                                                           MonthDayZeroPadding.No);
                        break;
                    case WarekiFormat.Seireki_Zero_Dot:
                        ret = this.GetJapaneseDateToString(GengoOutputType.Seireki,
                                                           SplitOutputType.Dot,
                                                           YearOutputType.Display, MonthOutputType.Display, DayOutputType.Display,
                                                           NendoConv.No,
                                                           GanNenType.Kanji,
                                                           MonthDayZeroPadding.Yes);
                        break;
                    case WarekiFormat.Seireki_Slash:
                        ret = this.GetJapaneseDateToString(GengoOutputType.Seireki,
                                                           SplitOutputType.Slash,
                                                           YearOutputType.Display, MonthOutputType.Display, DayOutputType.Display,
                                                           NendoConv.No,
                                                           GanNenType.Kanji,
                                                           MonthDayZeroPadding.Yes);
                        break;
                    case WarekiFormat.Seireki_Zero_Slash:
                        ret = this.GetJapaneseDateToString(GengoOutputType.Seireki,
                                                           SplitOutputType.Slash,
                                                           YearOutputType.Display, MonthOutputType.Display, DayOutputType.Display,
                                                           NendoConv.No,
                                                           GanNenType.Kanji,
                                                           MonthDayZeroPadding.Yes);
                        break;
                    default:
                        ret = "";
                        break;
                }

                return ret;
            }
        }
        #endregion

        // 日付の演算、比較
        #region Add (このインスタンスの値に、指定された DateTime の値を加算した新しい TimeSpan を返します。)
        /// <summary>
        /// このインスタンスの値に、指定された DateTime の値を加算した新しい TimeSpan を返します。
        /// </summary>
        /// <param name="value">正または負の時間間隔。</param>
        /// <returns>このインスタンスで表された日付と時刻に value で表された時間間隔を加算した値を保持するオブジェクト。</returns>
        public DateTime Add(TimeSpan value)
        {
            switch (this.SetData(this.InputDate.Add(value)))
            {
                case DateStatus.None:
                    break;
                case DateStatus.Success:
                    break;
                case DateStatus.RegexIsMatchError:
                    break;
                case DateStatus.ConversionImpossible:
                    break;
                case DateStatus.Error_NendoRenge:
                    break;
                case DateStatus.Error_Fatal:
                    break;
                default:
                    break;
            }
            return this.InputDate;
        }
        #endregion
        #region AddDays(このインスタンスの値に、指定された日数を加算した新しい DateTime を返します。)
        /// <summary>
        /// このインスタンスの値に、指定された日数を加算した新しい DateTime を返します。
        /// </summary>
        /// <param name="value"></param>
        /// <returns>このインスタンスで表された日付と時刻に value で表された年数を加算した値を保持するオブジェクト。</returns>
        public DateTime AddDays(double value)
        {
            switch (this.SetData(this.InputDate.AddDays(value)))
            {
                case DateStatus.None:
                    break;
                case DateStatus.Success:
                    break;
                case DateStatus.RegexIsMatchError:
                    break;
                case DateStatus.ConversionImpossible:
                    break;
                case DateStatus.Error_NendoRenge:
                    break;
                case DateStatus.Error_Fatal:
                    break;
                default:
                    break;
            }
            return this.InputDate;
        }
        #endregion
        #region AddYears (このインスタンスの値に、指定された年数を加算した新しい DateTime を返します。)
        /// <summary>
        /// このインスタンスの値に、指定された年数を加算した新しい DateTime を返します。
        /// </summary>
        /// <param name="value">年数。 value パラメーターは、正または負のどちらの場合もあります。</param>
        /// <returns>このインスタンスで表された日付と時刻に value で表された年数を加算した値を保持するオブジェクト。</returns>
        public DateTime AddYears(int value)
        {
            switch (this.SetData(this.InputDate.AddYears(value)))
            {
                case DateStatus.None:
                    break;
                case DateStatus.Success:
                    break;
                case DateStatus.RegexIsMatchError:
                    break;
                case DateStatus.ConversionImpossible:
                    break;
                case DateStatus.Error_NendoRenge:
                    break;
                case DateStatus.Error_Fatal:
                    break;
                default:
                    break;
            }
            return this.InputDate;
        }
        #endregion
        #region AddMonths (このインスタンスの値に、指定された月数を加算した新しい DateTime を返します。)
        /// <summary>
        /// このインスタンスの値に、指定された月数を加算した新しい DateTime を返します。
        /// </summary>
        /// <param name="months">月数。 months パラメーターは、正または負のどちらの場合もあります。</param>
        /// <returns>このインスタンスで表された日付と時刻に months を加算した値を保持するオブジェクト。</returns>
        public DateTime AddMonths(int months)
        {
            switch (this.SetData(this.InputDate.AddMonths(months)))
            {
                case DateStatus.None:
                    break;
                case DateStatus.Success:
                    break;
                case DateStatus.RegexIsMatchError:
                    break;
                case DateStatus.ConversionImpossible:
                    break;
                case DateStatus.Error_NendoRenge:
                    break;
                case DateStatus.Error_Fatal:
                    break;
                default:
                    break;
            }
            return this.InputDate;
        }
        #endregion
        #region Compare (JapaneseDate の 2 つのインスタンスを比較し、第 1 のインスタンスが第 2 のインスタンスよりも前か、同じか、それとも後かを示す整数を返します。)
        /// <summary>
        /// JapaneseDate の 2 つのインスタンスを比較し、第 1 のインスタンスが第 2 のインスタンスよりも前か、同じか、それとも後かを示す整数を返します。
        /// </summary>
        /// <param name="t1">比較する最初のオブジェクト。</param>
        /// <param name="t2">比較する 2 番目のオブジェクト。</param>
        /// <returns>t1 と t2 の相対値を示す符号付き数値。値型状態0 より小さい値t1 が t2 よりも前の日時です。0t1 は t2 と同じです。0 を超える値t1 が t2 より後の日時です。</returns>
        public static int Compare(JapaneseDate t1, JapaneseDate t2)
        {
            return DateTime.Compare(t1.InputDate, t2.InputDate);
        }
        #endregion
        #region Compare (DateTime の 2 つのインスタンスを比較し、第 1 のインスタンスが第 2 のインスタンスよりも前か、同じか、それとも後かを示す整数を返します。)
        /// <summary>
        /// DateTime の 2 つのインスタンスを比較し、第 1 のインスタンスが第 2 のインスタンスよりも前か、同じか、それとも後かを示す整数を返します。
        /// </summary>
        /// <param name="t1">比較する最初のオブジェクト。</param>
        /// <param name="t2">比較する 2 番目のオブジェクト。</param>
        /// <returns>t1 と t2 の相対値を示す符号付き数値。値型状態0 より小さい値t1 が t2 よりも前の日時です。0t1 は t2 と同じです。0 を超える値t1 が t2 より後の日時です。</returns>
        public static int Compare(DateTime t1, DateTime t2)
        {
            return DateTime.Compare(t1, t2);
        }
        #endregion
        #region CompareTo (このインスタンスの値と指定した System.DateTime の値を比較し、このインスタンスの値が指定した System.DateTime の値よりも前か、同じか、または後かを示す整数を返します。)
        /// <summary>
        /// このインスタンスの値と指定した System.DateTime の値を比較し、このインスタンスの値が指定した System.DateTime の値よりも前か、同じか、または後かを示す整数を返します。
        /// </summary>
        /// <param name="value">現在のインスタンスと比較する対象のオブジェクト。</param>
        /// <returns>このインスタンスと value パラメーターの相対値を示す符号付き数値。値説明0 より小さい値このインスタンスは value より前の時刻を表しています。0このインスタンスは value と同じです。0 を超える値このインスタンスは value より後の時刻を表しています。</returns>
        public int CompareTo(JapaneseDate value)
        {
            return this.InputDate.CompareTo(value.InputDate);
        }
        #endregion
        #region CompareTo (このインスタンスの値と指定した System.DateTime の値を比較し、このインスタンスの値が指定した System.DateTime の値よりも前か、同じか、または後かを示す整数を返します。)
        /// <summary>
        /// このインスタンスの値と指定した System.DateTime の値を比較し、このインスタンスの値が指定した System.DateTime の値よりも前か、同じか、または後かを示す整数を返します。
        /// </summary>
        /// <param name="value">現在のインスタンスと比較する対象のオブジェクト。</param>
        /// <returns>このインスタンスと value パラメーターの相対値を示す符号付き数値。値説明0 より小さい値このインスタンスは value より前の時刻を表しています。0このインスタンスは value と同じです。0 を超える値このインスタンスは value より後の時刻を表しています。</returns>
        public int CompareTo(DateTime value)
        {
            return this.InputDate.CompareTo(value);
        }
        #endregion

        // このインスタンスの値の設定情を変更するメソッド
        #region ChangeWarekiFormat (和暦変換した時のフォーマットタイプを変更します。)
        /// <summary>
        /// 和暦変換した時のフォーマットタイプを変更します。
        /// </summary>
        public WarekiFormat ChangeWarekiFormat
        {
            set
            {
                this.WarekiFormatType = value;
            }
        }
        #endregion
        
        // このインスタンスの値の設定情報を取得するメソッド
        #region IsBetweenGengo (元号の範囲内か確認します。)
        /// <summary>
        /// 元号の範囲内か確認します。
        /// </summary>
        public bool IsBetweenGengo
        {
            get
            {
                return TargetGengo == Gengo.None ? false : true;
            }
        }
        #endregion

        // 日付 → 日付文字列変換
        #region GetJapaneseDateToString (入力した日付から西暦・和暦・年度に変換した文字列を返します。)
        /// <summary>
        /// 入力した日付から西暦・和暦・年度に変換した文字列を返します。
        /// </summary>
        /// <param name="gengoType">元号の種別です。</param>
        /// <param name="splitType">年月日を区切る文字です。</param>
        /// <param name="year">年を出力する判定フラグです。</param>
        /// <param name="month">月を出力する判定フラグです。</param>
        /// <param name="day">日を出力する判定フラグです。</param>
        /// <param name="nendo">年度に変換する判定フラグです。</param>
        /// <param name="GanNenTyoe">元年の表記種別です。</param>
        /// <param name="zeroPadding">月と日をゼロ埋めする判定フラグです。</param>
        /// <remarks>入力した日付がnullまたは入力前にコールした場合は空白を返します。</remarks>
        /// <returns>日付変換後の文字列</returns>
        public string GetJapaneseDateToString(GengoOutputType gengoType, SplitOutputType splitType, YearOutputType year, MonthOutputType month, DayOutputType day, NendoConv nendo , GanNenType ganNen, MonthDayZeroPadding zeroPadding)
        {
            string ret;

            ret = "";

            // 日付でない場合
            if (this.InputDate == null)
            {
                return ret;
            }

            // 年を出力する
            #region 年を出力する
            if (year == YearOutputType.Display)
            {
                if (gengoType == GengoOutputType.None) // 何も出力しない場合
                {
                    ret += this.Year.ToString();
                }
                else if (gengoType == GengoOutputType.Seireki) // 西暦の場合
                {
                    ret += "西暦" + this.Year.ToString();
                }
                else
                {
                    string y = "";

                    if (nendo == NendoConv.No) // 和暦表示
                    {
                        // 元号種別が西暦以外の場合は元年表記を取得する
                        switch (ganNen)
                        {
                            case GanNenType.Kanji:
                                y = this.EraYear.isBetween(0, 1) ? "元" : this.EraYear.ToString();
                                break;
                            case GanNenType.Num:
                                y = this.EraYear.isBetween(0, 1) ? "1" : this.EraYear.ToString();
                                break;
                            case GanNenType.NumZeroPadding:
                                y = this.EraYear.isBetween(0, 1) ? "01" : this.EraYear.ToString();
                                break;
                            default:
                                break;
                        }
                    }
                    else if (nendo == NendoConv.Yes) // 元年表記
                    {
                        // 元年表記+年度の値取得
                        switch (ganNen)
                        {
                            case GanNenType.Kanji:
                                y = this.Nendo.isBetween(0, 1) ? "元" : this.Nendo.ToString();
                                break;
                            case GanNenType.Num:
                                y = this.Nendo.isBetween(0, 1) ? "1" : this.Nendo.ToString();
                                break;
                            case GanNenType.NumZeroPadding:
                                y = this.Nendo.isBetween(0, 1) ? "01" : this.Nendo.ToString();
                                break;
                            default:
                                break;
                        }
                    }

                    // 元号を取得
                    switch (gengoType)
                    {
                        case GengoOutputType.WarekiName:
                            ret += this.Era + y;
                            break;
                        case GengoOutputType.WarekiAlphabet:
                            ret += this.EraAlphabet + y;
                            break;
                        case GengoOutputType.WarekiAbbreviation:
                            ret += this.EraAbbreviation + y;
                            break;
                        case GengoOutputType.None:
                        case GengoOutputType.Seireki:
                        default:
                            break;
                    }
                }

                // 区切り文字
                switch (splitType)
                {
                    case SplitOutputType.None:
                        break;
                    case SplitOutputType.Kanji:
                        if (nendo == NendoConv.Yes)
                        {
                            ret += "年度";
                        }
                        else if (nendo == NendoConv.No)
                        {
                            ret += "年";
                        }
                        break;
                    case SplitOutputType.Slash:
                        ret += "/";
                        break;
                    case SplitOutputType.Dot:
                        ret += ".";
                        break;
                    default:
                        break;
                }
            }
            #endregion

            // 月を出力する
            #region 月を出力する
            if (month == MonthOutputType.Display)
            {
                // 月をゼロ埋めにする場合
                ret += zeroPadding == MonthDayZeroPadding.Yes ? this.Month.ToString(): String.Format("{0:D2}", this.Month);

                // 区切り文字に対応した文字を取得
                switch (splitType)
                {
                    case SplitOutputType.None:
                        break;
                    case SplitOutputType.Kanji:
                        ret += "月";
                        break;
                    case SplitOutputType.Slash:
                        ret += "/";
                        break;
                    case SplitOutputType.Dot:
                        ret += ".";
                        break;
                }
            }
            #endregion

            // 日を出力する
            #region 日を出力する
            if (day == DayOutputType.Display)
            {
                // 日の値を代入
                // ※2桁目をゼロ埋めする場合
                ret += zeroPadding == MonthDayZeroPadding.Yes ? String.Format("{0:D2}", this.Day) : this.Day.ToString();

                // 区切り文字が漢字の場合、日のみ取得する。
                ret += splitType == SplitOutputType.Kanji ? "日" : "";
            }
            #endregion

            return ret;
        }
        #endregion

        // 年月日の数値型
        // Note:入力した文字列が日付に変換できるものだけ利用可能
        #region ToInt
        #region YearToInt (このインスタンスで表される日付の年コンポーネントを取得します。)
        /// <summary>
        /// このインスタンスで表される日付の年コンポーネントを取得します。
        /// </summary>
        /// <returns>年</returns>
        [Description("このインスタンスで表される日付の年コンポーネントを取得します。")]
        public int YearToInt
        {
            get
            {
                return this.InputDate == null ? MinValueYearToDataBaseInt : Convert.ToInt32(this.InputDate.Year);
            }
        }
        #endregion
        #region MonthToInt (このインスタンスで表される日付の月コンポーネントを取得します。)
        /// <summary>
        /// このインスタンスで表される日付の月コンポーネントを取得します。
        /// </summary>
        /// <returns>月</returns>
        [Description("このインスタンスで表される日付の月コンポーネントを取得します。")]
        public int MonthToInt
        {
            get
            {
                return this.InputDate == null ? MinValueMonthToDataBaseInt : Convert.ToInt32(this.InputDate.Month);
            }
        }
        #endregion
        #region DayToInt (このインスタンスで表される日付の日コンポーネントを取得します。)
        /// <summary>
        /// このインスタンスで表される日付の日コンポーネントを取得します。
        /// </summary>
        /// <returns>日</returns>
        [Description("このインスタンスで表される日付の日コンポーネントを取得します。")]
        public int DayToInt
        {
            get
            {
                return this.InputDate == null ? MinValueDayToDataBaseInt : Convert.ToInt32(this.InputDate.Day);
            }
        }
        #endregion
        #endregion
        #region ToDecimal
        #region YearToDecimal (このインスタンスで表される日付の年コンポーネントを取得します。)
        /// <summary>
        /// このインスタンスで表される日付の年コンポーネントを取得します。
        /// </summary>
        /// <returns>年</returns>
        [Description("このインスタンスで表される日付の年コンポーネントを取得します。")]
        public decimal YearToDecimal
        {
            get
            {
                return this.InputDate == null ? MinValueYearToDataBaseInt : Convert.ToDecimal(this.InputDate.Year);
            }
        }
        #endregion
        #region MonthToDecimal (このインスタンスで表される日付の月コンポーネントを取得します。)
        /// <summary>
        /// このインスタンスで表される日付の月コンポーネントを取得します。
        /// </summary>
        /// <returns>月</returns>
        [Description("このインスタンスで表される日付の月コンポーネントを取得します。")]
        public decimal MonthToDecimal
        {
            get
            {
                return this.InputDate == null ? MinValueMonthToDataBaseInt : Convert.ToDecimal(this.InputDate.Month);
            }
        }
        #endregion
        #region DayToDecimal (このインスタンスで表される日付の日コンポーネントを取得します。)
        /// <summary>
        /// このインスタンスで表される日付の日コンポーネントを取得します。
        /// </summary>
        /// <returns>日</returns>
        [Description("このインスタンスで表される日付の日コンポーネントを取得します。")]
        public decimal DayToDecimal
        {
            get
            {
                return this.InputDate == null ? MinValueDayToDataBaseInt : Convert.ToDecimal(this.InputDate.Day);
            }
        }
        #endregion
        #endregion
        #region ToDouble
        #region YearToDouble (このインスタンスで表される日付の年コンポーネントを取得します。)
        /// <summary>
        /// このインスタンスで表される日付の年コンポーネントを取得します。
        /// </summary>
        /// <returns>年</returns>
        [Description("このインスタンスで表される日付の年コンポーネントを取得します。")]
        public double YearToDouble
        {
            get
            {
                return this.InputDate == null ? 1 : Convert.ToDouble(this.InputDate.Year);
            }
        }
        #endregion
        #region MonthToDecimal (このインスタンスで表される日付の月コンポーネントを取得します。)
        /// <summary>
        /// このインスタンスで表される日付の月コンポーネントを取得します。
        /// </summary>
        /// <returns>月</returns>
        [Description("このインスタンスで表される日付の月コンポーネントを取得します。")]
        public double MonthToDouble
        {
            get
            {
                return this.InputDate == null ? 1 : Convert.ToDouble(this.InputDate.Month);
            }
        }
        #endregion
        #region DayToDecimal (このインスタンスで表される日付の日コンポーネントを取得します。)
        /// <summary>
        /// このインスタンスで表される日付の日コンポーネントを取得します。
        /// </summary>
        /// <returns>日</returns>
        [Description("このインスタンスで表される日付の日コンポーネントを取得します。")]
        public double DayToDouble
        {
            get
            {
                return this.InputDate == null ? 1 : Convert.ToDouble(this.InputDate.Day);
            }
        }
        #endregion
        #endregion

        // 日付の日付型
        // Note:入力した文字列が日付に変換できるものだけ利用可能
        #region ToDateTime (このインスタンスで表される日付の年コンポーネントを取得します。)
        /// <summary>
        /// このインスタンスで表される日付を取得します。
        /// </summary>
        /// <returns>日付</returns>
        [Description("このインスタンスで表される日付の年コンポーネントを取得します。")]
        public DateTime ToDateTime
        {
            get
            {
                return this.InputDate;
            }
        }
        #endregion

        // 和暦の文字列型
        // Note:入力した文字列が日付に変換できるものだけ利用可能
        #region ToString (和暦)
        #region DateToString (このインスタンスで表される日付の年月日コンポーネントを取得します。)
        /// <summary>
        /// このインスタンスで表される日付の年月日コンポーネントを取得します。
        /// </summary>
        /// <returns>年</returns>
        [Description("このインスタンスで表される日付の年コンポーネントを取得します。")]
        public string DateToString
        {
            get
            {
                // このインスタンスで
                return this.GetJapaneseDateToString(GengoOutputType.WarekiName,
                                                    SplitOutputType.Kanji,
                                                    YearOutputType.Display, MonthOutputType.Display, DayOutputType.Display,
                                                    NendoConv.No,
                                                    GanNenType.Kanji,
                                                    MonthDayZeroPadding.No);
            }
        }
        #endregion
        #region YearToString (このインスタンスで表される日付の年コンポーネントを取得します。)
        /// <summary>
        /// このインスタンスで表される日付の年コンポーネントを取得します。
        /// </summary>
        /// <returns>年</returns>
        [Description("このインスタンスで表される日付の年コンポーネントを取得します。")]
        public string YearToString
        {
            get
            {
                return this.GetJapaneseDateToString(GengoOutputType.WarekiName,
                                                    SplitOutputType.Kanji,
                                                    YearOutputType.Display, MonthOutputType.Hide, DayOutputType.Hide,
                                                    NendoConv.No,
                                                    GanNenType.Kanji,
                                                    MonthDayZeroPadding.No);
            }
        }
        #endregion
        #region MonthToString (このインスタンスで表される日付の月コンポーネントを取得します。)
        /// <summary>
        /// このインスタンスで表される日付の月コンポーネントを取得します。
        /// </summary>
        /// <returns>月</returns>
        [Description("このインスタンスで表される日付の月コンポーネントを取得します。")]
        public string MonthToString
        {
            get
            {
                return this.GetJapaneseDateToString(GengoOutputType.WarekiName,
                                                    SplitOutputType.Kanji,
                                                    YearOutputType.Hide, MonthOutputType.Display, DayOutputType.Hide,
                                                    NendoConv.No,
                                                    GanNenType.Kanji,
                                                    MonthDayZeroPadding.No);
            }
        }
        #endregion
        #region MonthZeroPaddingToString (このインスタンスで表される日付の月コンポーネントを取得します。)
        /// <summary>
        /// このインスタンスで表される日付の月コンポーネントを取得します。
        /// </summary>
        /// <returns>月</returns>
        [Description("このインスタンスで表される日付の月コンポーネントを取得します。")]
        public string MonthZeroPaddingToString
        {
            get
            {
                return this.GetJapaneseDateToString(GengoOutputType.WarekiName,
                                                    SplitOutputType.Kanji,
                                                    YearOutputType.Hide, MonthOutputType.Display, DayOutputType.Hide,
                                                    NendoConv.No,
                                                    GanNenType.Kanji,
                                                    MonthDayZeroPadding.Yes);

            }
        }
        #endregion
        #region DayTostring (このインスタンスで表される日付の日コンポーネントを取得します。)
        /// <summary>
        /// このインスタンスで表される日付の日コンポーネントを取得します。
        /// </summary>
        /// <returns>日</returns>
        [Description("このインスタンスで表される日付の日コンポーネントを取得します。")]
        public string DayTostring
        {
            get
            {
                return this.GetJapaneseDateToString(GengoOutputType.WarekiName,
                                                    SplitOutputType.Kanji,
                                                    YearOutputType.Hide, MonthOutputType.Hide,DayOutputType.Display,
                                                    NendoConv.No,
                                                    GanNenType.Kanji,
                                                    MonthDayZeroPadding.Yes);
            }
        }
        #endregion
        #region DayZeroPaddingToString (このインスタンスで表される日付の日コンポーネントを取得します。)
        /// <summary>
        /// このインスタンスで表される日付の日コンポーネントを取得します。
        /// </summary>
        /// <returns>日</returns>
        [Description("このインスタンスで表される日付の日コンポーネントを取得します。")]
        public string DayZeroPaddingToString
        {
            get
            {
                return this.GetJapaneseDateToString(GengoOutputType.WarekiName,
                                                    SplitOutputType.Kanji,
                                                    YearOutputType.Hide, MonthOutputType.Hide, DayOutputType.Display,
                                                    NendoConv.No,
                                                    GanNenType.Kanji,
                                                    MonthDayZeroPadding.Yes);
            }
        }
        #endregion
        #endregion

        // 年度の文字列型
        #region ToString (年度)
        #region NendoDateToString (このインスタンスで表される日付の年月日コンポーネントを取得します。)
        /// <summary>
        /// このインスタンスで表される日付の年月日コンポーネントを取得します。
        /// </summary>
        /// <returns>年度の年月日</returns>
        [Description("このインスタンスで表される日付の年コンポーネントを取得します。")]
        public string NendoDateToString
        {
            get
            {
                return this.GetJapaneseDateToString(GengoOutputType.WarekiName,
                                                    SplitOutputType.Kanji,
                                                    YearOutputType.Display, MonthOutputType.Display, DayOutputType.Display,
                                                    NendoConv.No,
                                                    GanNenType.Kanji,
                                                    MonthDayZeroPadding.Yes);
            }
        }
        #endregion
        #region NendoYearToString (このインスタンスで表される日付の年コンポーネントを取得します。)
        /// <summary>
        /// このインスタンスで表される日付の年コンポーネントを取得します。
        /// </summary>
        /// <returns>年度の年</returns>
        [Description("このインスタンスで表される日付の年コンポーネントを取得します。")]
        public string NendoYearToString
        {
            get
            {
                return this.GetJapaneseDateToString(GengoOutputType.WarekiName,
                                                    SplitOutputType.Kanji,
                                                    YearOutputType.Display, MonthOutputType.Hide, DayOutputType.Hide,
                                                    NendoConv.No,
                                                    GanNenType.Kanji,
                                                    MonthDayZeroPadding.No);
            }
        }
        #endregion
        #region NendoMonthToString (このインスタンスで表される日付の月コンポーネントを取得します。)
        /// <summary>
        /// このインスタンスで表される日付の月コンポーネントを取得します。
        /// </summary>
        /// <returns>月</returns>
        [Description("このインスタンスで表される日付の月コンポーネントを取得します。")]
        public string NendoMonthToString
        {
            get
            {
                return this.GetJapaneseDateToString(GengoOutputType.WarekiName,
                                                    SplitOutputType.Kanji,
                                                    YearOutputType.Hide, MonthOutputType.Display, DayOutputType.Hide,
                                                    NendoConv.No,
                                                    GanNenType.Kanji,
                                                    MonthDayZeroPadding.No);
            }
        }
        #endregion
        #region NendoMonthZeroPaddingToString (このインスタンスで表される日付の月コンポーネントを取得します。)
        /// <summary>
        /// このインスタンスで表される日付の月コンポーネントを取得します。
        /// </summary>
        /// <returns>月</returns>
        [Description("このインスタンスで表される日付の月コンポーネントを取得します。")]
        public string NendoMonthZeroPaddingToString
        {
            get
            {
                // このインスタンスで
                return this.GetJapaneseDateToString(GengoOutputType.WarekiName,
                                                    SplitOutputType.Kanji,
                                                    YearOutputType.Hide, MonthOutputType.Display, DayOutputType.Hide,
                                                    NendoConv.No,
                                                    GanNenType.Kanji,
                                                    MonthDayZeroPadding.Yes);
            }
        }
        #endregion
        #region NendoDayTostring (このインスタンスで表される日付の日コンポーネントを取得します。)
        /// <summary>
        /// このインスタンスで表される日付の日コンポーネントを取得します。
        /// </summary>
        /// <returns>日</returns>
        [Description("このインスタンスで表される日付の日コンポーネントを取得します。")]
        public string NendoDayTostring
        {
            get
            {
                return this.GetJapaneseDateToString(GengoOutputType.WarekiName,
                                                    SplitOutputType.Kanji,
                                                    YearOutputType.Hide, MonthOutputType.Hide, DayOutputType.Display,
                                                    NendoConv.Yes,
                                                    GanNenType.Kanji,
                                                    MonthDayZeroPadding.No);
            }
        }
        #endregion
        #region NendoDayZeroPaddingToString (このインスタンスで表される日付の日コンポーネントを取得します。)
        /// <summary>
        /// このインスタンスで表される日付の日コンポーネントを取得します。
        /// </summary>
        /// <returns>日</returns>
        [Description("このインスタンスで表される日付の日コンポーネントを取得します。")]
        public string NendoDayZeroPaddingToString
        {
            get
            {
                // このインスタンスで
                return this.GetJapaneseDateToString(GengoOutputType.WarekiName,
                                                    SplitOutputType.Kanji,
                                                    YearOutputType.Hide, MonthOutputType.Hide, DayOutputType.Display,
                                                    NendoConv.Yes,
                                                    GanNenType.Kanji,
                                                    MonthDayZeroPadding.Yes);
            }
        }
        #endregion
        #endregion

        // データベースに格納する日付値
        #region ToDataBaseInt (数値)
        /// <summary>
        /// 
        /// </summary>
        public int ToDataBaseInt
        {
            get
            {
                return Convert.ToInt32(this.GetDataBaseInt());
            }
        }
        #endregion
        #region ToDataBaseDecimal
        /// <summary>
        /// 
        /// </summary>
        public decimal ToDataBaseDecimal
        {
            get
            {
                return Convert.ToDecimal(this.GetDataBaseInt());
            }
        }
        #endregion
        #region ToDataBaseString
        /// <summary>
        /// 
        /// </summary>
        public string ToDataBaseString
        {
            get
            {
                return this.GetJapaneseDateToString(GengoOutputType.None,
                                                    SplitOutputType.Slash,
                                                    YearOutputType.Display, MonthOutputType.Display, DayOutputType.Display,
                                                    NendoConv.No,
                                                    GanNenType.Kanji,
                                                    MonthDayZeroPadding.Yes);
            }
        }
        #endregion
        
        // その他
        #region NendoRegexString(年度の正規表現を作成します。)
        /// <summary>
        /// 年度の正規表現を作成します。
        /// </summary>
        /// <remarks>
        /// この処理はデバッグ用です。
        /// </remarks>
        public string NendoRegexString()
        {
            string workRegex = ""; // 正規表現の作成

            workRegex = "";
            for (int cnt = (int)Gengo.Min; cnt <= (int)Gengo.Max; cnt++)
            {
                // Todo:string.Joinが使えない。
                workRegex += GengoCharAlphabet[cnt] + "|" + GengoCharNum[cnt] + "|" + GengoName[cnt] + "|" + GengoAbbreviation[cnt];

                if (cnt != (int)Gengo.Max) // 最後の文字は|を出力しない。
                {
                    workRegex += "|";
                }
            }

            return NendoRegexBase.Replace(@"【元号】", workRegex);
        }
        #endregion
        #region MonthToIndex (登録している日付の月から月Indexを返します。)
        /// <summary>
        /// 登録している日付の月から月Indexを返します。
        /// </summary>
        /// <param name="month">月</param>
        /// <returns></returns>
        public int MonthToIndex(int month)
        {
            if (month.isBetween(1, 12))
            {
                return (month <= 3) ? month + 9 : month - 3;
            }
            else
            {
                return 0;
            }
        }
        #endregion
        #region IndexToMonth (登録している日付の月から月Indexを返します。)
        /// <summary>
        /// 登録している日付の月から月Indexを返します。
        /// </summary>
        /// <param name="index">月のIndex</param>
        /// <returns></returns>
        public int IndexToMonth(int index)
        {
            if (index.isBetween(1, 12))
            {
                return (index >= 10) ? index - 9 : index + 3;
            }
            else
            {
                return 0;
            }
        }
        #endregion
        #region GetGengoState (元号とデータ取得タイプから登録している元号の設定値を文字列として返します。)
        /// <summary>
        /// 元号とデータ取得タイプから登録している元号の設定値を文字列として返します。
        /// </summary>
        /// <param name="gengo">元号</param>
        /// <param name="type">取得タイプ</param>
        /// <returns>登録している元号の設定値</returns>
        public string GetGengoState(Gengo gengo, DataType type)
        {
            int work = (int)gengo;

            switch (type)
            {
                case DataType.GengoStartYear:
                    return GengoStartYear[work].ToString();
                case DataType.GengoEndYear:
                    return GengoEndYear[work].ToString();
                case DataType.GengoStartNendo:
                    return GengoStartNendo[work].ToString();
                case DataType.GengoCharAlphabet:
                    return GengoCharAlphabet[work].ToString();
                case DataType.GengoCharNum:
                    return GengoCharNum[work].ToString();
                case DataType.GengoName:
                    return GengoName[work].ToString();
                case DataType.GengoAbbreviation:
                    return GengoAbbreviation[work].ToString();
                default:
                    return "";
            }
        }
        #endregion
        #endregion

        #region privateメソッド
        // 初期化
        #region NendoInitializ (初期化を行います。)
        /// <summary>
        /// 初期化を行います。
        /// </summary>
        private void NendoInitializ()
        {
            this.WarekiFormatType = WarekiFormat.Wareki_JP;
            this.TargetGengo = Gengo.None;
            this.Year = 0;             // 西暦
            this.Month = 0;            // 月
            this.Day = 0;              // 日
            this.Nendo = 0;            // 年度
            this.EraYear = 0;          // 元号の年
            this.Era = "";             // 元号の正式名称
            this.EraAlphabet = "";     // 元号の記号
            this.EraAbbreviation = ""; // 元号の略称
        }
        #endregion

        // 日付文字列 → 日付変換処理
        #region SetDataAssort (日付として妥当か仕分けます。)
        /// <summary>
        /// 日付として妥当か仕分けます。
        /// </summary>
        /// <param name="convData">変換対象となる日付データ</param>
        /// <returns>変換結果ステータス</returns>
        private DateStatus SetDataAssort(string convData)
        {
            DateTime tryDate;
            string workDate;
            int tryInt;

            this.NendoInitializ(); // 初期化

            // 送られてきた文字列は日付に変換できるか。
            if (DateTime.TryParse(convData, out tryDate))
            {
                return this.SetDateAndGengou(tryDate);
            }
            else if (int.TryParse(convData, out tryInt)) // 送られてきた文字列が数値に変換できるか。
            {
                if (convData.Length == 7)
                {
                    // 1桁目が1～4か
                    string charStr = convData.Substring(0, 1);
                    string makeStr = "";
                    bool chkFlg;

                    chkFlg = true;
                    for (int cnt = (int)Gengo.Min; cnt <= (int)Gengo.Max; cnt++)
                    {
                        if (GengoCharNum[cnt] == charStr)
                        {
                            // 元号99年9999に変換する。
                            makeStr = GengoName[cnt] + convData.Substring(1, 2) + "年" + convData.Substring(3, 2) + "月" + convData.Substring(5, 2) + "日";
                            chkFlg = false;
                            break;
                        }
                    }

                    if (chkFlg)
                    {
                        return DateStatus.ConversionImpossible;
                    }

                    return this.SetDataAssortWareki(makeStr);

                }
                else if (convData.Length == 8)
                {
                    // yyyy/mm/dd形式に文字列を整える。
                    workDate = convData.Substring(0, 4) + "/" + convData.Substring(4, 2) + "/" + convData.Substring(6, 2);

                    // 送られてきた文字列は日付に変換できるか。
                    if (DateTime.TryParse(workDate, out tryDate))
                    {
                        return this.SetDateAndGengou(tryDate);
                    }
                    else
                    {
                        return DateStatus.ConversionImpossible;
                    }
                }
                else
                {
                    return DateStatus.ConversionImpossible;
                }
            }

            // 正規表現チェック
            if (!this.RegexIsMatch(convData))
            {
                return DateStatus.RegexIsMatchError;
            }

            return this.SetDataAssortWareki(convData);
        }
        #endregion
        #region SetDataAssortWareki (和暦日付として妥当か仕分けます。)
        /// <summary>
        /// 和暦日付として妥当か仕分けます。
        /// </summary>
        /// <param name="dataString"></param>
        /// <returns></returns>
        private DateStatus SetDataAssortWareki(string dataString)
        {
            int workYear;
            int workMonth;
            int workDay;
            DateTime checkDate;
            string item;

            item = dataString;

            // 文字列を区切り文字(カンマ)に変換する。元は平成元年を1年とみなす。
            item = item.Replace("/", ",").Replace(".", ",").Replace("年", ",").Replace("月", ",").Replace("日", "").Replace("元", "01");

            bool chkdate = true; // 変換可能かチェックするフラグ。trueは変換できなかった状態。

            for (int cnt = (int)Gengo.Min; cnt <= (int)Gengo.Max; cnt++)
            {
                string w = GengoStartNendo[cnt].ToString() + ","; // 元号のみ元年の数字に変換する。

                // 変換対象のデータが存在するか確認
                if ((dataString.IndexOf(GengoCharAlphabet[cnt]) >= 0) || (dataString.IndexOf(GengoName[cnt]) >= 0) || (dataString.IndexOf(GengoAbbreviation[cnt]) >= 0))
                {
                    chkdate = false; // 変換できたのでフラグを下げる
                    item = item.Replace(GengoCharAlphabet[cnt], w).Replace(GengoName[cnt], w).Replace(GengoAbbreviation[cnt], w); // 元号を現年の数字に変換する。
                    string[] Itemarray = item.Split(','); // 変換した区切り文字を配列にする。

                    if (Itemarray.Count() != 4) // 元号、年、月、日の4つに分解できなかった場合0を返す。
                    {
                        return DateStatus.ConversionImpossible;
                    }

                    // 配列にしたデータから年、月、日を保持。
                    // 元号と年度分を足した値を年とする
                    int baseYear = Convert.ToInt32(Itemarray[0]);
                    int addYear = Convert.ToInt32(Itemarray[1]);
                    workYear = (addYear == 1) ? baseYear : baseYear + addYear - 1; // 年
                    workMonth = Convert.ToInt32(Itemarray[2]);                 // 月
                    workDay = Convert.ToInt32(Itemarray[3]);                   // 日

                    // 日付(数字)を作成
                    int makeDate = Convert.ToInt32(workYear.ToString() + workMonth.ToString().PadLeft(2, '0') + workDay.ToString().PadLeft(2, '0'));

                    // 日付(文字)を作成
                    string tryStr = workYear.ToString() + "/" + workMonth.ToString().PadLeft(2, '0') + "/" + workDay.ToString().PadLeft(2, '0');

                    // 年度の範囲内か確認する。
                    if (makeDate.isBetween(GengoStartYear[cnt], GengoEndYear[cnt]))
                    {

                        if (DateTime.TryParse(tryStr, out checkDate))
                        {
                            // 変換が成功した場合、
                            return this.SetDateAndGengou(checkDate);
                        }
                        else
                        {
                            return DateStatus.ConversionImpossible;
                        }
                    }
                    else
                    {
                        return DateStatus.Error_NendoRenge;
                    }
                }
            }

            if (chkdate) // 保持している年号分ループ中に変換対象データが存在しなかった
            {
                // このメソッドは登録している元号以外のデータはエラーとする。
                return DateStatus.ConversionImpossible;
            }

            return DateStatus.Error_Fatal;

        }
        #endregion
        #region SetDateAndGengou (日付変換ができたものを登録し、日付から元号をセットします。)
        /// <summary>
        /// 日付変換ができたものを登録し、日付から元号をセットします。
        /// </summary>
        /// <param name="date"></param>
        private DateStatus SetDateAndGengou(DateTime date)
        {
            this.TargetGengo = Gengo.None; // 元号を初期化
            this.InputDate = date;        // 変換できた日付を登録
            this.EraYear = 0;      // 和暦を初期化

            bool chkdate; // 元号取得可否フラグ

            chkdate = true; // 元号取得可否フラグ初期化

            for (int cnt = (int)Gengo.Min; cnt <= (int)Gengo.Max; cnt++)
            {
                int wdate = Convert.ToInt32(date.ToString("yyyyMMdd"));

                // 変換対象のデータが存在するか確認
                if (wdate.isBetween(GengoStartYear[cnt], GengoEndYear[cnt]))
                {
                    this.TargetGengo = (Gengo)cnt; // 元号を初期化
                    this.Year = InputDate.Year; // 西暦
                    this.Month = InputDate.Month; // 月
                    this.Day = InputDate.Day; // 日
                    this.EraYear = (Convert.ToInt32(this.InputDate.Year) - GengoStartNendo[cnt] + 1); // 和暦年数を保存
                    this.Nendo = this.Month.isBetween(1, 3) ? this.EraYear - 1 : this.EraYear; // 年度(１月から3月までは年を-1する)
                    this.Era = GengoName[cnt]; // 元号の正式名称
                    this.EraAlphabet = GengoCharAlphabet[cnt]; // 元号の記号
                    this.EraAbbreviation = GengoAbbreviation[cnt]; // 元号の略称
                    chkdate = false; // 変換できたのでフラグを下げる
                    break;
                }
            }

            if (chkdate)
            {
                Console.WriteLine("元号範囲外");
            }

            return DateStatus.Success;
        }
        #endregion
        #region RegexIsMatch (正規表現で変換可能な日付かチェックします。)
        /// <summary>
        /// 正規表現で変換可能な日付かチェックします。
        /// </summary>
        /// <param name="dt">日付</param>
        /// <returns></returns>
        private bool RegexIsMatch(string dt)
        {
            // 正規表現チェック
            if (Regex.IsMatch(dt, NendoRegex))
            {
                return true;
            }
            return false;
        }
        #endregion
        #region DatePrepare (入力された日付文字列を読み込み可能な状態にする前処理を行います。)
        /// <summary>
        /// 入力された日付文字列を読み込み可能な状態にする前処理を行います。
        /// </summary>
        /// <returns></returns>
        private string DatePrepare(string dateStr)
        {
            string ret;

            ret = dateStr;

            // 大文字に変換
            ret = ret.ToUpper();

            // 年月日を変換
            ret = ret.Replace(".", "/").Replace("年", "/").Replace("月", "/").Replace("日", "");

            // 元号を記号に変更
            for (int cnt = (int)Gengo.Min; cnt <= (int)Gengo.Max; cnt++)
            {
                ret = ret.Replace(GengoName[cnt], GengoCharAlphabet[cnt]);
                ret = ret.Replace(GengoAbbreviation[cnt], GengoCharAlphabet[cnt]);
            }

            return ret;
        }
        #endregion
        
        // データベースに格納する値
        #region GetDataBaseInt
        private int GetDataBaseInt()
        {
            string date = this.GetJapaneseDateToString(GengoOutputType.Seireki,
                                                       SplitOutputType.None,
                                                       YearOutputType.Display, MonthOutputType.Display, DayOutputType.Display,
                                                       NendoConv.No,
                                                       GanNenType.Kanji,
                                                       MonthDayZeroPadding.Yes);

            if (date == "")
            {
                return 0;
            }

            int dateint = Convert.ToInt32(date); // 日付を数値型に変更

            // 数値日付がデータベースに格納可能な日付の範囲でない場合、データベースに格納可能な最小値を返す。
            return dateint <= MinValueDateToDataBaseInt ? MinValueDateToDataBaseInt : dateint;
        }
        #endregion

        #endregion

    }
}
