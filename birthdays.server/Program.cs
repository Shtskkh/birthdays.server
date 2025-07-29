using birthdays.server.Classes;

Menu.Options[1].Action.Invoke();

ConsoleKeyInfo keyInfo;
do
{
    keyInfo = Console.ReadKey();

    switch (keyInfo.Key)
    {
        case ConsoleKey.D1:
            Menu.Options[0].Action.Invoke();
            break;
        case ConsoleKey.D2:
            Menu.Options[1].Action.Invoke();
            break;
        case ConsoleKey.D3:
            Menu.Options[2].Action.Invoke();
            break;
        case ConsoleKey.D4:
            Menu.Options[3].Action.Invoke();
            break;
        case ConsoleKey.D5:
            Menu.Options[4].Action.Invoke();
            break;
    }
} while (keyInfo.Key != ConsoleKey.X);

Console.ReadKey();