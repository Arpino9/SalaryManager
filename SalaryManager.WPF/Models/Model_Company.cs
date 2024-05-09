using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
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

            this.ViewModel.Companies_ItemSource.Value = ListUtils.ToObservableCollection(Companies.FetchByAscending().ToList());

            if (this.ViewModel.BusinessCategory_Large_Text is null)
            {
                this.ViewModel.BusinessCategory_Large_Text.Value = this.ViewModel.BusinessCategory_Large_ItemsSource.Value.First().LargeName;

                this.BusinessCategory_Large_SelectionChanged();
            }
        }

        /// <summary>
        /// 業種 (大区分) - SelectionChanged
        /// </summary>
        public void BusinessCategory_Large_SelectionChanged()
        {
            var item = new BusinessCategoryValue(this.ViewModel.BusinessCategory_Large_Text.Value);
            
            if (this.ViewModel.BusinessCategory_Middle_Text.Value is null) 
            {
                item.MiddleList.Values.ToList().ForEach(x => this.ViewModel.BusinessCategory_Middle_ItemSource.Add(x));

                this.ViewModel.BusinessCategory_Middle_Text.Value = this.ViewModel.BusinessCategory_Middle_ItemSource.FirstOrDefault();
            }
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
            // 会社名
            this.ViewModel.CompanyName_Text.Value = string.Empty;

            // 郵便番号
            this.ViewModel.PostCode_Text.Value = string.Empty;

            // 住所
            this.ViewModel.Address_Text.Value  = string.Empty;
            this.ViewModel.Address_Google_Text.Value = string.Empty;

            // 備考
            this.ViewModel.Remarks_Text.Value = string.Empty;

            // 追加ボタン
            this.ViewModel.Add_IsEnabled.Value = false;
            // 更新ボタン
            this.ViewModel.Update_IsEnabled.Value = false;
            // 削除ボタン
            this.ViewModel.Delete_IsEnabled.Value = false;
        }

        /// <summary>
        /// 会社名 - TextChanged
        /// </summary>
        public void EnableAddButton()
        {
            var inputted = !string.IsNullOrEmpty(this.ViewModel.CompanyName_Text.Value) &&
                           !string.IsNullOrEmpty(this.ViewModel.Address_Google_Text.Value);

            this.ViewModel.Add_IsEnabled.Value = inputted;
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

            // 追加ボタン
            this.EnableAddButton();
            // 更新、削除ボタン
            this.EnableControlButton();
        }

        /// <summary> 会社名(更新前) </summary>
        private string CompanyName_Prev;
        
        /// <summary> 会社の住所(更新前) </summary>
        private string CompanyAddress_Prev;

        /// <summary>
        /// 会社一覧 - SelectionChanged
        /// </summary>
        //public async Task Companies_SelectionChanged()
        public void Companies_SelectionChanged()
        {
            if (this.ViewModel.Companies_SelectedIndex.Value == -1)
            {
                return;
            }

            this.EnableControlButton();

            if (!this.ViewModel.Companies_ItemSource.Value.Any())
            {
                return;
            }

            var entity = this.ViewModel.Companies_ItemSource.Value[this.ViewModel.Companies_SelectedIndex.Value];

            // 業種(大区分)
            this.ViewModel.BusinessCategory_Large_ItemsSource.Value = ListUtils.ToObservableCollection(BusinessCategoryValue.LargeCategory);
            this.ViewModel.BusinessCategory_Large_Text.Value  = entity.BusinessCategory.LargeName;

            // 業種(中区分)
            var item = new BusinessCategoryValue(this.ViewModel.BusinessCategory_Large_Text.Value);

            item.MiddleList.Values.ToList().ForEach(x => this.ViewModel.BusinessCategory_Middle_ItemSource.Add(x));

            this.ViewModel.BusinessCategory_Middle_Text.Value = entity.BusinessCategory.MiddleName;
            // 会社名
            this.ViewModel.CompanyName_Text.Value    = entity.CompanyName;
            this.CompanyName_Prev              = entity.CompanyName;
            this.CompanyAddress_Prev           = entity.Address_Google;

            // 郵便番号
            this.ViewModel.PostCode_Text.Value       = entity.PostCode;
            // 住所
            this.ViewModel.Address_Text.Value        = entity.Address;
            // 住所(Google)
            this.ViewModel.Address_Google_Text.Value = entity.Address_Google;
            // 備考
            this.ViewModel.Remarks_Text.Value        = entity.Remarks;
        }

        /// <summary>
        /// Enable - 操作ボタン
        /// </summary>
        /// <remarks>
        /// 追加ボタンは「会社名」に値があれば押下可能。
        /// </remarks>
        private void EnableControlButton()
        {
            var selected = this.ViewModel.Companies_ItemSource.Value.Any()
                        && this.ViewModel.Companies_SelectedIndex.Value >= 0;

            // 更新ボタン
            this.ViewModel.Update_IsEnabled.Value = selected;
            // 削除ボタン
            this.ViewModel.Delete_IsEnabled.Value = selected;
        }

        /// <summary>
        /// リロード
        /// </summary>
        public void Reload()
        {
            using (var cursor = new CursorWaiting())
            {
                Companies.Create(_repository);

                this.ViewModel.Companies_ItemSource.Value = ListUtils.ToObservableCollection(Companies.FetchByDescending().ToList());

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
                foreach (var entity in this.ViewModel.Companies_ItemSource.Value)
                {
                    _repository.Save(transaction, entity);
                    _repository.SaveAddress(transaction, entity);
                }

                transaction.Commit();
            }

            this.AddtionalUpdate();

            this.Reload();
        }

        /// <summary> Model </summary>
        public Model_WorkingPlace Model_WorkingPlace { get; set; } = Model_WorkingPlace.GetInstance(new WorkingPlaceSQLite());

        /// <summary>
        /// 追加更新
        /// </summary>
        public void AddtionalUpdate()
        {
            IWorkingPlaceRepository workingPlaceRepository = new WorkingPlaceSQLite();

            if (string.IsNullOrEmpty(this.CompanyName_Prev) == false &&
                string.IsNullOrEmpty(this.ViewModel.CompanyName_Text.Value) == false &&
                this.CompanyName_Prev != this.ViewModel.CompanyName_Text.Value)
            {
                // 会社名が変更された場合
                this.Model_WorkingPlace.UpdateCompanyName(this.CompanyName_Prev, this.ViewModel.CompanyName_Text.Value);

                using (var transaction = new SQLiteTransaction())
                {
                    workingPlaceRepository.UpdateCompanyName(transaction, CompanyName_Prev, this.ViewModel.CompanyName_Text.Value);
                    
                    transaction.Commit();
                }
            }

            if (string.IsNullOrEmpty(this.CompanyAddress_Prev) == false &&
                string.IsNullOrEmpty(this.ViewModel.Address_Google_Text.Value) == false &&
                this.CompanyAddress_Prev != this.ViewModel.Address_Google_Text.Value)
            {
                // 会社の住所が変更された場合
                workingPlaceRepository.UpdateCompanyAddress(this.CompanyAddress_Prev, this.ViewModel.Address_Google_Text.Value);
            }
        }

        /// <summary>
        /// 追加
        /// </summary>
        public void Add()
        {
            if (!Message.ShowConfirmingMessage($"入力された会社情報を追加しますか？", this.ViewModel.Window_Title.Value))
            {
                // キャンセル
                return;
            }

            using (var cursor = new CursorWaiting())
            {
                this.ViewModel.Delete_IsEnabled.Value = true;

                var id = this.ViewModel.Companies_ItemSource.Value.Any() ? 
                         this.ViewModel.Companies_ItemSource.Value.Max(x => x.ID) + 1 : 1;

                var entity = this.CreateEntity(id);

                this.ViewModel.Companies_ItemSource.Value.Add(entity);
                this.Save();

                // 並び変え
                this.ViewModel.Companies_ItemSource.Value = new ObservableCollection<CompanyEntity>(this.ViewModel.Companies_ItemSource.Value.OrderByDescending(x => x.ID));

                this.ViewModel.Companies_SelectedIndex.Value = this.ViewModel.Companies_ItemSource.Value.Count;
            }
        }

        /// <summary>
        /// Create Entity
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>職歴</returns>
        private CompanyEntity CreateEntity(int id)
        {
            var item = new BusinessCategoryValue(this.ViewModel.BusinessCategory_Large_Text.Value);
            var middleNo = item.GetMiddleCategoryKey(this.ViewModel.BusinessCategory_Middle_Text.Value);

            return new CompanyEntity(
                id,
                int.Parse(middleNo),
                this.ViewModel.CompanyName_Text.Value,
                this.ViewModel.PostCode_Text.Value,
                this.ViewModel.Address_Text.Value,
                this.ViewModel.Address_Google_Text.Value,
                this.ViewModel.Remarks_Text.Value);
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void Update()
        {
            if (!Message.ShowConfirmingMessage($"選択中の会社情報を更新しますか？", this.ViewModel.Window_Title.Value))
            {
                // キャンセル
                return;
            }

            using (var cursor = new CursorWaiting())
            {
                var id = this.ViewModel.Companies_ItemSource.Value[this.ViewModel.Companies_SelectedIndex.Value].ID;

                var entity = this.CreateEntity(id);
                this.ViewModel.Companies_ItemSource.Value[this.ViewModel.Companies_SelectedIndex.Value] = entity;

                this.Save();
            }
        }

        /// <summary>
        /// 削除
        /// </summary>
        public void Delete()
        {
            if (this.ViewModel.Companies_SelectedIndex.Value == -1 ||
                !this.ViewModel.Companies_ItemSource.Value.Any())
            {
                return;
            }

            if (!Message.ShowConfirmingMessage($"選択中の会社情報を削除しますか？", this.ViewModel.Window_Title.Value))
            {
                // キャンセル
                return;
            }

            using (var cursor = new CursorWaiting())
            {
                var id = this.ViewModel.Companies_ItemSource.Value[this.ViewModel.Companies_SelectedIndex.Value].ID;

                _repository.Delete(id);

                this.ViewModel.Companies_ItemSource.Value.RemoveAt(this.ViewModel.Companies_SelectedIndex.Value);

                this.Reload();
                this.EnableControlButton();
            }
        }
    }
}
