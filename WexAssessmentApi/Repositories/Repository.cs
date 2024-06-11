using System.Reflection.Metadata.Ecma335;

namespace WexAssessmentApi.Repositories
{
    public abstract class Repository<T> : IRepository<T>
    {
        private Dictionary<int, T> _items;

        /// <summary>
        /// Allows for a Dictionary to be passed in, but will create it's own if necessary.
        /// </summary>
        /// <param name="items"></param>
        public Repository(Dictionary<int, T> items)
        {
            _items = items ?? new Dictionary<int, T>();
        }

        /// <summary>
        /// Adds provided entity to Dictionary with a random integer as it's key.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task AddAsync(T entity)
        {
            await Task.Run(() => _items.Add(Random.Shared.Next(), entity));
        }

        /// <summary>
        /// Deletes entity with matching key.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(int id)
        {
            await Task.Run(() => _items.Remove(id));
        }

        /// <summary>
        /// Returns all values in dictionary as an IEnumerable.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await Task.Run(() => _items.Values);
        }

        /// <summary>
        /// Returns only the entity value of the KeyValuePair with key == id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T> GetByIdAsync(int id)
        {
            return await Task.Run(() => _items.GetValueOrDefault(id));
        }


        /// <summary>
        /// Tries to get matching entry where value == entity.
        /// If no matching entry is found, nothing happens.
        /// If a matching entry is found, it removes the old one and upserts the new entity with the old key.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task UpdateAsync(T entity)
        {
            await Task.Run(() =>
            {
                var keyValuePair = _items.FirstOrDefault(x => x.Value?.Equals(entity) ?? false);
                // Check to see we got a valid value back. KeyValuePair will return default, not null;
                if (!keyValuePair.Equals(default(KeyValuePair<int, T>))){
                    _items.Remove(keyValuePair.Key);
                    _items.Add(keyValuePair.Key, entity);
                }
            });
        }
    }
}
