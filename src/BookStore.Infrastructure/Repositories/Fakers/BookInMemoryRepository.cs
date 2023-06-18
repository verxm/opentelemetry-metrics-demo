using BookStore.Domain.Interfaces;
using BookStore.Domain.Models;
using BookStore.Infrastructure.Metrics;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Infrastructure.Repositories.Fakers;

public class BookInMemoryRepository : InMemoryRepository<Book>, IBookRepository
{
    private readonly OtelMetrics _meters;

    public BookInMemoryRepository(OtelMetrics meters)
    {
        _meters = meters;
    }

    public override async Task<List<Book>> GetAll()
    {
        return Entities.ToList();
    }

    public override async Task<Book> GetById(int id)
    {
        return Entities
            .Where(b => b.Id == id)
            .FirstOrDefault();
    }

    public async Task<IEnumerable<Book>> GetBooksByCategory(int categoryId)
    {
        return await SearchWithFunc(b => b.CategoryId == categoryId);
    }

    public async Task<IEnumerable<Book>> SearchBookWithCategory(string searchedValue)
    {
        return Entities
            .Where(b => b.Name.Contains(searchedValue) ||
                        b.Author.Contains(searchedValue) ||
                        b.Description.Contains(searchedValue) ||
                        b.Category.Name.Contains(searchedValue))
            .ToList();
    }

    public override async Task Add(Book entity)
    {
        await base.Add(entity);

        _meters.AddBook();
        _meters.IncreaseTotalBooks();
    }

    public override async Task Update(Book entity)
    {
        await base.Update(entity);

        _meters.UpdateBook();
    }

    public override async Task Remove(Book entity)
    {
        await base.Remove(entity);

        _meters.DeleteBook();
        _meters.DecreaseTotalBooks();
    }
}
