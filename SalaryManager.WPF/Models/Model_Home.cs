using SalaryManager.Domain.Entities;
using SalaryManager.Domain.Modules.Helpers;
using SalaryManager.Domain.Modules.Logics;
using SalaryManager.Domain.Repositories;
using SalaryManager.Domain.StaticValues;
using SalaryManager.Infrastructure.Interface;
using SalaryManager.WPF.ViewModels;
using System.Collections.ObjectModel;
using System.Linq;

namespace SalaryManager.WPF.Models
{
    public class Model_Home : IMaster
    {
        #region Get Instance

        private static Model_Home model = null;

        public Model_Home(IHomeRepository repository)
        {
            _repository = repository;
        }

        #endregion

        public IHomeRepository _repository { get; }

        public static Model_Home GetInstance(IHomeRepository repository)
        {
            if (model == null)
            {
                model = new Model_Home(repository);
            }

            return model;
        }

        /// <summary> ViewModel - 職歴 </summary>
        public ViewModel_Home ViewModel { get; set; }

        public void Clear_InputForm()
        {
            this.ViewModel.DisplayName_Text = string.Empty;
            this.ViewModel.Address_Text     = string.Empty;
            this.ViewModel.Remarks_Text     = string.Empty;
        }

        public void Initialize()
        {
            Homes.Create(_repository);

            this.ViewModel.Homes_ItemSource = ListUtils.ToObservableCollection(Homes.FetchByAscending().ToList());

            Homes_SelectionChanged();
        }

        public void Refresh()
        {
            Reflesh_InputForm();
        }

        /// <summary>
        /// 再描画 - 入力用フォーム
        /// </summary>
        private void Reflesh_InputForm()
        {
            // 追加ボタン
            this.EnableAddButton();
            // 更新、削除ボタン
            this.EnableControlButton();
        }

        /// <summary>
        /// 経歴 - SelectionChanged
        /// </summary>
        public void Homes_SelectionChanged()
        {
            if (this.ViewModel.Homes_SelectedIndex == -1)
            {
                return;
            }

            this.EnableControlButton();

            if (!this.ViewModel.Homes_ItemSource.Any())
            {
                return;
            }

            var entity = this.ViewModel.Homes_ItemSource[this.ViewModel.Homes_SelectedIndex];

            // 名称
            this.ViewModel.DisplayName_Text    = entity.DisplayName;
            // 住所
            this.ViewModel.Address_Text        = entity.Address;
            // 会社名
            this.ViewModel.Address_Google_Text = entity.Address_Google;
            // 郵便番号
            this.ViewModel.PostCode_Text       = entity.PostCode;
            // 備考
            this.ViewModel.Remarks_Text        = entity.Remarks;
        }

        /// <summary>
        /// 自宅 - TextChanged
        /// </summary>
        public void EnableAddButton()
        {
            var inputted = !string.IsNullOrEmpty(this.ViewModel.Address_Text);

            this.ViewModel.Add_IsEnabled = inputted;
        }

        public void Reload()
        {
            using (var cursor = new CursorWaiting())
            {
                Homes.Create(_repository);

                this.ViewModel.Homes_ItemSource = ListUtils.ToObservableCollection(Homes.FetchByDescending().ToList());

                this.Refresh();
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        public void Save()
        {
            foreach (var entity in this.ViewModel.Homes_ItemSource)
            {
                _repository.Save(entity);
            }

            this.Reload();
        }

        /// <summary>
        /// Create Entity
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>職歴</returns>
        private HomeEntity CreateEntity(int id)
        {
            return new HomeEntity(
                id,
                this.ViewModel.DisplayName_Text,
                this.ViewModel.PostCode_Text,
                this.ViewModel.Address_Text,
                this.ViewModel.Address_Google_Text,
                this.ViewModel.Remarks_Text);
        }

        /// <summary>
        /// Enable - 操作ボタン
        /// </summary>
        /// <remarks>
        /// 追加ボタンは「会社名」に値があれば押下可能。
        /// </remarks>
        private void EnableControlButton()
        {
            var selected = this.ViewModel.Homes_ItemSource.Any()
                        && this.ViewModel.Homes_SelectedIndex >= 0;

            // 更新ボタン
            this.ViewModel.Update_IsEnabled = selected;
            // 削除ボタン
            this.ViewModel.Delete_IsEnabled = selected;
        }

        /// <summary>
        /// 追加
        /// </summary>
        public void Add()
        {
            if (!Message.ShowConfirmingMessage($"入力された自宅情報を追加しますか？", this.ViewModel.Title))
            {
                // キャンセル
                return;
            }

            using (var cursor = new CursorWaiting())
            {
                this.ViewModel.Delete_IsEnabled = true;

                var id = this.ViewModel.Homes_ItemSource.Any() ?
                         this.ViewModel.Homes_ItemSource.Max(x => x.ID) + 1 : 1;

                var entity = this.CreateEntity(id);

                this.ViewModel.Homes_ItemSource.Add(entity);
                this.Save();

                // 並び変え
                this.ViewModel.Homes_ItemSource = new ObservableCollection<HomeEntity>(this.ViewModel.Homes_ItemSource.OrderByDescending(x => x.ID));

                this.ViewModel.Homes_SelectedIndex = this.ViewModel.Homes_ItemSource.Count;
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void Update()
        {
            if (!Message.ShowConfirmingMessage($"選択中の自宅情報を更新しますか？", this.ViewModel.Title))
            {
                // キャンセル
                return;
            }

            using (var cursor = new CursorWaiting())
            {
                var id = this.ViewModel.Homes_ItemSource[this.ViewModel.Homes_SelectedIndex].ID;

                var entity = this.CreateEntity(id);
                this.ViewModel.Homes_ItemSource[this.ViewModel.Homes_SelectedIndex] = entity;

                this.Save();
            }
        }

        /// <summary>
        /// 削除
        /// </summary>
        public void Delete()
        {
            if (this.ViewModel.Homes_SelectedIndex == -1 ||
                !this.ViewModel.Homes_ItemSource.Any())
            {
                return;
            }

            if (!Message.ShowConfirmingMessage($"選択中の職歴を削除しますか？", this.ViewModel.Title))
            {
                // キャンセル
                return;
            }

            using (var cursor = new CursorWaiting())
            {
                _repository.Delete(this.ViewModel.Homes_SelectedIndex + 1);

                this.ViewModel.Homes_ItemSource.RemoveAt(this.ViewModel.Homes_SelectedIndex);

                this.Reload();
                this.EnableControlButton();
            }

            Clear_InputForm();
        }
    }
}
