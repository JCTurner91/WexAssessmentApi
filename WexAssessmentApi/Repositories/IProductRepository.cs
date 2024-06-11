using WexAssessmentApi.Models;

namespace WexAssessmentApi.Repositories
{
    /// <summary>
    /// Expanded repository interface containing a Get filtered by Category.
    /// </summary>
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category);
    }
}
