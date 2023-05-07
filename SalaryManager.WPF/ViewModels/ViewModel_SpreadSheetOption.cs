using SalaryManager.WPF.Converter;
using SalaryManager.WPF.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SalaryManager.WPF.ViewModels
{
    public class ViewModel_SpreadSheetOption : INotifyPropertyChanged
    {
        #region Property Changed

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(
            [CallerMemberName] string propertyName = null)
        {
            var d = PropertyChanged;
            d?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        public ViewModel_SpreadSheetOption()
        {
            this.Model.SpreadSheetOption = this;

            this.Model.Initialize_SpreadSheet();
        }

        /// <summary> Model - オプション </summary>
        public Model_Option Model = Model_Option.GetInstance();

        #region 認証ファイルの保存先パス

        private string _selectPrivateKey_Text;

        /// <summary>
        /// 認証ファイルの保存先パス - Text
        /// </summary>
        public string SelectPrivateKey_Text
        {
            get => this._selectPrivateKey_Text;
            set
            {
                this._selectPrivateKey_Text = value;
                this.RaisePropertyChanged();
            }
        }

        private RelayCommand _selectPrivateKey_Command;

        /// <summary>
        /// 認証ファイルの保存先パス - Command
        /// </summary>
        /// <remarks>
        /// 開く
        /// </remarks>
        public RelayCommand SelectPrivateKey_Command
        {
            get
            {
                if (this._selectPrivateKey_Command == null)
                {
                    this._selectPrivateKey_Command = new RelayCommand(this.Model.SelectPrivateKeyPath);
                }
                return this._selectPrivateKey_Command;
            }
        }

        #endregion

        #region シートID

        private string _sheetId_Text;

        /// <summary>
        /// シートID - Text
        /// </summary>
        public string SheetId_Text
        {
            get => this._sheetId_Text;
            set
            {
                this._sheetId_Text = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

    }
}
