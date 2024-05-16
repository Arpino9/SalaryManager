namespace SalaryManager.WPF.Converter;

/// <summary>
/// Enum変換
/// </summary>
/// <remarks>
/// Reference: https://araramistudio.jimdo.com/2016/12/27/wpf%E3%81%A7radiobutton%E3%81%AEischecked%E3%81%AB%E5%88%97%E6%8C%99%E5%9E%8B%E3%82%92%E3%83%90%E3%82%A4%E3%83%B3%E3%83%89%E3%81%99%E3%82%8B/
/// </remarks>
public sealed class EnumToBoolConverter : IValueConverter
{
    // プロパティ値→ブール値へ変換する
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var parameterString = parameter as string;
        if (null == parameterString)
        {
            return DependencyProperty.UnsetValue;
        }
        
        if (!Enum.IsDefined(value.GetType(), value))
        {
            return DependencyProperty.UnsetValue;
        }
        
        var parameterValue = Enum.Parse(value.GetType(), parameterString);
        return parameterValue.Equals(value);
    }

    // ブール値→プロパティ値へ逆変換する
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var parameterString = parameter as string;
        if (null == parameterString)
        {
            return DependencyProperty.UnsetValue;
        }

        if (false.Equals(value))
        {               
            return DependencyProperty.UnsetValue;
        }
        
        return Enum.Parse(targetType, parameterString);
    }
}
