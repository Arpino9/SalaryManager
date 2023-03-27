using SalaryManager.WPF.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SalaryManager.WPF.ViewModels
{
    public class ViewModel_ImageViewer : INotifyPropertyChanged
    {

        #region Property Changed

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            var d = PropertyChanged;
            d?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        public ViewModel_ImageViewer()
        {
            this.Model.ViewModel_ImageViewer = this;
        }

        /// <summary> Model - 添付ファイル管理 </summary>
        public Model_FileStorage Model { get; set; } = Model_FileStorage.GetInstance();

        private double _fileImage_Height;

        /// <summary>
        /// 画像 - Height
        /// </summary>
        public double FileImage_Height
        {
            get => this._fileImage_Height;
            set
            {
                this._fileImage_Height = value;
                this.RaisePropertyChanged();
            }
        }

        private double _fileImage_Width;

        /// <summary>
        /// 画像 - Width
        /// </summary>
        public double FileImage_Width
        {
            get => this._fileImage_Width;
            set
            {
                this._fileImage_Width = value;
                this.RaisePropertyChanged();
            }
        }

        #region 画像

        private ImageSource _fileImage_Image;

        /// <summary>
        /// 画像 - Image
        /// </summary>
        public ImageSource FileImage_Image
        {
            get => this._fileImage_Image;
            set
            {
                this._fileImage_Image = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

    }
}
