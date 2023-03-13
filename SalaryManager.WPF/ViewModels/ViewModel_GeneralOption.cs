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
        }

        /// <summary> Model - 支給額 </summary>
        public Model_Option Model { get; set; } = Model_Option.GetInstance();

        #region SQLite選択

        private string _selectSQLite_Text;

        /// <summary>
        /// 基本給 - Value
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

        #endregion

    }
}
