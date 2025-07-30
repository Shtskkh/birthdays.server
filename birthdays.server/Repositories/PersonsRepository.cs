using birthdays.server.Context;
using birthdays.server.Entities;
using birthdays.server.Requests;
using Microsoft.EntityFrameworkCore;

namespace birthdays.server.Repositories;

public class PersonsRepository
{
    private readonly BirthdaysContext _context = new();

    public RepositoryResult<Person> GetPersonBirthday(int id)
    {
        var person = _context.Persons.AsNoTracking().FirstOrDefault(p => p.Id == id);

        if (person != null)
        {
            return RepositoryResult<Person>.Success(person);
        }

        return RepositoryResult<Person>.Fail("Запись с таким ID не найдена.");
    }

    public Person[] GetAllBirthdays()
    {
        return _context.Persons.AsNoTracking().ToArray();
    }

    public Person[] GetCurrentBirthdays()
    {
        var currentDate = DateOnly.FromDateTime(DateTime.Now);
        return _context.Persons.AsNoTracking()
            .Where(p => p.Birthday >= currentDate && p.Birthday <= currentDate.AddDays(14))
            .ToArray();
    }

    public RepositoryResult<Person> AddBirthday(Person person)
    {
        try
        {
            _context.Persons.Add(person);
            _context.SaveChanges();
            return RepositoryResult<Person>.Success(person);
        }
        catch (DbUpdateException e)
        {
            return RepositoryResult<Person>.Fail(e.Message);
        }
        catch (Exception e)
        {
            return RepositoryResult<Person>.Fail(e.Message);
        }
    }

    public RepositoryResult<Person> UpdateBirthday(UpdatePerson request)
    {
        if (!_context.Persons.Any(p => p.Id == request.Id))
        {
            return RepositoryResult<Person>.Fail("Запись с таким ID не найдена.");
        }

        var person = new Person
        {
            Id = request.Id,
        };

        _context.Persons.Attach(person);

        if (request.Name != null)
        {
            person.Name = request.Name;
            _context.Entry(person).Property(p => p.Name).IsModified = true;
        }

        if (request.Birthday.HasValue)
        {
            person.Birthday = request.Birthday.Value;
            _context.Entry(person).Property(p => p.Birthday).IsModified = true;
        }

        _context.SaveChanges();
        return RepositoryResult<Person>.Success(person);
    }

    public RepositoryResult<Person> DeleteBirthday(int id)
    {
        if (id < 1)
        {
            return RepositoryResult<Person>.Fail("ID не может быть меньше 1.");
        }

        try
        {
            var person = _context.Persons.Find(id);
            if (person == null)
            {
                return RepositoryResult<Person>.Fail("Не удалось найти человека с таким ID.");
            }

            _context.Persons.Remove(person);
            _context.SaveChanges();

            return RepositoryResult<Person>.Success(person);
        }
        catch (DbUpdateException e)
        {
            return RepositoryResult<Person>.Fail(e.Message);
        }
        catch (Exception e)
        {
            return RepositoryResult<Person>.Fail(e.Message);
        }
    }
}