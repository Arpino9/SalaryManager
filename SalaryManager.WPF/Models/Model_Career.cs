using DocumentFormat.OpenXml.Office2010.ExcelAc;
using SalaryManager.Domain.Entities;
using SalaryManager.Domain.Modules.Helpers;
using SalaryManager.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Documents;

namespace SalaryManager.WPF.Models
{
    /// <summary>
    /// Model - 職歴
    /// </summary>
    public class Model_Career
    {
        #region Get Instance

        private static Model_Career model = null;

        public static Model_Career GetInstance()
        {
            if (model == null)
            {
                model = new Model_Career();
            }

            return model;
        }

        #endregion

        public Model_Career()
        {
            
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            // 勤務開始日、勤務終了日
            this.ViewModel.WorkingStartDate = DateTime.Now;
            this.ViewModel.WorkingEndDate   = DateTime.Now;

            // 雇用形態
            this.ViewModel.WorkingStatus_ItemSource.Clear();
            this.ViewModel.WorkingStatus_ItemSource.Add("正社員");
            this.ViewModel.WorkingStatus_ItemSource.Add("契約社員");
            this.ViewModel.WorkingStatus_ItemSource.Add("派遣社員");
            this.ViewModel.WorkingStatus_ItemSource.Add("業務委託");
            this.ViewModel.WorkingStatus_ItemSource.Add("アルバイト");

            if (this.ViewModel.Careers_ItemSource.Any())
            {
                this.ViewModel.Remove_IsEnabled = true;
            }

            this.ViewModel.WorkingStatus_Text = this.ViewModel.WorkingStatus_ItemSource.First();

            this.IsWorking_Checked();
        }

        /// <summary> ViewModel - 職歴 </summary>
        public ViewModel_Career ViewModel { get; set; }

        internal void IsWorking_Checked()
        {
            if (this.ViewModel.IsWorking)
            {
                this.ViewModel.WorkingEndDate_IsEnabled = false;
            }
            else
            {
                this.ViewModel.WorkingEndDate_IsEnabled = true;
            }
        }

        /// <summary>
        /// 追加
        /// </summary>
        public void Add()
        {
            this.ViewModel.Remove_IsEnabled = true;

            var allowance = CreateAllowanceExistenceEntity();

            var workingEndDate = this.ViewModel.IsWorking ? DateTime.MaxValue : this.ViewModel.WorkingEndDate;
            this.ViewModel.Careers_ItemSource.Add(
                new CareerEntity(
                    this.ViewModel.WorkingStatus_Text,
                    this.ViewModel.CompanyName, 
                    this.ViewModel.WorkingStartDate, 
                    workingEndDate,
                    allowance,
                    this.ViewModel.Remarks));

            // 並び変え
            this.ViewModel.Careers_ItemSource = new ObservableCollection<CareerEntity>(this.ViewModel.Careers_ItemSource.OrderByDescending(x => x.WorkingStartDate.ToString()));
        }

        /// <summary>
        /// 手当有無の作成
        /// </summary>
        /// <returns>手当有無</returns>
        private AllowanceExistenceEntity CreateAllowanceExistenceEntity()
        {
            return new AllowanceExistenceEntity(
                this.ViewModel.PerfectAttendanceAllowance_IsChecked,
                this.ViewModel.EducationAllowance_IsChecked,
                this.ViewModel.ElectricityAllowance_IsChecked,
                this.ViewModel.CertificationAllowance_IsChecked,
                this.ViewModel.OvertimeAllowance_IsChecked,
                this.ViewModel.TravelAllowance_IsChecked,
                this.ViewModel.HousingAllowance_IsChecked,
                this.ViewModel.FoodAllowance_IsChecked,
                this.ViewModel.LateNightAllowance_IsChecked,
                this.ViewModel.AreaAllowance_IsChecked,
                this.ViewModel.CommutingAllowance_IsChecked,
                this.ViewModel.DependencyAllowance_IsChecked,
                this.ViewModel.ExecutiveAllowance_IsChecked,
                this.ViewModel.SpecialAllowance_IsChecked);
        }

        /// <summary>
        /// 削除
        /// </summary>
        public void Remove()
        {
            if (this.ViewModel.Careers_SelectedIndex == -1 ||
                !this.ViewModel.Careers_ItemSource.Any()) 
            {
                return;
            }

            this.ViewModel.Careers_ItemSource.RemoveAt(this.ViewModel.Careers_SelectedIndex);

            if (!this.ViewModel.Careers_ItemSource.Any())
            {
                this.ViewModel.Remove_IsEnabled = false;
            }
        }
    }
}
