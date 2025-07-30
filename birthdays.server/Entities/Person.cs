using System;
using System.Collections.Generic;

namespace birthdays.server.Entities;

public partial class Person
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateOnly Birthday { get; set; }
}