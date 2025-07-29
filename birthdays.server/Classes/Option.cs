namespace birthdays.server.Classes;

public class Option(string title, Action action)
{
    public readonly string Title = title;
    public readonly Action Action = action;
}