using System.Linq;
using WexAssessmentApi.Models;

namespace WexAssessmentApi.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private List<Product> _products;

        /// <summary>
        /// Start with a dummy inventory that's held in memory.
        /// </summary>
        public ProductRepository()
        {
            if (this._products == null)
            {
                _products = new List<Product>()
                {
                    new Product
                    {
                        Id = 1,
                        Name = "Milk",
                        Category = "Groceries",
                        Price = 5,
                        StockQuantity = 10
                    },
                    new Product
                    {
                        Id = 2,
                        Name = "Eggs",
                        Category = "Groceries",
                        Price = 2,
                        StockQuantity = 20
                    },
                    new Product
                    {
                        Id = 3,
                        Name = "Toilet Paper",
                        Category = "Toiletries",
                        Price = 10,
                        StockQuantity = 200
                    }
                };
            }
        }

        /// <summary>
        /// Adds entity to dummy inventory.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task AddAsync(Product entity)
        {
            await Task.Run(() => _products.Add(entity));
        }

        /// <summary>
        /// If item exists with matching id, deletes it.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(int id)
        {
            await Task.Run(() => _products.Remove(_products.FirstOrDefault(x => x.Id == id)));
        }

        /// <summary>
        /// Returns all items in dummy inventory as an IEnumerable.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await Task.Run(() => _products.AsEnumerable());
        }

        /// <summary>
        /// Returns only the first item with the matching id, or null if not found.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Product> GetByIdAsync(int id)
        {
            return await Task.Run(() => _products.FirstOrDefault(x => x.Id == id));
        }

        /// <summary>
        /// Returns only the subset of products with a matching Category to the one provided.
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category)
        {
            return await Task.Run(() => _products.Where(x => x.Category == category).AsEnumerable());
        }

        /// <summary>
        /// Removes any item with matching Id before upserting the new one.
        /// If no matching id exists, would just upsert the new payload.
        /// Should never delete unless a valid payload is provided as the API validates the payload before calling this method.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task UpdateAsync(Product entity)
        {
            await Task.Run(() =>
            {
                _products.Remove(_products.FirstOrDefault(x => x.Id == entity.Id));
                _products.Add(entity);
            });
        }
    }
}
