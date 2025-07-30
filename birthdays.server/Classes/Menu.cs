using birthdays.server.Entities;
using birthdays.server.Repositories;
using birthdays.server.Requests;

namespace birthdays.server.Classes;

public static class Menu
{
    private static readonly PersonsRepository Repository = new();

    public static readonly List<Option> Options =
    [
        new("1. Все дни рождения", WriteAllBirthdays),
        new("2. Текущие и ближайщие дни рождения", WriteCurrentBirthdays),
        new("3. Добавить день рождения", AddBirthday),
        new("4. Редактировать день рождения", UpdateBirthday),
        new("5. Удалить день рождения", DeleteBirthday)
    ];

    public static void WriteMenu()
    {
        Console.WriteLine();

        foreach (var option in Options)
        {
            Console.WriteLine(option.Title);
        }

        Console.WriteLine();
        Console.WriteLine("Введите X чтобы выйти из программы.");
    }

    private static void WriteAllBirthdays()
    {
        Console.Clear();

        var persons = Repository.GetAllBirthdays();

        if (persons.Length == 0)
        {
            Console.WriteLine("На данный момент список дней рождения пуст.");
        }
        else
        {
            foreach (var person in Repository.GetAllBirthdays())
            {
                Console.WriteLine("Все дни рождения:");
                Console.WriteLine($"{person.Id}. Имя: {person.Name}, день рождения: {person.Birthday}");
            }
        }
    }

    private static void WriteCurrentBirthdays()
    {
        Console.Clear();

        var persons = Repository.GetCurrentBirthdays();

        if (persons.Length == 0)
        {
            Console.WriteLine("В ближайшие две недели нет дней рождения.");
        }
        else
        {
            foreach (var person in Repository.GetAllBirthdays())
            {
                Console.WriteLine("Дни рождения сегодня и в ближайшие две недели:");
                Console.WriteLine($"{person.Id}. Имя: {person.Name}, день рождения: {person.Birthday}");
            }
        }
    }

    private static void AddBirthday()
    {
        Console.Clear();

        Console.Write("Введите имя: ");
        var name = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Введено некорретное имя, попробуйте ещё раз.");
            WriteMenu();
            return;
        }

        Console.Write("Введите день рождения (ДД.ММ.ГГГГ): ");
        var birthdayString = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(birthdayString) || !DateOnly.TryParse(birthdayString, out var birthdayDate))
        {
            Console.Clear();
            Console.WriteLine("Введена некорректная дата, попробуйте ещё раз.");
            WriteMenu();
            return;
        }

        var trimmedName = name.Trim();

        var result = Repository.AddBirthday(new Person
        {
            Name = trimmedName,
            Birthday = birthdayDate
        });

        if (!result.IsSuccess)
        {
            Console.Clear();
            Console.WriteLine($"Ошибка: {result.ErrorMessage}");
            WriteMenu();
            return;
        }

        Console.Clear();
        Console.WriteLine("День рождения успешно добавлен.");
        WriteMenu();
    }

    private static void UpdateBirthday()
    {
        Console.Clear();

        WriteAllBirthdays();

        Console.Write("Введите номер записи, которую хотите обновить: ");
        var idString = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(idString) || !int.TryParse(idString, out var id))
        {
            Console.Clear();
            Console.WriteLine("Введён некорретный номер записи, попробуйте ещё раз.");
            WriteMenu();
            return;
        }

        var person = Repository.GetPersonBirthday(id);

        if (!person.IsSuccess)
        {
            Console.Clear();
            Console.WriteLine("Не удалось получить указанную запись с указанным номером, попробуйте ещё раз.");
            WriteMenu();
            return;
        }

        var updatePerson = new UpdatePerson
        {
            Id = person.Data.Id
        };

        Console.Clear();
        Console.WriteLine($"Предыдущие имя: {person.Data.Name}");
        Console.Write("Введите новое имя или оставьте строку пустой, чтобы оставить предыдущее: ");
        var nameString = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(nameString))
        {
            updatePerson.Name = nameString;
        }

        Console.WriteLine($"Предыдущая дата: {person.Data.Birthday}");
        Console.Write("Введите новую дату или оставьте строку пустой, чтобы оставить предыдущее: ");
        var birthdayString = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(birthdayString))
        {
            if (!DateOnly.TryParse(birthdayString, out var birthdayDate))
            {
                Console.Clear();
                Console.WriteLine("Введена некорректная дата, попробуйте ещё раз.");
                WriteMenu();
                return;
            }

            updatePerson.Birthday = birthdayDate;
        }

        var result = Repository.UpdateBirthday(updatePerson);

        if (!result.IsSuccess)
        {
            Console.Clear();
            Console.WriteLine($"Ошибка: {result.ErrorMessage}");
            WriteMenu();
            return;
        }

        Console.Clear();
        Console.WriteLine("День рождения успешно обновлён.");
        WriteMenu();
    }

    private static void DeleteBirthday()
    {
        Console.Clear();

        WriteAllBirthdays();

        Console.Write("Введите номер записи, которую хотите удалить: ");
        var idString = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(idString) || !int.TryParse(idString, out var id))
        {
            Console.Clear();
            Console.WriteLine("Введён некорретный номер записи, попробуйте ещё раз.");
            WriteMenu();
            return;
        }

        var result = Repository.DeleteBirthday(id);

        if (!result.IsSuccess)
        {
            Console.Clear();
            Console.WriteLine($"Ошибка: {result.ErrorMessage}");
            WriteMenu();
            return;
        }

        Console.Clear();
        Console.WriteLine("День рождения успешно удалён.");
        WriteMenu();
    }
}