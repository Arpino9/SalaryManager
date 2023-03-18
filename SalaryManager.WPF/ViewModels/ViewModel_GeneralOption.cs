using SalaryManager.WPF.Converter;
using SalaryManager.WPF.Models;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SalaryManager.WPF.ViewModels
{
    public class ViewModel_GeneralOption : INotifyPropertyChanged
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

        public ViewModel_GeneralOption()
        {
            this.Model.ViewModel = this;
            this.Model.Initialize();
        }

        /// <summary> Model - 支給額 </summary>
        public Model_Option Model = Model_Option.GetInstance();

        #region SQLite選択

        private string _selectSQLite_Text;

        /// <summary>
        /// Excelテンプレートのパス - Value
        /// </summary>
        public string ExcelTemplatePath_Text
        {
            get => this._selectSQLite_Text;
            set
            {
                this._selectSQLite_Text = value;
                this.RaisePropertyChanged();
            }
        }

        private RelayCommand _selectExcelTempletePath_Command;

        /// <summary>
        /// Excelテンプレートパスの指定 - Command
        /// </summary>
        public RelayCommand SelectExcelTemplatePath_Command
        {
            get
            {
                if (this._selectExcelTempletePath_Command == null)
                {
                    this._selectExcelTempletePath_Command = new RelayCommand(this.Model.SelectExcelTemplatePath);
                }
                return this._selectExcelTempletePath_Command;
            }
        }

        private RelayCommand _setDefault_SelectExcelTemplatePath_Command;

        public RelayCommand SetDefault_SelectExcelTemplatePath_Command
        {
            get
            {
                if (this._setDefault_SelectExcelTemplatePath_Command == null)
                {
                    this._setDefault_SelectExcelTemplatePath_Command = new RelayCommand(this.Model.SetDefault_SelectExcelTemplatePath);
                }
                return this._setDefault_SelectExcelTemplatePath_Command;
            }
        }

        #endregion

        #region 保存

        private RelayCommand _save_Command;

        /// <summary>
        /// Excelテンプレートパスの指定 - Command
        /// </summary>
        public RelayCommand Save_Command
        {
            get
            {
                if (this._save_Command == null)
                {
                    this._save_Command = new RelayCommand(this.Model.Save);
                }
                return this._save_Command;
            }
        }

        #endregion

    }
}
