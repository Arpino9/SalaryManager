using System;
using System.Linq;
using SalaryManager.Domain.Entities;
using SalaryManager.WPF.ViewModels;
using SalaryManager.Domain.Modules.Logics;
using SalaryManager.Domain.StaticValues;
using SalaryManager.Infrastructure.Interface;
using SalaryManager.Infrastructure.XML;
using SalaryManager.Domain.Repositories;
using SalaryManager.Infrastructure.SQLite;
using SalaryManager.Domain.Modules.Helpers;
using System.Collections.Generic;

namespace SalaryManager.WPF.Models
{
    /// <summary>
    /// Model - 職歴
    /// </summary>
    public class Model_WorkingPlace : IMaster
    {
        #region Get Instance

        private static Model_WorkingPlace model = null;

        public static Model_WorkingPlace GetInstance(IWorkingPlaceRepository repository)
        {
            if (model == null)
            {
                model = new Model_WorkingPlace(repository);
            }

            return model;
        }

        #endregion

        /// <summary> Repository </summary>
        private IWorkingPlaceRepository _repository;

        public Model_WorkingPlace(IWorkingPlaceRepository repository)
        {
            _repository = repository;
        }

        /// <summary> ViewModel - 職歴 </summary>
        public ViewModel_WorkingPlace ViewModel { get; set; }

        /// <summary> エンティティ </summary>
        public IReadOnlyList<WorkingPlaceEntity> Entities { get; internal set; }

        /// <summary>
        /// 初期化
        /// </summary>
        /// <remarks>
        /// 未選択状態、かつ新規登録が可能な状態にする。
        /// </remarks>
        public void Initialize()
        {
            WorkingPlace.Create(_repository);
            Companies.Create(new CompanySQLite());
            Homes.Create(new HomeSQLite());

            var companies = Companies.FetchByDescending().ToList();

            if (companies.Any())
            {
                var companyNames = companies.Select(x => x.CompanyName).ToList();
                this.ViewModel.CompanyName_ItemSource  = companyNames.ToReactiveCollection();

                var workingPlace = companyNames.Union(Homes.FetchByDescending().Select(x => x.DisplayName)).ToList();
                this.ViewModel.WorkingPlace_ItemSource = workingPlace.ToReactiveCollection();
            }

            this.ViewModel.Window_FontFamily.Value = XMLLoader.FetchFontFamily();
            this.ViewModel.Window_FontSize.Value   = XMLLoader.FetchFontSize();

            this.ViewModel.Window_Background.Value = XMLLoader.FetchBackgroundColorBrush();

            this.Entities = WorkingPlace.FetchByDescending();

            this.Reflesh_ListView();

            this.ViewModel.WorkingPlaces_SelectedIndex.Value = -1;
            this.Clear_InputForm();

            this.EnableWaitingButton();
            this.IsWorking_Checked();
        }

        /// <summary>
        /// 再描画
        /// </summary>
        /// <remarks>
        /// 該当月に経歴情報が存在すれば、各項目を再描画する。
        /// </remarks>
        public void Refresh()
        {
            // ListView
            this.Reflesh_ListView();
            // 入力用フォーム
            this.Reflesh_InputForm();
        }

        /// <summary>
        /// 再描画 - ListView
        /// </summary>
        private void Reflesh_ListView()
        {
            if (this.Entities.IsEmpty())
            {
                this.Clear_InputForm();
                return;
            }

            this.Entities.ToReactiveCollection(this.ViewModel.WorkingPlaces_ItemSource);
        }

        /// <summary>
        /// Enable - 操作ボタン
        /// </summary>
        /// <remarks>
        /// 追加ボタンは「会社名」に値があれば押下可能。
        /// </remarks>
        private void EnableControlButton()
        {
            var selected = this.ViewModel.WorkingPlaces_ItemSource.Any() 
                        && this.ViewModel.WorkingPlaces_SelectedIndex.Value >= 0;

            // 更新ボタン
            this.ViewModel.Update_IsEnabled.Value = selected;
            // 削除ボタン
            this.ViewModel.Delete_IsEnabled.Value = selected;
        }

        /// <summary>
        /// 就業中 - Checked
        /// </summary>
        public void IsWorking_Checked()
        {
            if (this.ViewModel.IsWorking_IsChacked.Value)
            {
                this.ViewModel.WorkingEnd_SelectedDate.Value = DateTime.Now;
            }
        }

        /// <summary>
        /// 経歴 - SelectionChanged
        /// </summary>
        public void Careers_SelectionChanged()
        {
            if (this.ViewModel.WorkingPlaces_SelectedIndex.Value == -1)
            {
                return;
            }

            this.EnableControlButton();

            if (this.ViewModel.WorkingPlaces_ItemSource.IsEmpty())
            {
                return;
            }

            var entity = this.ViewModel.WorkingPlaces_ItemSource[this.ViewModel.WorkingPlaces_SelectedIndex.Value];
            
            // 派遣元会社名
            this.ViewModel.DispatchingCompanyName_Text.Value = entity.DispatchingCompany.Text;
            this.ViewModel.DispatchedCompanyName_Text.Value  = entity.DispatchedCompany.Text;
            // 会社名
            this.ViewModel.WorkingPlace_Name_Text.Value = entity.WorkingPlace_Name.Text;

            // 住所
            this.ViewModel.WorkingPlace_Address_Text.Value = entity.WorkingPlace_Address;

            this.ViewModel.WorkingStart_SelectedDate.Value = entity.WorkingStart;
            this.ViewModel.WorkingEnd_SelectedDate.Value   = entity.WorkingEnd;

            // 待機中
            this.ViewModel.IsWaiting_IsChacked.Value = entity.IsWaiting;
            // 就業中
            this.ViewModel.IsWorking_IsChacked.Value = entity.IsWorking;

            // 勤務開始時間(時)
            this.ViewModel.WorkingTime_Start_Hour_Text.Value   = entity.WorkingTime.Start.Hours;
            // 勤務開始時間(分)
            this.ViewModel.WorkingTime_Start_Minute_Text.Value = entity.WorkingTime.Start.Minutes;
            // 勤務終了時間(時)
            this.ViewModel.WorkingTime_End_Hour_Text.Value     = entity.WorkingTime.End.Hours;
            // 勤務終了時間(分)
            this.ViewModel.WorkingTime_End_Minute_Text.Value   = entity.WorkingTime.End.Minutes;

            // 昼休憩開始時間(時)
            this.ViewModel.LunchTime_Start_Hour_Text.Value   = entity.LunchTime.Start.Hours;
            // 昼休憩開始時間(分)
            this.ViewModel.LunchTime_Start_Minute_Text.Value = entity.LunchTime.Start.Minutes;
            // 昼休憩開始時間(時)
            this.ViewModel.LunchTime_End_Hour_Text.Value     = entity.LunchTime.End.Hours;
            // 昼休憩開始時間(分)
            this.ViewModel.LunchTime_End_Minute_Text.Value   = entity.LunchTime.End.Minutes;

            // 昼休憩開始時間(時)
            this.ViewModel.BreakTime_Start_Hour_Text.Value   = entity.BreakTime.Start.Hours;
            // 昼休憩開始時間(分)
            this.ViewModel.BreakTime_Start_Minute_Text.Value = entity.BreakTime.Start.Minutes;
            // 昼休憩開始時間(時)
            this.ViewModel.BreakTime_End_Hour_Text.Value     = entity.BreakTime.End.Hours;
            // 昼休憩開始時間(分)
            this.ViewModel.BreakTime_End_Minute_Text.Value   = entity.BreakTime.End.Minutes;

            // 備考
            this.ViewModel.Remarks_Text.Value = entity.Remarks;
        }

        /// <summary>
        /// 会社名 - TextChanged
        /// </summary>
        public void EnableAddButton()
        {
            var inputted = (!string.IsNullOrEmpty(this.ViewModel.WorkingPlace_Name_Text.Value) && 
                            !string.IsNullOrEmpty(this.ViewModel.WorkingPlace_Address_Text.Value));

            this.ViewModel.Add_IsEnabled.Value = inputted;
        }

        /// <summary>
        /// Enabled - 待機ボタン
        /// </summary>
        public void EnableWaitingButton()
        {
            var inputted = (!string.IsNullOrEmpty(this.ViewModel.DispatchedCompanyName_Text.Value) &&
                            !string.IsNullOrEmpty(this.ViewModel.DispatchingCompanyName_Text.Value) &&
                             this.ViewModel.DispatchedCompanyName_Text.Value == this.ViewModel.DispatchingCompanyName_Text.Value);

            this.ViewModel.IsWaiting_Visibility.Value = inputted ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
        }

        /// <summary>
        /// 就業場所名から住所検索
        /// </summary>
        public void SearchAddress()
        {
            var companies = Companies.FetchByDescending().ToList();
            if (companies.Any(x => x.CompanyName.Contains(this.ViewModel.WorkingPlace_Name_Text.Value)))
            {
                this.ViewModel.WorkingPlace_Address_Text.Value = companies.Where(x => x.CompanyName.Contains(this.ViewModel.WorkingPlace_Name_Text.Value))
                                                                    .Select(x => x.Address_Google).FirstOrDefault();
                return;
            }

            var homes = Homes.FetchByDescending().ToList();
            if (homes.Any(x => x.DisplayName.Contains(this.ViewModel.WorkingPlace_Name_Text.Value)))
            {
                this.ViewModel.WorkingPlace_Address_Text.Value = homes.Where(x => x.DisplayName.Contains(this.ViewModel.WorkingPlace_Name_Text.Value))
                                                                .Select(x => x.Address_Google).FirstOrDefault();
                return;
            }
        }

        /// <summary>
        /// 再描画 - 入力用フォーム
        /// </summary>
        private void Reflesh_InputForm()
        {
            this.Careers_SelectionChanged();

            // 追加ボタン
            this.EnableAddButton();
            // 更新、削除ボタン
            this.EnableControlButton();
        }

        /// <summary>
        /// リロード
        /// </summary>
        /// <remarks>
        /// 年月の変更時などに、該当月の項目を取得する。
        /// </remarks>
        public void Reload()
        {
            using (var cursor = new CursorWaiting())
            {
                WorkingPlace.Create(_repository);

                this.Entities = WorkingPlace.FetchByDescending();

                this.Refresh();
            }
        }

        /// <summary>
        /// クリア
        /// </summary>
        /// <remarks>
        /// 各項目を初期化する。
        /// </remarks>
        public void Clear_InputForm()
        {
            // 派遣元会社
            this.ViewModel.DispatchingCompanyName_Text.Value = default(string);
            // 派遣先会社
            this.ViewModel.DispatchedCompanyName_Text.Value  = default(string);

            // 会社名
            this.ViewModel.WorkingPlace_Name_Text.Value = default(string);
            // 住所
            this.ViewModel.WorkingPlace_Address_Text.Value      = default(string);

            this.ViewModel.WorkingStart_SelectedDate.Value = DateTime.Now;
            this.ViewModel.WorkingEnd_SelectedDate.Value   = DateTime.Now;

            this.ViewModel.IsWaiting_IsChacked.Value = false;
            this.ViewModel.IsWorking_IsChacked.Value = false;
            
            // 労働 - 開始 - 時
            this.ViewModel.WorkingTime_Start_Hour_Text.Value   = default(int);
            // 労働 - 開始 - 分
            this.ViewModel.WorkingTime_Start_Minute_Text.Value = default(int);
            // 労働 - 終了 - 時
            this.ViewModel.WorkingTime_End_Hour_Text.Value     = default(int);
            // 労働 - 終了 - 分
            this.ViewModel.WorkingTime_End_Minute_Text.Value   = default(int);
            
            // 昼休憩 - 開始 - 時
            this.ViewModel.LunchTime_Start_Hour_Text.Value   = default(int);
            // 昼休憩 - 開始 - 分
            this.ViewModel.LunchTime_Start_Minute_Text.Value = default(int);
            // 昼休憩 - 終了 - 時
            this.ViewModel.LunchTime_End_Hour_Text.Value     = default(int);
            // 昼休憩 - 終了 - 分
            this.ViewModel.LunchTime_End_Minute_Text.Value   = default(int);

            // 休憩 - 開始 - 時
            this.ViewModel.BreakTime_Start_Hour_Text.Value   = default(int);
            // 休憩 - 開始 - 分
            this.ViewModel.BreakTime_Start_Minute_Text.Value = default(int);
            // 休憩 - 終了 - 時
            this.ViewModel.BreakTime_End_Hour_Text.Value     = default(int);
            // 休憩 - 終了 - 分
            this.ViewModel.BreakTime_End_Minute_Text.Value   = default(int);

            // 備考
            this.ViewModel.Remarks_Text.Value = default(string);

            // 追加ボタン
            this.ViewModel.Add_IsEnabled.Value    = false;
            // 更新ボタン
            this.ViewModel.Update_IsEnabled.Value = false;
            // 削除ボタン
            this.ViewModel.Delete_IsEnabled.Value = false;
        }

        /// <summary>
        /// 追加
        /// </summary>
        public void Add()
        {
            if (!Message.ShowConfirmingMessage($"入力された就業場所を追加しますか？", this.ViewModel.Window_Title.Value))
            {
                // キャンセル
                return;
            }

            using (var cursor = new CursorWaiting())
            {
                this.ViewModel.Delete_IsEnabled.Value = true;

                var entity = this.CreateEntity(this.Entities.Count + 1);
                this.ViewModel.WorkingPlaces_ItemSource.Add(entity);
                this.Save();

                this.Reload();
            }
        }

        /// <summary>
        /// Create Entity
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>職歴</returns>
        private WorkingPlaceEntity CreateEntity(int id)
        {
            return new WorkingPlaceEntity(
                id,
                this.ViewModel.DispatchingCompanyName_Text.Value,
                this.ViewModel.DispatchedCompanyName_Text.Value,
                this.ViewModel.WorkingPlace_Name_Text.Value,
                this.ViewModel.WorkingPlace_Address_Text.Value,
                this.ViewModel.WorkingStart_SelectedDate.Value, 
                this.ViewModel.WorkingEnd_SelectedDate.Value,
                this.ViewModel.IsWaiting_IsChacked.Value,
                this.ViewModel.IsWorking_IsChacked.Value,
                (this.ViewModel.WorkingTime_Start_Hour_Text.Value, 
                 this.ViewModel.WorkingTime_Start_Minute_Text.Value),
                (this.ViewModel.WorkingTime_End_Hour_Text.Value,   
                 this.ViewModel.WorkingTime_End_Minute_Text.Value),
                (this.ViewModel.LunchTime_Start_Hour_Text.Value,  
                 this.ViewModel.WorkingTime_End_Minute_Text.Value),
                (this.ViewModel.LunchTime_End_Hour_Text.Value,     
                 this.ViewModel.LunchTime_End_Minute_Text.Value),
                (this.ViewModel.BreakTime_Start_Hour_Text.Value,   
               　this.ViewModel.BreakTime_Start_Minute_Text.Value),
                (this.ViewModel.BreakTime_End_Hour_Text.Value,     
                 this.ViewModel.BreakTime_End_Minute_Text.Value),
                this.ViewModel.Remarks_Text.Value);
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void Update()
        {
            if (!Message.ShowConfirmingMessage($"選択中の職歴を更新しますか？", this.ViewModel.Window_Title.Value))
            {
                // キャンセル
                return;
            }

            using (var cursor = new CursorWaiting())
            {
                var id = this.ViewModel.WorkingPlaces_ItemSource[this.ViewModel.WorkingPlaces_SelectedIndex.Value].ID;

                var entity = this.CreateEntity(id);
                this.ViewModel.WorkingPlaces_ItemSource[this.ViewModel.WorkingPlaces_SelectedIndex.Value] = entity;

                this.Save();
            }   
        }

        /// <summary>
        /// 削除
        /// </summary>
        public void Delete()
        {
            if (this.ViewModel.WorkingPlaces_SelectedIndex.Value == -1 ||
                this.ViewModel.WorkingPlaces_ItemSource.IsEmpty())
            {
                return;
            }

            if (!Message.ShowConfirmingMessage($"選択中の職歴を削除しますか？", this.ViewModel.Window_Title.Value))
            {
                // キャンセル
                return;
            }

            using (var cursor = new CursorWaiting())
            {
                _repository.Delete(this.ViewModel.WorkingPlaces_SelectedIndex.Value + 1);

                this.ViewModel.WorkingPlaces_ItemSource.RemoveAt(this.ViewModel.WorkingPlaces_SelectedIndex.Value);

                this.Reload();
                this.EnableControlButton();
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        public void Save()
        {
            foreach (var entity in this.ViewModel.WorkingPlaces_ItemSource)
            {
                _repository.Save(entity);
            }

            this.Reload();
        }

        public void UpdateCompanyName(string oldName, string newName)
        {
            using (var transaction = new SQLiteTransaction())
            {
                _repository.UpdateCompanyName(transaction, oldName, newName);

                transaction.Commit();
            }
        }

        public void UpdateCompanyAddress(string oldAddress, string newAddress)
        {
            _repository.UpdateCompanyAddress(oldAddress, newAddress);
        }
    }
}
