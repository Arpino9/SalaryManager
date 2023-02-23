using System.Drawing;
/// <summary>
/// Utils
/// </summary>
namespace SalaryManager
{
    /// <summary>
    /// ユーティリティ
    /// </summary>
    public class Utils
    {
        #region DatabaseTransaction

        /// <summary>
        /// データベース接続
        /// </summary>
        public DatabaseTransaction DatabaseTransaction
        {
            get;
            set;
        } = new DatabaseTransaction();
        
        /// <summary>
        /// 給与明細
        /// </summary>
        private DataSetPayslip DataSetPayslip
        {
            get;
            set;
        } = new DataSetPayslip();

        #endregion

        #region 日付

        /// <summary>
        /// 日付
        /// </summary>
        public string TargetDate
        {
            get;
            set;
        }

        #endregion

        #region 金額 → 数値

        /// <summary>
        /// 金額 → 数値
        /// </summary>
        /// <param name="strValue">判定値</param>
        /// <returns>price</returns>
        /// <remarks>金額から数値に変換する</remarks>
        public int FormatPriceToInt(
            // 判定値
            string strValue
        )
        {
            int price;

            // strValue is Null ?
            if (string.IsNullOrEmpty(strValue))
            {
                // 無効
                return 0;
            }

            // strValue - 1桁目 - マイナス ?
            if (strValue.Substring(0, 1) != "-")
            {
                int.TryParse(strValue, System.Globalization.NumberStyles.AllowThousands, null, out price);

                return price;
            }

            // -をとる
            strValue = strValue.Replace("-", string.Empty);

            // 数値変換
            if (int.TryParse(strValue, System.Globalization.NumberStyles.AllowThousands, null, out price))
            {
                // price = 正数
                price = price - (price * 2);
            }

            return price;
        }

        #endregion

        #region 数値 → 金額

        /// <summary>
        /// 数値から金額に変換する
        /// </summary>
        /// <param name="value">判定値</param>
        /// <returns>カンマ区切りの金額</returns>
        public string FormatIntToPrice(string value)
        {
            // value = 3桁以下 ?
            if (value.Length <= 3)
            {
                // 無効
                return value;
            }

            // value is int ?
            if (!int.TryParse(value, out int outPrice))
            {
                // 無効
               return value;
            }

            // Format
            return System.String.Format("{0:#,0}", System.Convert.ToInt32(value));
        }

        #endregion

        #region キー入力判定

        /// <summary>
        /// キー入力判定
        /// </summary>
        /// <param name="e">KeyPress Event Args</param>
        public void IsInputValNumeric(
            // KeyPress Event Args
            System.Windows.Forms.KeyPressEventArgs e
        )
        {
            // Event - KeyChar = - ?
            if (e.KeyChar == '-')
            {
                // 無効
                return;
            }

            // Event - KeyChar = BackSpace ?
            if (e.KeyChar == '\b')
            {
                // 無効
                return;
            }

            // Event - KeyChar = 1 ～ 8 ?
            if (e.KeyChar < '0' || '9' < e.KeyChar)
            {
                // Event Handled = True
                e.Handled = true;
            }
        }

        #endregion

        #region 値の更新

        /// <summary>
        /// 色変更
        /// </summary>
        /// <param name="targetText">対象のテキスト</param>
        /// <param name="price">対象金額</param>
        /// <param name="key">取り出したいキー</param>
        public bool ChangeBackgroundColor(
            // 対象のテキスト
            System.Windows.Forms.Control targetText,
            // 色変更するか
            bool isChanged
        )
        {
            if (isChanged)
            {
                // 変更あり
                targetText.BackColor = System.Drawing.Color.Pink;

                return true;
            }

            // 変更なし
            targetText.BackColor = System.Drawing.Color.FromKnownColor(System.Drawing.KnownColor.Window);

            return false;
        }

        #endregion

        #region 0を空白にする

        /// <summary>
        /// 0を空白にする(int)
        /// </summary>
        /// <param name="targetText">対象のテキスト</param>
        /// <param name="price">対象金額</param>
        public void ZeroToBlank(
            // 対象のテキスト
            System.Windows.Forms.Control targetText,
            // 対象金額
            int price
        )
        {
            if (price.ToString() == "0")
            {
                // Text = 空
                targetText.Text = string.Empty;
            }
        }

        #endregion

    }
}
