using System;
using System.Windows.Forms;

namespace SalaryManager.Domain.Modules.Logics
{
    /// <summary>
    /// カーソル待ち
    /// </summary>
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
