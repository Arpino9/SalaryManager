using MahApps.Metro.Controls;
using Button = System.Windows.Controls.Button;

namespace SalaryManager.Prism.Views;

/// <summary>
/// Interaction logic for Home
/// </summary>
public partial class Home : System.Windows.Controls.UserControl
{
    public Home()
    {
        InitializeComponent();
        this.Loaded += this.OnLoaded;
    }

    /// <summary>
    /// ロード時に画面中央配置とタイトルバードラッグを設定する。
    /// </summary>
    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (Window.GetWindow(this) is not MetroWindow metro) return;

        // タイトルバー子コントロールが Handled にする前に捕捉するためトンネリングイベントを使用
        metro.PreviewMouseLeftButtonDown += this.OnTitleBarDrag;
    }

    /// <summary>
    /// タイトルバー領域のドラッグでウィンドウを移動する。
    /// 最小化・最大化・×ボタンのクリックは除外する。
    /// </summary>
    private void OnTitleBarDrag(object sender, MouseButtonEventArgs e)
    {
        if (sender is not MetroWindow metro) return;
        if (e.GetPosition(metro).Y > metro.TitleBarHeight) return;
        if (IsDescendantOfButton(e.OriginalSource as DependencyObject)) return;

        metro.DragMove();
    }

    /// <summary>
    /// 指定要素またはその祖先に Button が含まれるか判定する。
    /// </summary>
    private static bool IsDescendantOfButton(DependencyObject? element)
    {
        while (element is not null)
        {
            if (element is Button) return true;
            element = VisualTreeHelper.GetParent(element);
        }
        return false;
    }
}
