﻿using SalaryManager.WPF.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SalaryManager.WPF.ViewModels
{
    public class ViewModel_PDFOption : INotifyPropertyChanged
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

        public ViewModel_PDFOption()
        {
            this.Model.PDFOption = this;

            this.Model.Initialize_PDF();
        }

        /// <summary> Model - オプション </summary>
        public Model_Option Model = Model_Option.GetInstance();

        #region パスワード

        private string _password_Text;

        /// <summary>
        /// パスワード - Text
        /// </summary>
        public string Password_Text
        {
            get => this._password_Text;
            set
            {
                this._password_Text = value;
                this.RaisePropertyChanged();
            }
        }

        private char _password_PasswordChar = '⚫';

        /// <summary>
        /// パスワード - PasswordChar
        /// </summary>
        public char Password_PasswordChar
        {
            get => this._password_PasswordChar;
            set
            {
                this._password_PasswordChar = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region パスワード表示/非表示ボタン

        private bool _password_IsVisible;

        /// <summary>
        /// パスワード - Text
        /// </summary>
        public bool Password_IsVisible
        {
            get => this._password_IsVisible;
            set
            {
                this._password_IsVisible = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

    }
}