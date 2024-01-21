using Database.Contract.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Database;

public class GenericRepository<T> : IGenericRepository<T> where T : class, new()
{
    private readonly DbSet<T> _dbSet;

    public GenericRepository(DbSet<T> dbSet)
    {
        _dbSet = dbSet ?? throw new ArgumentNullException(nameof(dbSet));
    }

    public T GetById(int id)
    {
        return _dbSet.Find(id);
    }

    public IEnumerable<T> GetAll()
    {
        return _dbSet.ToList();
    }

    public void Add(T entity)
    {
        _dbSet.Add(entity);
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }
}