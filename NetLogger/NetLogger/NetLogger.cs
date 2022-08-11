namespace NetLogger;

public abstract class NetLogger
{
    private Dictionary<string, string> _logPrefix { get; set; }
    
    public abstract bool AddLogsPrefix(string prefixKey, string prefixValue);
    public abstract bool WriteInLogs(string prefixKey, string message);

    public void Init()
    {
        if (!LogsDirExists())
        {
            Directory.CreateDirectory("logs");
        }

        if (!LogsFileExists())
        {
            File.Create("logs/logs.log");
        }
    }

    public bool LogsDirExists() =>
        Directory.Exists("logs");

    public bool LogsFileExists() =>
        File.Exists("logs/logs.log");

    /*public void AddLogsPrefix(string prefixKey, string prefixValue)
    {
        if (_logPrefix.ContainsKey(prefixKey) || prefixValue.Trim() == string.Empty)
        {
            return;
        }

        _logPrefix.Add(prefixKey, prefixValue);
    }

    public bool WriteInLogs(string prefixKey, string message)
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
                writer.WriteLine($"{_logPrefix[prefixKey]}");
            }
        }
        catch (FileLoadException exception)
        {
            Console.WriteLine($"Ошибка: {exception.StackTrace}");
            return false;
        }

        return true;
    }*/
}