namespace NetLogger;

public class NetLoggerConsole : NetLogger
{
    // TODO: Переписать catch на конкретный тип исключений
    // TODO: Покрыть юнит тестами все это говнище

    private Dictionary<string, string> _logPrefix { get; set; }
    
    public NetLoggerConsole()
    {
        _logPrefix = new Dictionary<string, string>() 
        {
            {"info", "INFO > "},
            {"warn", "WARN > "},
            {"error", "ERROR > "},
            {"debug", "DEBUG > "}
        };
    }

    public override bool AddLogsPrefix(string prefixKey, string prefixValue)
    {
        try
        {
            if (_logPrefix.ContainsKey(prefixKey) || prefixValue.Trim() == string.Empty)
            {
                return false;
            }

            _logPrefix.Add(prefixKey, prefixValue);
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.StackTrace, "Ошибка");
            return false;
        }

        return true;
    }

    public override bool WriteInLogs(string prefixKey, string message)
    {
        try
        {
            if (!_logPrefix.ContainsKey(prefixKey) || message.Trim() == string.Empty)
            {
                return false;
            }

            if (!LogsFileExists())
            {
                throw new FileNotFoundException("Файл logs/logs.log не существует");
            }
            
            using (StreamWriter writer = new StreamWriter("logs/logs.log", true))
            {
                writer.WriteLine($"{_logPrefix[prefixKey]}{message}");
            }
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.StackTrace, "Ошибка");
            return false;
        }

        return true;
    }
}