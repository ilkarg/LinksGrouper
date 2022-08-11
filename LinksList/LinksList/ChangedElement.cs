namespace LinksList;

public class ChangedElement
{
    public string? PrevValue { get; set; }
    public string? CurrentValue { get; set; }

    public ChangedElement(int index, string? prevValue, string? currentValue)
    {
        PrevValue = prevValue;
        CurrentValue = currentValue;
    }
}