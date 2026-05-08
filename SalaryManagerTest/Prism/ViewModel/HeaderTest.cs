using Moq;
using SalaryManager.Domain.Entities;
using SalaryManager.Domain.Modules.Helpers;
using SalaryManager.Domain.Repositories;
using SalaryManager.Prism.ViewModels;

namespace SalaryManagerTest;

/// <summary>
/// ViewModel - ヘッダ テスト
/// </summary>
public class HeaderTest
{
    [Fact]
    public void 初期化テスト()
    {
        var header = new HeaderEntity(1, DateUtils.Today, true, DateTime.Now, DateTime.Now);
        var headerMock = new Mock<IHeaderRepository>();
        headerMock.Setup(x => x.FetchDefault()).Returns(header);

        var vm = new HeaderViewModel(headerMock.Object);
        vm.Year_Text.Equals(DateTime.Today.Year);
        vm.Month_Text.Equals(DateTime.Today.Month);
    }
}
