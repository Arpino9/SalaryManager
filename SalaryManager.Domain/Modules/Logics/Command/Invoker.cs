namespace SalaryManager.Domain.Modules.Logics.LogCommand;

/// <summary>
/// CommandパターンのInvoker
/// </summary>
public static class Invoker
{
    private static Queue<Command> _command = new Queue<Command>();

    private static AutoResetEvent _autoreset = new AutoResetEvent(false);

    public static void AddCommand(Command command)
    {
        lock(_command)
        {
            _command.Enqueue(command);
        }

        _autoreset.Set();
    }

    public static async Task ExecuteCommands()
    {
        while (true)
        {
            Command command = null;
            lock (_command)
            {
                if (_command.Count > 0)
                {
                    command = _command.Dequeue();
                }
            }

            if (command != null)
            {
                command.Execute();
                await Task.Delay(1000);
            }
            else
            {
                _autoreset.WaitOne();
            }
        }
    }
}