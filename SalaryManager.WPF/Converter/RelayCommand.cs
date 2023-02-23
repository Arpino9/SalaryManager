namespace SalaryManager.WPF.Converter
{
	/// <summary>
	/// その機能を中継することのみを目的とするコマンド
	/// デリゲートを呼び出すことにより、他のオブジェクトに対して呼び出します。
	/// CanExecute メソッドの既定の戻り値は 'true' です。
	/// <see cref="RaiseCanExecuteChanged"/> は、次の場合は必ず呼び出す必要があります。
	/// <see cref="CanExecute"/> は、別の値を返すことが予期されます。
	/// </summary>
	public class RelayCommand :
		// Command
		System.Windows.Input.ICommand
	{

		#region Constructor

		/// <summary>
		/// 常に実行可能な新しいコマンドを作成します。
		/// </summary>
		/// <param name="execute">実行ロジック。</param>
		public RelayCommand(
			// Execute
			System.Action execute
		) :
			this(
				// Execute
				execute,
				// Can Execute
				null
			)
		{
		}

		/// <summary>
		/// 新しいコマンドを作成します。
		/// </summary>
		/// <param name="execute">実行ロジック。</param>
		/// <param name="canExecute">実行ステータス ロジック。</param>
		public RelayCommand(
			// Execute
			System.Action execute,
			// Can Execute
			System.Func<bool> canExecute
		)
		{
			// Execute
			if (execute == null)
			{
				throw new System.ArgumentNullException("execute");
			}

			// Execute
			this._execute = execute;

			// Can Execute
			this._canExecute = canExecute;
		}

		#endregion

		#region Execute

		/// <summary>
		/// 実行ロジック
		/// </summary>
		private readonly System.Action _execute;

		/// <summary>
		/// 現在のコマンド ターゲットに対して <see cref="RelayCommand"/> を実行します。
		/// </summary>
		/// <param name="parameter">
		/// コマンドによって使用されるデータ。コマンドが、データの引き渡しを必要としない場合、このオブジェクトを null に設定できます。
		/// </param>
		public void Execute(
			// Parameter
			object parameter
		)
		{
			// Execute
			this._execute();
		}

		#endregion

		#region CanExecute

		/// <summary>
		/// 実行ステータス ロジック
		/// </summary>
		private readonly System.Func<bool> _canExecute;

		/// <summary>
		/// 現在の状態でこの <see cref="RelayCommand"/> が実行できるかどうかを判定します。
		/// </summary>
		/// <param name="parameter">
		/// コマンドによって使用されるデータ。コマンドが、データの引き渡しを必要としない場合、このオブジェクトを null に設定できます。
		/// </param>
		/// <returns>このコマンドが実行可能な場合は true、それ以外の場合は false。</returns>
		public bool CanExecute(
			// パラメータ
			object parameter
		)
		{
			// Can Execute
			var canExecute = this._canExecute;

			return (canExecute == null) ? true : canExecute();
		}

		#endregion

		#region CanExecuteChanged

		/// <summary>
		/// RaiseCanExecuteChanged が呼び出されたときに生成されます。
		/// </summary>
		public event System.EventHandler CanExecuteChanged;

		/// <summary>
		/// <see cref="CanExecuteChanged"/> イベントを発生させるために使用されるメソッド
		/// <see cref="CanExecute"/> の戻り値を表すために
		/// メソッドが変更されました。
		/// </summary>
		public void RaiseCanExecuteChanged()
		{
			// Invoke
			this.CanExecuteChanged?.Invoke(
				// Instance
				this,
				// Event Arguments
				System.EventArgs.Empty
			);
		}

		#endregion

	}
}
