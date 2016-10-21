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
    public class JapaneseDate
    {
        #region 定数
        #region ReadOnlyCollection(元号)
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
        public static readonly ReadOnlyCollection<string> GengoName = Array.AsReadOnly(new string[] { "初期値", "明治", "大正", "昭和", "平成" });
        /// <summary>
        /// 元号を略称で表します。
        /// </summary>
        public static readonly ReadOnlyCollection<string> GengoAbbreviation = Array.AsReadOnly(new string[] { "初期値", "明", "大", "昭", "平" });
        /// <summary>
        /// 元号の正規表現です。
        /// </summary>
        public static readonly string NendoRegex = "(M|1|明治|明|T|2|大正|大|S|3|昭和|昭|H|4|平成|平)[0-9]{1,2}(年|/|.)[0-1]?[1-9]{1}(月|/|.)[0-3]?[1-9]{1}(日)?";
        /// <summary>
        /// 元号の正規表現ベースです。
        /// </summary>
        public static readonly string NendoRegexBase = @"(【元号】)[0-9]{1,2}(年|/|.)[0-1]?[1-9]{1}(月|/|.)[0-3]?[1-9]{1}(日)?";
        #endregion
        #region enumItem
        /// <summary>
        /// 元号の種別です。
        /// </summary>
        public enum GengoType
        {
            /// <summary>
            /// 元号の正式名称です
            /// </summary>
            Name,
            /// <summary>
            /// 元号のアルファベット1文字です
            /// </summary>
            Alphabet,
            /// <summary>
            /// 元号の略称です。
            /// </summary>
            Abbreviation,
        }

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

        /// <summary>
        /// 和暦変換した時のフォーマットです。
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
        #region フィールド
        /// <summary>
        /// 入力
        /// </summary>
        private DateTime TargetDate;

        /// <summary>
        /// 和暦の年度を保持します。
        /// </summary>
        private string WarekiYear;

        /// <summary>
        /// セットされた元号です。
        /// </summary>
        private Gengo TargetGengo;

        /// <summary>
        /// JapaneseDateで表現する元号の種類です。 
        /// </summary>
        public WarekiFormat WarekiFormatType { get; set; }
        #endregion
        #region Publicメソッド
        #region コンストラクタ
        /// <summary>
        /// 日付文字列のコンストラクタです。
        /// </summary>
        /// <param name="data">日付文字列</param>
        public JapaneseDate(string data)
        {
            this.SetData(data);
        }

        /// <summary>
        /// 日付のコンストラクタです。
        /// </summary>
        /// <param name="data">日付</param>
        public JapaneseDate(DateTime data)
        {
            this.SetData(data);
        }

        /// <summary>
        /// 数値のコンストラクタです。
        /// </summary>
        /// <see cref=""/>
        /// <param name="data">日付数値</param>
        public JapaneseDate(int data)
        {
            this.SetData(data);
        }
        #endregion
        #region SetData (日付となる値をセットします。)
        /// <summary>
        /// 日付となる値をセットします。
        /// </summary>
        /// <param name="convData">変換対象となる日付データ</param>
        /// <returns>変換結果ステータス</returns>
        public DateStatus SetData(DateTime convData)
        {
            return this.SetDataAssort(convData.ToString());
        }
        #endregion
        #region SetData (日付となる値をセットします。)
        /// <summary>
        /// 日付となる値をセットします。
        /// </summary>
        /// <param name="convData">変換対象となる日付データ</param>
        /// <returns>変換結果ステータス</returns>
        public DateStatus SetData(int convData)
        {
            return this.SetDataAssort(convData.ToString());
        }
        #endregion
        #region SetData (日付となる値をセットします。)
        /// <summary>
        /// 日付となる値をセットします。
        /// </summary>
        /// <param name="convData">変換対象となる日付データ</param>
        /// <returns>変換結果ステータス</returns>
        public DateStatus SetData(string convData)
        {
            return this.SetDataAssort(convData.ToString());
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
        #region Year (このインスタンスで表される日付の年コンポーネントを取得します。)
        /// <summary>
        /// このインスタンスで表される日付を取得します。
        /// </summary>
        /// <returns>日付</returns>
        [Description("このインスタンスで表される日付の年コンポーネントを取得します。")]
        public DateTime DateTime
        {
            get
            {
                return this.TargetDate;
            }
        }
        #endregion        
        #region Year (このインスタンスで表される日付の年コンポーネントを取得します。)
        /// <summary>
        /// このインスタンスで表される日付の年コンポーネントを取得します。
        /// </summary>
        /// <returns>年</returns>
        [Description("このインスタンスで表される日付の年コンポーネントを取得します。")]
        public int Year
        {
            get
            {
                return this.TargetDate == null ? 1 : this.TargetDate.Year;
            }
        }
        #endregion
        #region Month (このインスタンスで表される日付の月コンポーネントを取得します。)
        /// <summary>
        /// このインスタンスで表される日付の月コンポーネントを取得します。
        /// </summary>
        /// <returns>月</returns>
        [Description("このインスタンスで表される日付の月コンポーネントを取得します。")]
        public int Month
        {
            get
            {
                return this.TargetDate == null ? 1 : this.TargetDate.Month;
            }
        }
        #endregion
        #region Day (このインスタンスで表される日付の日コンポーネントを取得します。)
        /// <summary>
        /// このインスタンスで表される日付の日コンポーネントを取得します。
        /// </summary>
        /// <returns>日</returns>
        [Description("このインスタンスで表される日付の日コンポーネントを取得します。")]
        public int Day
        {
            get
            {
                return this.TargetDate == null ? 1 : this.TargetDate.Day;
            }
        }
        #endregion
        #region JapaneseYear (このインスタンスで表される日付の年コンポーネントを取得します。)
        /// <summary>
        /// このインスタンスで表される日付の年コンポーネントを取得します。
        /// </summary>
        /// <returns>年</returns>
        [Description("このインスタンスで表される日付の年コンポーネントを取得します。")]
        public string JapaneseYear
        {
            get
            {
                return this.TargetDate == null ? "" : this.TargetDate.Year.ToString() + "年";
            }
        }
        #endregion
        #region JapaneseMonth (このインスタンスで表される日付の月コンポーネントを取得します。)
        /// <summary>
        /// このインスタンスで表される日付の月コンポーネントを取得します。
        /// </summary>
        /// <returns>月</returns>
        [Description("このインスタンスで表される日付の月コンポーネントを取得します。")]
        public string JapaneseMonth
        {
            get
            {
                return this.TargetDate == null ? "" : this.TargetDate.Month.ToString() + "月";
            }
        }
        #endregion
        #region JapaneseDay (このインスタンスで表される日付の日コンポーネントを取得します。)
        /// <summary>
        /// このインスタンスで表される日付の日コンポーネントを取得します。
        /// </summary>
        /// <returns>日</returns>
        [Description("このインスタンスで表される日付の日コンポーネントを取得します。")]
        public string JapaneseDay
        {
            get
            {
                return this.TargetDate == null ? "" : this.TargetDate.Day.ToString() + "日";
            }
        }
        #endregion
        #region Date (このインスタンスで表される日付の日コンポーネントを取得します。)
        /// <summary>
        /// このインスタンスで表される日付の日コンポーネントを取得します。
        /// </summary>
        /// <returns>日</returns>
        [Description("このインスタンスで表される日付の日コンポーネントを取得します。")]
        public string Date
        {
            get
            {
                return this.TargetDate == null ? "" : this.Wareki(false);
            }
        }
        #endregion
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
        #region NendoYearString (このインスタンスで表される日付の年コンポーネントを取得します。)
        /// <summary>
        /// このインスタンスで表される日付の年コンポーネントを取得します。
        /// </summary>
        /// <returns>年</returns>
        [Description("このインスタンスで表される日付の年コンポーネントを取得します。")]
        public string NendoYearString
        {
            get
            {
                string nendo = this.WarekiYear;

                // １月から3月までは年を-1する。
                if (this.Month.isBetween(1, 3))
                {
                    nendo = (Convert.ToInt32(this.WarekiYear) - 1).ToString();
                }

                // 元年表記にする
                return (nendo.isBetween("0", "1")) ? "元年度" : nendo + "年度";

            }
        }
        #endregion
        #region NendoYearInt (このインスタンスで表される日付の年コンポーネントを取得します。)
        /// <summary>
        /// このインスタンスで表される日付の年コンポーネントを取得します。
        /// </summary>
        /// <returns>年</returns>
        [Description("このインスタンスで表される日付の年コンポーネントを取得します。")]
        public int NendoYearInt
        {
            get
            {
                int w = Convert.ToInt32(this.WarekiYear);

                // １月から3月までは年を-1する。
                if (this.Month.isBetween(1, 3))
                {
                    w--;
                }

                return w;
            }
        }
        #endregion        
        #region NendoMonth (このインスタンスで表される日付の月コンポーネントを取得します。)
        /// <summary>
        /// このインスタンスで表される日付の月コンポーネントを取得します。
        /// </summary>
        /// <returns>月</returns>
        [Description("このインスタンスで表される日付の月コンポーネントを取得します。")]
        public string NendoMonth
        {
            get
            {
                return this.JapaneseMonth;
            }
        }
        #endregion
        #region NendoiDay (このインスタンスで表される日付の日コンポーネントを取得します。)
        /// <summary>
        /// このインスタンスで表される日付の日コンポーネントを取得します。
        /// </summary>
        /// <returns>日</returns>
        [Description("このインスタンスで表される日付の日コンポーネントを取得します。")]
        public string NendoiDay
        {
            get
            {
                return this.JapaneseDay;
            }
        }
        #endregion
        #region NendoDate (このインスタンスで表される日付の日コンポーネントを取得します。)
        /// <summary>
        /// このインスタンスで表される日付の日コンポーネントを取得します。
        /// </summary>
        /// <returns>日</returns>
        [Description("このインスタンスで表される日付の日コンポーネントを取得します。")]
        public string NendoDate
        {
            get
            {
                return this.TargetDate == null ? "" : this.Wareki(true);
            }
        }
        #endregion
        #region ThisMonthToIndex (このインスタンスで表される日付の月インデックスを返します。)
        /// <summary>
        /// このインスタンスで表される日付の月インデックスを返します。
        /// </summary>
        public int ThisMonthToIndex
        {
            get
            {

                int i = 0;

                if (this.TargetDate == null)
                {
                    return i;
                }

                i = (this.TargetDate.Month <= 3) ? this.TargetDate.Month + 9 : this.TargetDate.Month - 3;

                return i;
            }
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
        #endregion
        #region privateメソッド
        #region NendoInitializ (初期化を行います。)
        /// <summary>
        /// 初期化を行います。
        /// </summary>
        private void NendoInitializ()
        {
            this.WarekiFormatType = WarekiFormat.Wareki_JP;
            this.TargetGengo = Gengo.None;
        }
        #endregion
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
            this.TargetDate = date;              // 変換できた日付を登録
            this.WarekiYear = "";           // 和暦を初期化

            bool chkdate; // 元号取得可否フラグ

            chkdate = true; // 元号取得可否フラグ初期化

            for (int cnt = (int)Gengo.Min; cnt <= (int)Gengo.Max; cnt++)
            {
                int wdate = Convert.ToInt32(date.ToString("yyyyMMdd"));

                // 変換対象のデータが存在するか確認
                if (wdate.isBetween(GengoStartYear[cnt], GengoEndYear[cnt]))
                {
                    this.TargetGengo = (Gengo)cnt; // 元号を初期化
                    this.WarekiYear = (Convert.ToInt32(this.TargetDate.Year) - GengoStartNendo[cnt] + 1).ToString();    // 和暦年数を保存
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
        #region Wareki (登録している日付から和暦を返します。)
        /// <summary>
        /// 登録している日付から和暦を返します。
        /// </summary>
        /// <returns></returns>
        private string Wareki(bool nend)
        {
            string ret;
            string wareki;

            // 年度を返す場合
            if (nend)
            {
                wareki = this.WarekiYear;

                // １月から3月までは年を-1する。
                if (this.Month.isBetween(1, 3))
                {
                    wareki = (Convert.ToInt32(this.WarekiYear) - 1).ToString();
                }

                // 元年表記にする
                wareki = (wareki.isBetween("0", "1")) ? "元年度" : wareki + "年度";
            }
            else
            {
                // 元年表記にする
                wareki = (this.WarekiYear == "1") ? "元年" : this.WarekiYear + "年";
            }

            switch (this.WarekiFormatType)
            {
                case WarekiFormat.None:
                case WarekiFormat.Wareki_JP:
                case WarekiFormat.Wareki_Zero_JP:
                case WarekiFormat.Wareki_Abbreviation_JP:
                case WarekiFormat.Wareki_Abbreviation_Zero_JP:
                case WarekiFormat.Wareki_Alphabet_JP:
                case WarekiFormat.Wareki_Alphabet_Zero_JP:
                default:
                    // 和暦で元号の範囲外の場合西暦を返す
                    if (this.TargetGengo == Gengo.None)
                    {
                        return this.TargetDate.ToString("yyyy年M月d日");
                    }
                    break;
            }


            switch (this.WarekiFormatType)
            {
                case WarekiFormat.None:
                    ret = "";
                    break;
                case WarekiFormat.Seireki_JP:
                    ret = this.TargetDate.ToString("yyyy年M月d日");
                    break;
                case WarekiFormat.Seireki_Zero_JP:
                    ret = this.TargetDate.ToString("yyyy年MM月dd日");
                    break;
                case WarekiFormat.Wareki_JP:
                    ret = this.GetGengoState(this.TargetGengo, DataType.GengoName) + wareki + this.TargetDate.ToString("M月d日");
                    break;
                case WarekiFormat.Wareki_Zero_JP:
                    ret = this.GetGengoState(this.TargetGengo, DataType.GengoName) + wareki + this.TargetDate.ToString("MM月dd日");
                    break;
                case WarekiFormat.Wareki_Abbreviation_JP:
                    ret = this.GetGengoState(this.TargetGengo, DataType.GengoAbbreviation) + wareki + this.TargetDate.ToString("M月d日");
                    break;
                case WarekiFormat.Wareki_Abbreviation_Zero_JP:
                    ret = this.GetGengoState(this.TargetGengo, DataType.GengoAbbreviation) + wareki + this.TargetDate.ToString("MM月dd日");
                    break;
                case WarekiFormat.Wareki_Alphabet_JP:
                    ret = this.GetGengoState(this.TargetGengo, DataType.GengoCharAlphabet) + wareki + this.TargetDate.ToString("M月d日");
                    break;
                case WarekiFormat.Wareki_Alphabet_Zero_JP:
                    ret = this.GetGengoState(this.TargetGengo, DataType.GengoCharAlphabet) + wareki + this.TargetDate.ToString("MM月dd日");
                    break;
                case WarekiFormat.Seireki_Dot:
                    ret = this.TargetDate.ToString("yyyy.M.d");
                    break;
                case WarekiFormat.Seireki_Zero_Dot:
                    ret = this.TargetDate.ToString("yyyy.MM.dd");
                    break;
                case WarekiFormat.Seireki_Slash:
                    ret = this.TargetDate.ToString("yyyy/M/d");
                    break;
                case WarekiFormat.Seireki_Zero_Slash:
                    ret = this.TargetDate.ToString("yyyy/MM/dd");
                    break;
                default:
                    ret = "";
                    break;
            }
            return ret;
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
        #endregion

    }
}
