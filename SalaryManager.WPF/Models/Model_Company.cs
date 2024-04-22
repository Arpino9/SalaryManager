using SalaryManager.Domain.Modules.Helpers;
using SalaryManager.Domain.ValueObjects;
using SalaryManager.Infrastructure.Interface;
using SalaryManager.WPF.ViewModels;
using System;
using System.Linq;

namespace SalaryManager.WPF.Models
{
    public class Model_Company : IMaster
    {
        public void Initialize()
        {
            if (this.ViewModel.BusinessCategory_Large_SelectedItem is null)
            {
                this.ViewModel.BusinessCategory_Large_SelectedItem = this.ViewModel.BusinessCategory_Large_ItemsSource.First().Value;
                this.BusinessCategory_Large_SelectionChanged();
            }
        }

        public void BusinessCategory_Large_SelectionChanged()
        {
            var item = new BusinessCategoryValue(this.ViewModel.BusinessCategory_Large_SelectedItem);
            this.ViewModel.BusinessCategory_Middle_ItemSource = ListUtils.ToObservableCollection(item.GetMiddleCategoryList(this.ViewModel.BusinessCategory_Large_SelectedItem)
                                                                                                     .Select(x => x.Value).ToList());

            if (this.ViewModel.BusinessCategory_Middle_Text is null) this.ViewModel.BusinessCategory_Middle_Text = this.ViewModel.BusinessCategory_Middle_ItemSource.First();
            
            this.ViewModel.BusinessCategory_Middle_SelectedIndex = 0;
            
            this.ViewModel.BusinessCategory_MiddleNo = item.GetMiddleCategoryKey(this.ViewModel.BusinessCategory_Middle_Text);
        }

        /// <summary> ViewModel - 職歴 </summary>
        public ViewModel_Company ViewModel { get; set; }

        public void Clear_InputForm()
        {
            throw new NotImplementedException();
        }

        

        public void Refresh()
        {
            throw new NotImplementedException();
        }

        public void Reload()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
