using System.Collections.Generic;

namespace LinksList;

public class LinkGroup
{
    public string? Header { get; set; }
    public List<string> LinksList { get; set; }

    public LinkGroup(string? header)
    {
        Header = header;
        LinksList = new List<string>();
    }
}