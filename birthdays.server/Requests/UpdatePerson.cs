namespace birthdays.server.Requests;

public class UpdatePerson
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public DateOnly? Birthday { get; set; }
}