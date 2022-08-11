using System.Collections.Generic;
using NetLogger;

namespace LinksList;

public static class AppConfig
{
    public static string BackgroundPath = "None";
    public static List<LinkGroup?> LinkGroupsList = new List<LinkGroup?>();
    public static Dictionary<int, bool> DrawedLinkGroup = new Dictionary<int, bool>();
    public static AppSystem? appSystem { get; set; }
    public static NetLoggerDesktop netLoggerDesktop = new NetLoggerDesktop();
}