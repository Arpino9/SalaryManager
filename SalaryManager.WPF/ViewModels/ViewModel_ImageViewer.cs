using Reactive.Bindings;
using SalaryManager.Infrastructure.SQLite;
using SalaryManager.WPF.Models;
using System.Windows.Media;

namespace SalaryManager.WPF.ViewModels
{
    /// <summary>
    /// ViewModel - イメージビューアー
    /// </summary>
    public class ViewModel_ImageViewer
    {
        public ViewModel_ImageViewer()
        {
            this.Model.ViewModel_ImageViewer = this;
        }

        /// <summary> Model - 添付ファイル管理 </summary>
        public Model_FileStorage Model { get; set; } 
            = Model_FileStorage.GetInstance(new FileStorageSQLite());

        #region Window

        /// <summary> Window - Title </summary>
        public ReactiveProperty<string> Window_Title { get; }
            = new ReactiveProperty<string>("イメージビューワー");

        #endregion

        #region 画像

        /// <summary> 画像 - Height </summary>
        public ReactiveProperty<double> FileImage_Height { get; set; }
            = new ReactiveProperty<double>();

        /// <summary> 画像 - Width </summary>
        public ReactiveProperty<double> FileImage_Width { get; set; }
            = new ReactiveProperty<double>();

        /// <summary> 画像 - Image </summary>
        public ReactiveProperty<ImageSource> FileImage_Image { get; set; }
            = new ReactiveProperty<ImageSource>();

        #endregion

    }
}
