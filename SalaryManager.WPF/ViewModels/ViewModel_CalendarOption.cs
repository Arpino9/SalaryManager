using System;
using System.Reactive.Linq;
using Reactive.Bindings;
using SalaryManager.WPF.Models;

namespace SalaryManager.WPF.ViewModels
{
    /// <summary>
    /// ViewModel - オプション - カレンダー
    /// </summary>
    public class ViewModel_CalendarOption
    {
        public ViewModel_CalendarOption()
        {
            this.Model.CalendarOption = this;

            this.Model.Initialize_Calendar();

            this.BindEvents();
        }

        /// <summary>
        /// イベント登録
        /// </summary>
        /// <remarks>
        /// Viewの指定したイベントと、発火させるメソッドを紐付ける。
        /// Subscribe()メソッドのオーバーロードが正しく呼ばれないので、
        /// 名前空間に「using System;」を必ず入れること。
        /// </remarks>
        private void BindEvents()
        {
            this.SelectPrivateKey_Command.Subscribe(_ => this.Model.SelectPrivateKeyPath_Calendar());
        }

        /// <summary> Model - オプション </summary>
        public Model_Option Model 
            = Model_Option.GetInstance();

        #region JSONの保存先パス

        /// <summary> 認証ファイルの保存先パス - Text </summary>
        public ReactiveProperty<string> SelectPrivateKey_Text { get; }
            = new ReactiveProperty<string>();

        /// <summary> 認証ファイルの保存先パス - Command </summary>
        public ReactiveCommand SelectPrivateKey_Command { get; private set; }
            = new ReactiveCommand();

        #endregion

        #region カレンダーID

        /// <summary> カレンダーIDの保存先パス - Text </summary>
        public ReactiveProperty<string> SelectCalendarID_Text { get; }
            = new ReactiveProperty<string>();

        #endregion

    }
}
