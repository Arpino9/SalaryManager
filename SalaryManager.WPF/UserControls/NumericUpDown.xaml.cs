using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SalaryManager.WPF.UserControls
{
    /// <summary>
    /// NumericUpDownButton.xaml の相互作用ロジック
    /// </summary>
    /// <remarks>
    /// Reference : https://gogowaten.hatenablog.com/entry/2020/06/24/235053
    /// ※ロジックは全てリファクタリング済
    /// </remarks>
    public partial class NumericUpDown : UserControl
    {
        public NumericUpDown()
        {
            InitializeComponent();
        }

        #region 入力制限

        /// <summary>
        /// スペースキー押下時は判定を受け付けない。
        /// </summary>
        private void CustomTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// 入力の制限、数字とハイフンとピリオドだけ通す
        /// </summary>
        private void CustomTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (IsValid() == false)
            {
                e.Handled = true;
                return;
            }

            bool IsValid()
            {
                // 入力済みTextBox
                var textBox = (TextBox)sender;
                // 入力中のText
                var inputtingText = e.Text;

                if (new Regex("[0-9.-]").IsMatch(inputtingText) == false)
                {
                    // 入力文字が数値とピリオド、ハイフン以外
                    return false;
                }

                if (inputtingText == "-")
                {
                    // ハイフン入力中
                    if (textBox.CaretIndex != 0)
                    {
                        // キャレット(カーソル)位置が先頭(0)じゃない
                        return false;
                    }

                    if (textBox.SelectedText != textBox.Text &&
                        textBox.Text.Contains("-"))
                    {
                        // 2つ目のハイフン入力
                        return false;
                    }
                }

                if (inputtingText == ".")
                {
                    // ピリオド入力中
                    if (textBox.Text.Contains("."))
                    {
                        // 2つ目
                        return false;
                    }
                }

                return true;
            }
        }

        /// <summary>
        /// フォーカス消失時、不自然な文字を削除
        /// </summary>
        private void CustomTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            //ピリオドの削除
            //先頭か末尾にあった場合は削除
            var tb = (TextBox)sender;
            string text = tb.Text;
            if (text.StartsWith(".") || text.EndsWith("."))
            {
                text = text.Replace(".", "");
            }

            // -. も変なのでピリオドだけ削除
            text = text.Replace("-.", "-");

            //数値がないのにハイフンやピリオドがあった場合は削除
            if (text == "-" || text == ".")
            {
                text = "";
            }

            tb.Text = text;
        }

        //
        private void CustomTextBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            //貼り付け無効
            if (e.Command == ApplicationCommands.Paste)
            {
                e.Handled = true;
            }
        }

        //focusしたときにテキストを全選択
        private void CustomTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var tb = sender as TextBox;
            tb.SelectAll();
        }

        //        | オールトの雲
        //http://ooltcloud.sakura.ne.jp/blog/201311/article_30013700.html
        //クリックしたときにテキストを全選択
        private void CustomTextBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var tb = sender as TextBox;
            if (tb.IsFocused == false)
            {
                tb.Focus();
                e.Handled = true;
            }
        }
        #endregion 入力制限

        #region 依存関係プロパティ

        //要の値
        public decimal Value
        {
            get { return (decimal)GetValue(CustomValueProperty); }
            set { SetValue(CustomValueProperty, value); }
        }

        public static readonly DependencyProperty CustomValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(decimal), typeof(NumericUpDown),
                new PropertyMetadata(0m, OnCustomValuePropertyChanged, CoerceCustomValue));

        //CustomValueの変更直後の動作
        private static void OnCustomValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //することない
        }

        //CustomValueの変更直前の動作、値の検証、矛盾があれば値を書き換えて解消
        //入力された値が下限値より小さい場合は下限値に書き換え
        //入力された値が上限値より大きい場合は上限値に書き換え
        private static object CoerceCustomValue(DependencyObject d, object basaValue)
        {
            var ud = (NumericUpDown)d;
            var m = (decimal)basaValue;
            if (m < ud.Minimum) m = ud.Minimum;
            if (m > ud.Maximum) m = ud.Maximum;
            return m;
        }

        //小変更値
        public decimal CustomSmallChange
        {
            get { return (decimal)GetValue(CustomSmallChangeProperty); }
            set { SetValue(CustomSmallChangeProperty, value); }
        }
        public static readonly DependencyProperty CustomSmallChangeProperty =
            DependencyProperty.Register(nameof(CustomSmallChange), typeof(decimal), typeof(NumericUpDown), new PropertyMetadata(1m));


        //大変更値
        public decimal CustomLargeChange
        {
            get { return (decimal)GetValue(CustomLargeChangeProperty); }
            set { SetValue(CustomLargeChangeProperty, value); }
        }
        public static readonly DependencyProperty CustomLargeChangeProperty =
            DependencyProperty.Register(nameof(CustomLargeChange), typeof(decimal), typeof(NumericUpDown), new PropertyMetadata(10m));

        /// <summary>
        /// 下限値
        /// </summary>
        public decimal Minimum
        {
            get { return (decimal)GetValue(CustomMinValueProperty); }
            set { SetValue(CustomMinValueProperty, value); }
        }
        public static readonly DependencyProperty CustomMinValueProperty =
            DependencyProperty.Register(nameof(Minimum), typeof(decimal), typeof(NumericUpDown),
                new PropertyMetadata(decimal.MinValue, OnCustomMinValuePropertyChanged, CoerceCustomMinValue));

        //PropertyChangedコールバック、プロパティ値変更"直後"に実行される
        //変更された下限値と今の値での矛盾を解消
        //変更された新しい下限値と、今の値(CustomValue)で矛盾が生じた(下限値 < 今の値)場合は、今の値を下限値に変更する
        private static void OnCustomMinValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ud = (NumericUpDown)d;
            var min = (decimal)e.NewValue;//変更後の新しい下限値
            if (min > ud.Value) ud.Value = min;
        }

        //値の検証と変更
        //CoerceValueコールバック、プロパティ値変更"直前"に実行される
        //設定された値を強制(Coerce)的に変更できるので、矛盾があれば変更して解消する
        //入力された下限値と、今の上限値で矛盾が生じる(下限値 > 上限値)場合は、下限値を上限値に書き換える
        private static object CoerceCustomMinValue(DependencyObject d, object baseValue)
        {
            var ud = (NumericUpDown)d;
            var min = (decimal)baseValue;//入力された下限値
            if (min > ud.Maximum) min = ud.Maximum;
            return min;
        }


        /// <summary>
        /// 上限値
        /// </summary>
        public decimal Maximum
        {
            get { return (decimal)GetValue(CustomMaxValueProperty); }
            set { SetValue(CustomMaxValueProperty, value); }
        }
        public static readonly DependencyProperty CustomMaxValueProperty =
            DependencyProperty.Register(nameof(Maximum), typeof(decimal), typeof(NumericUpDown),
                new PropertyMetadata(decimal.MaxValue, OnCustomMaxValuePropertyChanged, CoerceCustomMaxValue));

        //上限値の変更直後の動作。上限値より今の値が大きい場合は、今の値を上限値に変更する
        private static void OnCustomMaxValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ud = (NumericUpDown)d;
            var max = (decimal)e.NewValue;
            if (max < ud.Value)
            {
                ud.Value = max;
            }
        }

        //上限値変更直前の動作。入力された上限値が今の下限値より小さくなる場合は、上限値を下限値に書き換える
        private static object CoerceCustomMaxValue(DependencyObject d, object baseValue)
        {
            var ud = (NumericUpDown)d;
            var max = (decimal)baseValue;
            if (max < ud.Minimum)
            {
                max = ud.Minimum;
            }
            return max;
        }

        #endregion 依存関係プロパティ

        private void RepeatButtonUp_Click(object sender, RoutedEventArgs e)
        {
            Value += CustomSmallChange;
        }

        private void RepeatButtonDown_Click(object sender, RoutedEventArgs e)
        {
            Value -= CustomSmallChange;
        }

        private void RepeatButton_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta < 0) Value -= CustomLargeChange;
            else Value += CustomLargeChange;
        }

        private void CustomTextBox_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta < 0) Value -= CustomSmallChange;
            else Value += CustomSmallChange;
        }
    }
}
