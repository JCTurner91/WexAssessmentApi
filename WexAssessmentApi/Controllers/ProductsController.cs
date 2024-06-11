using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WexAssessmentApi.Models;
using WexAssessmentApi.Repositories;

namespace WexAssessmentApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private ProductRepository _productsRepository;

        // Using dependency injection for the repository so the in memory data storage isn't lost between calls.
        public ProductsController(ILogger<ProductsController> logger, ProductRepository repository)
        {
            _productsRepository = repository ?? new ProductRepository();
        }

        /// <summary>
        /// Get all items in inventory with pageing.
        /// Paging defaults to first page with a page size of 10 items.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Get([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            IEnumerable<Product> results = new List<Product>();

            Task.Run(async () => results = await _productsRepository.GetAllAsync()).Wait();

            var totalCount = results.Count();
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            results = results.Skip((page - 1) * pageSize).Take(pageSize);

            var result = new
            {
                TotalCount = totalCount,
                TotalPages = totalPages,
                CurrentPage = page,
                PageSize = pageSize,
                Products = results.ToList()
            };

            return Ok(result);
        }

        /// <summary>
        /// Get one by Id, if it exists.
        /// If no item with Id exists, returns 404 NotFound.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            Product product = null;
            Task.Run(async () => product = await _productsRepository.GetByIdAsync(id)).Wait();

            if (product != null)
            {
                return Ok(product);
            }

            return NotFound();
        }

        /// <summary>
        /// Create item if valid.
        /// Model defines various validation restrictions, including:
        /// - Name: Required, Max 100 characters
        /// - Category: Required
        /// - Price: >=0 and <= Int.MaxValue
        /// - StockQuantity: >=0 and <= Int.MaxValue
        /// If payload provided is invalid, API will return 400 BadRequest
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Post(Product product)
        {
            Task.Run(async () => await _productsRepository.AddAsync(product)).Wait();

            return Ok();
        }

        /// <summary>
        /// Update item with Id matching Id provided with provided payload.
        /// If no item exists with Id matching the one provided, the payload will simply be upserted if valid.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Product product)
        {
            Task.Run(async () => await _productsRepository.UpdateAsync(product)).Wait();

            return Ok();
        }

        /// <summary>
        /// Deletes item matching Id provided.
        /// If no item exists with matching Id, still retuns 200 OK as the item isn't in inventory.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            Task.Run(async () => await _productsRepository.DeleteAsync(id)).Wait();

            return Ok();
        }
    }
}
