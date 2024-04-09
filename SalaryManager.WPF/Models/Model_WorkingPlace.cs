﻿using System.Linq;
using SalaryManager.Domain.Entities;
using SalaryManager.WPF.ViewModels;
using SalaryManager.Domain.Modules.Logics;
using SalaryManager.Domain.StaticValues;
using SalaryManager.Infrastructure.Interface;
using SalaryManager.Infrastructure.XML;
using SalaryManager.Domain.Repositories;

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

        /// <summary>
        /// 初期化
        /// </summary>
        /// <remarks>
        /// 未選択状態、かつ新規登録が可能な状態にする。
        /// </remarks>
        public void Initialize()
        {
            WorkingPlace.Create(_repository);

            this.ViewModel.FontFamily = XMLLoader.FetchFontFamily();
            this.ViewModel.FontSize   = XMLLoader.FetchFontSize();

            this.ViewModel.Window_Background = XMLLoader.FetchBackgroundColorBrush();

            this.ViewModel.Entities = WorkingPlace.FetchByDescending();

            this.Reflesh_ListView();

            this.ViewModel.WorkingPlaces_SelectedIndex = -1;
            this.Clear_InputForm();
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
            this.ViewModel.WorkingPlaces_ItemSource.Clear();

            var entities = this.ViewModel.Entities;

            if (!entities.Any())
            {
                this.Clear_InputForm();
                return;
            }

            foreach (var entity in entities)
            {
                this.ViewModel.WorkingPlaces_ItemSource.Add(entity);
            }
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
                        && this.ViewModel.WorkingPlaces_SelectedIndex >= 0;

            // 更新ボタン
            this.ViewModel.Update_IsEnabled = selected;
            // 削除ボタン
            this.ViewModel.Delete_IsEnabled = selected;
        }

        /// <summary>
        /// 経歴 - SelectionChanged
        /// </summary>
        public void Careers_SelectionChanged()
        {
            if (this.ViewModel.WorkingPlaces_SelectedIndex == -1)
            {
                return;
            }

            this.EnableControlButton();

            if (!this.ViewModel.WorkingPlaces_ItemSource.Any())
            {
                return;
            }

            var entity = this.ViewModel.WorkingPlaces_ItemSource[this.ViewModel.WorkingPlaces_SelectedIndex];
            // 会社名
            this.ViewModel.CompanyName_Text = entity.CompanyName.Text;

            // 住所
            this.ViewModel.Address_Text = entity.Address;

            // 勤務開始時間(時)
            this.ViewModel.WorkingTime_Start_Hour   = entity.WorkingTime.Start.Hour;
            // 勤務開始時間(分)
            this.ViewModel.WorkingTime_Start_Minute = entity.WorkingTime.Start.Minute;
            // 勤務終了時間(時)
            this.ViewModel.WorkingTime_End_Hour     = entity.WorkingTime.End.Hour;
            // 勤務終了時間(分)
            this.ViewModel.WorkingTime_End_Minute   = entity.WorkingTime.End.Minute;

            // 昼休憩開始時間(時)
            this.ViewModel.LunchTime_Start_Hour   = entity.LunchTime.Start.Hour;
            // 昼休憩開始時間(分)
            this.ViewModel.LunchTime_Start_Minute = entity.LunchTime.Start.Minute;
            // 昼休憩開始時間(時)
            this.ViewModel.LunchTime_End_Hour     = entity.LunchTime.End.Hour;
            // 昼休憩開始時間(分)
            this.ViewModel.LunchTime_End_Minute   = entity.LunchTime.End.Minute;

            // 昼休憩開始時間(時)
            this.ViewModel.BreakTime_Start_Hour   = entity.BreakTime.Start.Hour;
            // 昼休憩開始時間(分)
            this.ViewModel.BreakTime_Start_Minute = entity.BreakTime.Start.Minute;
            // 昼休憩開始時間(時)
            this.ViewModel.BreakTime_End_Hour     = entity.BreakTime.End.Hour;
            // 昼休憩開始時間(分)
            this.ViewModel.BreakTime_End_Minute   = entity.BreakTime.End.Minute;

            // 備考
            this.ViewModel.Remarks = entity.Remarks;
        }

        /// <summary>
        /// 会社名 - TextChanged
        /// </summary>
        public void EnableAddButton()
        {
            var inputted = (!string.IsNullOrEmpty(this.ViewModel.CompanyName_Text) && !string.IsNullOrEmpty(this.ViewModel.Address_Text));

            this.ViewModel.Add_IsEnabled = inputted;
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

                this.ViewModel.Entities = WorkingPlace.FetchByDescending();

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
            // 会社名
            this.ViewModel.CompanyName_Text   = default(string);

            // 住所
            this.ViewModel.Address_Text       = default(string);
            
            // 労働 - 開始 - 時
            this.ViewModel.WorkingTime_Start_Hour   = default(int);
            // 労働 - 開始 - 分
            this.ViewModel.WorkingTime_Start_Minute = default(int);
            // 労働 - 終了 - 時
            this.ViewModel.WorkingTime_End_Hour     = default(int);
            // 労働 - 終了 - 分
            this.ViewModel.WorkingTime_End_Minute   = default(int);
            
            // 昼休憩 - 開始 - 時
            this.ViewModel.LunchTime_Start_Hour   = default(int);
            // 昼休憩 - 開始 - 分
            this.ViewModel.LunchTime_Start_Minute = default(int);
            // 昼休憩 - 終了 - 時
            this.ViewModel.LunchTime_End_Hour     = default(int);
            // 昼休憩 - 終了 - 分
            this.ViewModel.LunchTime_End_Minute   = default(int);

            // 休憩 - 開始 - 時
            this.ViewModel.BreakTime_Start_Hour   = default(int);
            // 休憩 - 開始 - 分
            this.ViewModel.BreakTime_Start_Minute = default(int);
            // 休憩 - 終了 - 時
            this.ViewModel.BreakTime_End_Hour     = default(int);
            // 休憩 - 終了 - 分
            this.ViewModel.BreakTime_End_Minute   = default(int);

            // 備考
            this.ViewModel.Remarks = default(string);

            // 追加ボタン
            this.ViewModel.Add_IsEnabled    = false;
            // 更新ボタン
            this.ViewModel.Update_IsEnabled = false;
            // 削除ボタン
            this.ViewModel.Delete_IsEnabled = false;
        }

        /// <summary>
        /// 追加
        /// </summary>
        public void Add()
        {
            if (!Message.ShowConfirmingMessage($"入力された就業場所を追加しますか？", this.ViewModel.Title))
            {
                // キャンセル
                return;
            }

            using (var cursor = new CursorWaiting())
            {
                this.ViewModel.Delete_IsEnabled = true;

                var entity = this.CreateEntity(this.ViewModel.Entities.Count + 1);
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
                this.ViewModel.CompanyName_Text,
                this.ViewModel.Address_Text,
                (this.ViewModel.WorkingTime_Start_Hour, this.ViewModel.WorkingTime_Start_Minute),
                (this.ViewModel.WorkingTime_End_Hour,   this.ViewModel.WorkingTime_End_Hour),
                (this.ViewModel.LunchTime_Start_Hour, this.ViewModel.LunchTime_Start_Minute),
                (this.ViewModel.LunchTime_End_Hour, this.ViewModel.LunchTime_End_Minute),
                (this.ViewModel.BreakTime_Start_Hour, this.ViewModel.BreakTime_Start_Minute),
                (this.ViewModel.BreakTime_End_Hour, this.ViewModel.BreakTime_End_Minute),
                this.ViewModel.Remarks);
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void Update()
        {
            if (!Message.ShowConfirmingMessage($"選択中の職歴を更新しますか？", this.ViewModel.Title))
            {
                // キャンセル
                return;
            }

            using (var cursor = new CursorWaiting())
            {
                var id = this.ViewModel.WorkingPlaces_ItemSource[this.ViewModel.WorkingPlaces_SelectedIndex].ID;

                var entity = this.CreateEntity(id);
                this.ViewModel.WorkingPlaces_ItemSource[this.ViewModel.WorkingPlaces_SelectedIndex] = entity;

                this.Save();
            }   
        }

        /// <summary>
        /// 削除
        /// </summary>
        public void Delete()
        {
            if (this.ViewModel.WorkingPlaces_SelectedIndex == -1 ||
                !this.ViewModel.WorkingPlaces_ItemSource.Any())
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
                _repository.Delete(this.ViewModel.WorkingPlaces_SelectedIndex + 1);

                this.ViewModel.WorkingPlaces_ItemSource.RemoveAt(this.ViewModel.WorkingPlaces_SelectedIndex);

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
    }
}