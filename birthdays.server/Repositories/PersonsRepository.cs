using birthdays.server.Context;
using birthdays.server.Entities;
using Microsoft.EntityFrameworkCore;

namespace birthdays.server.Repositories;

public class PersonsRepository
{
    private readonly BirthdaysContext _context = new();

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

    // public RepositoryResult<bool> UpdateBirthday(Person person)
    // {
    //     
    // }

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