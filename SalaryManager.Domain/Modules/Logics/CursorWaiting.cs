using System;
using System.Windows.Forms;

namespace SalaryManager.Domain.Modules.Logics
{
    /// <summary>
    /// カーソル待ち
    /// </summary>
    /// <remarks>
    /// 簡易的な実装なので、必要があればコンストラクタのlock化を検討。
    /// </remarks>
    public class CursorWaiting : IDisposable
    {
        private Cursor _cursor;

        public CursorWaiting()
        {
            _cursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
        }

        public void Dispose()
        {
            Cursor.Current = _cursor;
        }
    }
}
