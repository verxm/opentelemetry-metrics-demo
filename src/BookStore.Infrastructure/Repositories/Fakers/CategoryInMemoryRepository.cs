using BookStore.Domain.Interfaces;
using BookStore.Domain.Models;
using BookStore.Infrastructure.Metrics;
using System.Threading.Tasks;

namespace BookStore.Infrastructure.Repositories.Fakers;

public class CategoryInMemoryRepository : InMemoryRepository<Category>, ICategoryRepository
{
    private readonly OtelMetrics _meters;

    public CategoryInMemoryRepository(OtelMetrics meters)
    {
        _meters = meters;
    }

    public override async Task Add(Category entity)
    {
        await base.Add(entity);
        _meters.AddCategory();
        _meters.IncreaseTotalCategories();
    }

    public override async Task Update(Category entity)
    {
        await base.Update(entity);
        _meters.UpdateCategory();
    }

    public override async Task Remove(Category entity)
    {
        await base.Remove(entity);
        _meters.DeleteCategory();
        _meters.DecreaseTotalCategories();
    }
}
