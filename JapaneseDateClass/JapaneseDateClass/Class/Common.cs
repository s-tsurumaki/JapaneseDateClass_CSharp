using System;

namespace JapaneseDateClass.Class
{
    public static class Common
    {
        #region isBetween
        // 参考：ジェネリックと拡張メソッドを使ってみる
        // http://qiita.com/rohinomiya/items/1aa08c088a62f46d9fe1

        /// <summary>
        /// 数値 current がlower ～ higher の範囲内か？(Generic版)       
        /// </summary>
        /// <param name="lower">区間(開始)</param>
        /// <param name="current">比較される値</param>
        /// <param name="higher">区間(終了)</param>
        /// <param name="inclusive">閉区間か？(境界値を含む) (初期値:true)</param>
        /// <returns>範囲内であればtrue</returns>
        public static bool isBetween<T>(this T current, T lower, T higher, bool inclusive = true) where T : IComparable
        {
            // 拡張メソッドは1つ目の引数に this キーワードを付ける

            // ジェネリックだと比較演算子が使えなくなってしまうので、
            // where句 で型パラメーター T が IComparable<T> インターフェイスを実装するように指定
            // CompareTo() メソッドが使えるようになる。
            if (lower.CompareTo(higher) > 0) Swap(ref lower, ref higher);

            return inclusive ?
                (lower.CompareTo(current) <= 0 && current.CompareTo(higher) <= 0) :
                (lower.CompareTo(current) < 0 && current.CompareTo(higher) < 0);
        }

        /// <summary>
        /// ２つの値a,bを交換する
        /// </summary>
        /// <param name="a">値A</param>
        /// <param name="b">値B</param>
        public static void Swap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }
        #endregion
    }
}
