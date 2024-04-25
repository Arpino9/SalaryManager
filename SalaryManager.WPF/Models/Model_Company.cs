using System.Collections.ObjectModel;
using System.Linq;
using SalaryManager.Domain.Entities;
using SalaryManager.Domain.Modules.Helpers;
using SalaryManager.Domain.Modules.Logics;
using SalaryManager.Domain.Repositories;
using SalaryManager.Domain.StaticValues;
using SalaryManager.Domain.ValueObjects;
using SalaryManager.Infrastructure.Interface;
using SalaryManager.Infrastructure.SQLite;
using SalaryManager.WPF.ViewModels;

namespace SalaryManager.WPF.Models
{
    /// <summary>
    /// Model - 会社マスタ
    /// </summary>
    public class Model_Company : IMaster
    {
        #region Get Instance

        private static Model_Company model = null;

        public static Model_Company GetInstance(ICompanyRepository repository)
        {
            if (model == null)
            {
                model = new Model_Company(repository);
            }

            return model;
        }

        #endregion

        private ICompanyRepository _repository;

        public Model_Company(ICompanyRepository repository)
        {
            this._repository = repository;
        }

        /// <summary>
        /// 初期化
        /// </summary>
        /// <remarks>
        /// 未選択状態、かつ新規登録が可能な状態にする。
        /// </remarks>
        public void Initialize()
        {
            Companies.Create(_repository);

            this.ViewModel.Companies_ItemSource = ListUtils.ToObservableCollection(Companies.FetchByAscending().ToList());

            if (this.ViewModel.BusinessCategory_Large_SelectedItem is null)
            {
                this.ViewModel.BusinessCategory_Large_SelectedItem = this.ViewModel.BusinessCategory_Large_ItemsSource.First().LargeName;
                this.ViewModel.BusinessCategory_Large_Text         = this.ViewModel.BusinessCategory_Large_SelectedItem;

                this.BusinessCategory_Large_SelectionChanged();
            }
        }

        /// <summary>
        /// 業種 (大区分) - SelectionChanged
        /// </summary>
        public void BusinessCategory_Large_SelectionChanged()
        {
            var item = new BusinessCategoryValue(this.ViewModel.BusinessCategory_Large_SelectedItem);
            this.ViewModel.BusinessCategory_Middle_ItemSource = ListUtils.ToObservableCollection(item.MiddleList.Values.ToList());

            if (this.ViewModel.BusinessCategory_Middle_SelectedItem is null) 
            {
                this.ViewModel.BusinessCategory_Middle_SelectedItem = this.ViewModel.BusinessCategory_Middle_ItemSource.First();
                this.ViewModel.BusinessCategory_Middle_Text         = this.ViewModel.BusinessCategory_Middle_SelectedItem;
            }
            
            this.ViewModel.BusinessCategory_Middle_SelectedIndex = 0;
            
            this.ViewModel.BusinessCategory_MiddleNo = item.GetMiddleCategoryKey(this.ViewModel.BusinessCategory_Middle_SelectedItem);
        }

        /// <summary>
        /// 業種 (中区分) - SelectionChanged
        /// </summary>
        public void BusinessCategory_Middle_SelectionChanged()
        {
            var item = new BusinessCategoryValue(this.ViewModel.BusinessCategory_Large_SelectedItem);
            this.ViewModel.BusinessCategory_MiddleNo = item.GetMiddleCategoryKey(this.ViewModel.BusinessCategory_Middle_SelectedItem);
        }

        /// <summary> ViewModel - 職歴 </summary>
        public ViewModel_Company ViewModel { get; set; }

        /// <summary>
        /// クリア
        /// </summary>
        /// <remarks>
        /// 各項目を初期化する。
        /// </remarks>
        public void Clear_InputForm()
        {
            // 業種
            this.ViewModel.BusinessCategory_Large_SelectedIndex  = 0;
            this.ViewModel.BusinessCategory_Middle_SelectedIndex = 0;

            // 会社名
            this.ViewModel.CompanyName_Text = string.Empty;

            // 郵便番号
            this.ViewModel.PostCode_Text = string.Empty;

            // 住所
            this.ViewModel.Address_Text        = string.Empty;
            this.ViewModel.Address_Google_Text = string.Empty;

            // 備考
            this.ViewModel.Remarks_Text = string.Empty;

            // 追加ボタン
            this.ViewModel.Add_IsEnabled = false;
            // 更新ボタン
            this.ViewModel.Update_IsEnabled = false;
            // 削除ボタン
            this.ViewModel.Delete_IsEnabled = false;
        }

        /// <summary>
        /// 会社名 - TextChanged
        /// </summary>
        public void EnableAddButton()
        {
            var inputted = !string.IsNullOrEmpty(this.ViewModel.CompanyName_Text) &&
                           !string.IsNullOrEmpty(this.ViewModel.Address_Google_Text);

            this.ViewModel.Add_IsEnabled = inputted;
        }

        /// <summary>
        /// 再描画
        /// </summary>
        /// <remarks>
        /// 各項目を再描画する。
        /// </remarks>
        public void Refresh()
        {
            this.Reflesh_InputForm();
        }

        /// <summary>
        /// 再描画 - 入力用フォーム
        /// </summary>
        private void Reflesh_InputForm()
        {
            this.BusinessCategory_Large_SelectionChanged();
            this.BusinessCategory_Middle_SelectionChanged();

            // 追加ボタン
            this.EnableAddButton();
            // 更新、削除ボタン
            this.EnableControlButton();
        }

        /// <summary>
        /// 経歴 - SelectionChanged
        /// </summary>
        public void Companies_SelectionChanged()
        {
            if (this.ViewModel.Companies_SelectedIndex == -1)
            {
                return;
            }

            this.EnableControlButton();

            if (!this.ViewModel.Companies_ItemSource.Any())
            {
                return;
            }

            var entity = this.ViewModel.Companies_ItemSource[this.ViewModel.Companies_SelectedIndex];

            // 業種(大区分)
            this.ViewModel.BusinessCategory_Large_Text  = entity.BusinessCategory.LargeName;
            // 業種(中区分)
            this.ViewModel.BusinessCategory_Middle_Text = entity.BusinessCategory.MiddleName;
            // 会社名
            this.ViewModel.CompanyName_Text    = entity.CompanyName;
            // 郵便番号
            this.ViewModel.PostCode_Text       = entity.PostCode;
            // 住所
            this.ViewModel.Address_Text        = entity.Address;
            // 住所(Google)
            this.ViewModel.Address_Google_Text = entity.Address_Google;
            // 備考
            this.ViewModel.Remarks_Text        = entity.Remarks;
        }

        /// <summary>
        /// Enable - 操作ボタン
        /// </summary>
        /// <remarks>
        /// 追加ボタンは「会社名」に値があれば押下可能。
        /// </remarks>
        private void EnableControlButton()
        {
            var selected = this.ViewModel.Companies_ItemSource.Any()
                        && this.ViewModel.Companies_SelectedIndex >= 0;

            // 更新ボタン
            this.ViewModel.Update_IsEnabled = selected;
            // 削除ボタン
            this.ViewModel.Delete_IsEnabled = selected;
        }

        /// <summary>
        /// リロード
        /// </summary>
        public void Reload()
        {
            using (var cursor = new CursorWaiting())
            {
                Companies.Create(_repository);

                this.ViewModel.Companies_ItemSource = ListUtils.ToObservableCollection(Companies.FetchByDescending().ToList());

                this.Refresh();
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        public void Save()
        {
            using (var transaction = new SQLiteTransaction())
            {
                foreach (var entity in this.ViewModel.Companies_ItemSource)
                {
                    _repository.Save(transaction, entity);
                    _repository.SaveAddress(transaction, entity);
                }

                transaction.Commit();
            }

            this.Reload();
        }

        /// <summary>
        /// 追加
        /// </summary>
        public void Add()
        {
            if (!Message.ShowConfirmingMessage($"入力された会社情報を追加しますか？", this.ViewModel.Title))
            {
                // キャンセル
                return;
            }

            using (var cursor = new CursorWaiting())
            {
                this.ViewModel.Delete_IsEnabled = true;

                var id = this.ViewModel.Companies_ItemSource.Any() ? 
                         this.ViewModel.Companies_ItemSource.Max(x => x.ID) + 1 : 1;

                var entity = this.CreateEntity(id);

                this.ViewModel.Companies_ItemSource.Add(entity);
                this.Save();

                // 並び変え
                this.ViewModel.Companies_ItemSource = new ObservableCollection<CompanyEntity>(this.ViewModel.Companies_ItemSource.OrderByDescending(x => x.ID));

                this.ViewModel.Companies_SelectedIndex = this.ViewModel.Companies_ItemSource.Count;
            }
        }

        /// <summary>
        /// Create Entity
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>職歴</returns>
        private CompanyEntity CreateEntity(int id)
        {
            return new CompanyEntity(
                id,
                int.Parse(this.ViewModel.BusinessCategory_MiddleNo),
                this.ViewModel.CompanyName_Text,
                this.ViewModel.PostCode_Text,
                this.ViewModel.Address_Text,
                this.ViewModel.Address_Google_Text,
                this.ViewModel.Remarks_Text);
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void Update()
        {
            if (!Message.ShowConfirmingMessage($"選択中の会社情報を更新しますか？", this.ViewModel.Title))
            {
                // キャンセル
                return;
            }

            using (var cursor = new CursorWaiting())
            {
                var id = this.ViewModel.Companies_ItemSource[this.ViewModel.Companies_SelectedIndex].ID;

                var entity = this.CreateEntity(id);
                this.ViewModel.Companies_ItemSource[this.ViewModel.Companies_SelectedIndex] = entity;

                this.Save();
            }
        }

        /// <summary>
        /// 削除
        /// </summary>
        public void Delete()
        {
            if (this.ViewModel.Companies_SelectedIndex == -1 ||
                !this.ViewModel.Companies_ItemSource.Any())
            {
                return;
            }

            if (!Message.ShowConfirmingMessage($"選択中の会社情報を削除しますか？", this.ViewModel.Title))
            {
                // キャンセル
                return;
            }

            using (var cursor = new CursorWaiting())
            {
                var id = this.ViewModel.Companies_ItemSource[this.ViewModel.Companies_SelectedIndex].ID;

                _repository.Delete(id);

                this.ViewModel.Companies_ItemSource.RemoveAt(this.ViewModel.Companies_SelectedIndex);

                this.Reload();
                this.EnableControlButton();
            }
        }
    }
}
