﻿using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using TextBox = System.Windows.Controls.TextBox;

namespace SalaryManager.WPF.Behavior;

/// <summary>
/// TextBox用ビヘイビア
/// 整数値のみを入力許可
/// </summary>
/// <remarks>
/// Reference: https://hilapon.hatenadiary.org/entry/20101021/1287641423
/// </remarks>
public class TextBoxIntegerOnly
{

    /// <summary>
    /// True なら入力を数字のみに制限します。
    /// </summary>
    public static readonly DependencyProperty IsNumericProperty =
                DependencyProperty.RegisterAttached(
                    "IsNumeric", typeof(bool),
                    typeof(TextBoxIntegerOnly),
                    new UIPropertyMetadata(false, IsNumericChanged)
                );

    [AttachedPropertyBrowsableForType(typeof(TextBox))]
    public static bool GetIsNumeric(DependencyObject obj)
    {
        return (bool)obj.GetValue(IsNumericProperty);
    }

    [AttachedPropertyBrowsableForType(typeof(TextBox))]
    public static void SetIsNumeric(DependencyObject obj, bool value)
    {
        obj.SetValue(IsNumericProperty, value);
    }

    private static void IsNumericChanged
        (DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {

        var textBox = sender as TextBox;
        if (textBox == null) return;

        // イベントを登録・削除 
        textBox.KeyDown -= OnKeyDown;
        textBox.TextChanged -= OnTextChanged;
        var newValue = (bool)e.NewValue;
        if (newValue)
        {
            textBox.KeyDown += OnKeyDown;
            textBox.TextChanged += OnTextChanged;
        }
    }

    static void OnKeyDown(object sender, KeyEventArgs e)
    {
        var textBox = sender as TextBox;
        if (textBox == null) return;

        if ((Key.D0 <= e.Key && e.Key <= Key.D9) ||
            (Key.NumPad0 <= e.Key && e.Key <= Key.NumPad9) ||
            (Key.Delete == e.Key) || (Key.Back == e.Key) || (Key.Tab == e.Key))
        {
            e.Handled = false;
        }
        else
        {
            e.Handled = true;
        }
    }

    private static void OnTextChanged(object sender, TextChangedEventArgs e)
    {
        var textBox = sender as TextBox;
        if (textBox == null) return;

        if (string.IsNullOrEmpty(textBox.Text))
        {
            textBox.Text = "0";
        }
    }
}
