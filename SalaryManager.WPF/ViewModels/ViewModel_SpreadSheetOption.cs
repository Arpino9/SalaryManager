using System;
using System.Reactive.Linq;
using Reactive.Bindings;
using SalaryManager.WPF.Models;

namespace SalaryManager.WPF.ViewModels
{
    public class ViewModel_SpreadSheetOption
    {
        public ViewModel_SpreadSheetOption()
        {
            this.Model.SpreadSheetOption = this;

            this.Model.Initialize_SpreadSheet();

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
            this.SelectPrivateKey_Command.Subscribe(_ => this.Model.SelectPrivateKeyPath_SpreadSheet());
        }

        /// <summary> Model - オプション </summary>
        public Model_Option Model 
            = Model_Option.GetInstance();

        #region 認証ファイルの保存先パス

        /// <summary> 認証ファイルの保存先パス - Text </summary>
        public ReactiveProperty<string> SelectPrivateKey_Text { get; set; }
            = new ReactiveProperty<string>();

        /// <summary> 認証ファイルの保存先パス - Command </summary>
        public ReactiveCommand SelectPrivateKey_Command { get; private set; }
            = new ReactiveCommand();

        #endregion

        #region シートID

        /// <summary> シートID - Text </summary>
        public ReactiveProperty<string> SheetId_Text { get; set; }
            = new ReactiveProperty<string>();

        #endregion

    }
}
