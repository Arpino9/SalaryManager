/// <summary>
/// Salary Manager
/// </summary>
namespace SalaryManager
{
    /// <summary>
    /// Mouse Cursor Change Waiting
    /// </summary>
    public class MouseCursorChangeWaiting :
        // Disposable
        System.IDisposable
    {
		#region Mutex

		/// <summary>
		/// Mutex
		/// </summary>
		static private object mutex = 0;

		#endregion

		#region Constructor

		/// <summary>
		/// Constructor
		/// </summary>
		public MouseCursorChangeWaiting()
		{
			// Lock
			lock (mutex)
			{
				// Count
				int count = (int)mutex;

				// Top ?
				if (count <= 0)
				{
					// Mouse - Override Cursor = Wait
					System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
				}

				// increment Count
				++count;

				// restore Mutex
				mutex = count;
			}
		}

		#endregion

		#region Dispose

		/// <summary>
		/// Dispose
		/// </summary>
		public void Dispose()
		{
			// Lock
			lock (mutex)
			{
				// Count
				int count = (int)mutex;

				// decrement Count
				--count;

				// Top ?
				if (count <= 0)
				{
					// Mouse - Override Cursor = Arrow
					System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Arrow;
				}

				// restore Mutex
				mutex = count;
			}
		}

		#endregion
	}
}
