using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace SalaryManager.Prism.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : MahApps.Metro.Controls.MetroWindow
{
    private const int WM_NCHITTEST = 0x0084;
    private const int HTCAPTION   = 2;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();
    }

    /// <inheritdoc/>
    protected override void OnSourceInitialized(EventArgs e)
    {
        base.OnSourceInitialized(e);

        // base (MetroWindow) がフックを登録した後に追加することで LIFO 順により先に呼ばれる
        var source = HwndSource.FromHwnd(new WindowInteropHelper(this).Handle);
        source?.AddHook(TitleBarWndProc);
    }

    /// <summary>
    /// タイトルバー領域の WM_NCHITTEST を横取りして HTCAPTION を返す。
    /// </summary>
    /// <remarks>
    /// MahApps.Metro の WindowChrome がキャプション領域のマウスイベントを
    /// WPF に渡さないため、DragMove() ではなく Win32 レベルで対処する。
    /// メニューやボタン上のクリックは従来通り動作させる。
    /// </remarks>
    private IntPtr TitleBarWndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        if (msg != WM_NCHITTEST) return IntPtr.Zero;

        var rawX = (short)(lParam.ToInt32() & 0xFFFF);
        var rawY = (short)((lParam.ToInt32() >> 16) & 0xFFFF);
        var local = PointFromScreen(new Point(rawX, rawY));

        if (local.Y < 0 || local.Y > TitleBarHeight || local.X < 0 || local.X > ActualWidth)
            return IntPtr.Zero;

        var hit = VisualTreeHelper.HitTest(this, local);
        if (hit?.VisualHit != null && ContainsInteractiveAncestor(hit.VisualHit))
            return IntPtr.Zero;

        handled = true;
        return new IntPtr(HTCAPTION);
    }

    /// <summary>
    /// 要素のビジュアルツリーを上方向に辿り、クリック処理を持つ要素が含まれるか調べる。
    /// </summary>
    private bool ContainsInteractiveAncestor(DependencyObject element)
    {
        var current = element;
        while (current != null && !ReferenceEquals(current, this))
        {
            if (current is System.Windows.Controls.Primitives.ButtonBase ||
            current is System.Windows.Controls.MenuItem ||
            current is System.Windows.Controls.Menu)
                return true;
            current = VisualTreeHelper.GetParent(current);
        }
        return false;
    }
}
