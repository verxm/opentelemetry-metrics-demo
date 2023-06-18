using BookStore.Domain.Interfaces;
using BookStore.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BookStore.Infrastructure.Repositories.Fakers;

public class InMemoryRepository<TEntity> : IRepository<TEntity> where TEntity : Entity
{
    protected ICollection<TEntity> Entities { get; private set; }

    protected InMemoryRepository()
    {
        Entities = new List<TEntity>();
    }

    public virtual async Task Add(TEntity entity)
    {
        entity.Id = GetLastId() + 1;
        Entities.Add(entity);
        var _ = await SaveChanges();
    }

    public virtual async Task<List<TEntity>> GetAll()
    {
        return Entities.ToList();
    }

    public virtual async Task<TEntity> GetById(int id)
    {
        return Entities.FirstOrDefault(x => x.Id == id);
    }

    public virtual async Task Update(TEntity entity)
    {
        Entities.Remove(await GetById(entity.Id));
        Entities.Add(entity);
        await SaveChanges();
    }

    public virtual async Task Remove(TEntity entity)
    {
        Entities.Remove(entity);
        await SaveChanges();
    }

    public async Task<IEnumerable<TEntity>> SearchWithFunc(Func<TEntity, bool> predicate)
    {
        return Entities.Where(predicate).ToList();
    }
    public async Task<IEnumerable<TEntity>> Search(Expression<Func<TEntity, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public async Task<int> SaveChanges()
    {
        Console.WriteLine("SaveChanges called.");
        return Entities.Count;
    }

    public void Dispose()
    {
        Entities.Clear();
    }

    protected int GetLastId()
    {
        if (Entities.Any())
        {
            return Entities.Last().Id;
        }

        return 0;
    }
}
