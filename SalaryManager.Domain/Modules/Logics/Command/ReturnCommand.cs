namespace SalaryManager.Domain.Modules.Logics.LogCommand;

/// <summary>
/// Command - 戻る
/// </summary>
public class ReturnCommand : Command
{
    private static readonly log4net.ILog _logger =
     log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    private string _callerTitle;

    public ReturnCommand(string callerTitle)
    {
        _callerTitle = callerTitle;
    }

    /// <summary>
    /// Command - ログ出力
    /// </summary>
    public void Execute()
    {
        _logger.Info($"{_callerTitle} >> Return");
    }
}
