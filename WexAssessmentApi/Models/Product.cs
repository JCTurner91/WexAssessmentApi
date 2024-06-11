using System.ComponentModel.DataAnnotations;

namespace WexAssessmentApi.Models
{
    public class Product
    {
        /// <summary>
        /// Primary key integer ID, used for singular gets.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// String name for item.
        /// Required, with a character limit of 100.
        /// </summary>
        [Required, StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; }
        /// <summary>
        /// Integer price, valid only between 0 and maximum integer value.
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "Price must be positive.")]
        public decimal Price { get; set; }
        /// <summary>
        /// String category that can be used for filtering. Required.
        /// </summary>
        [Required]
        public string Category { get; set; }
        /// <summary>
        /// Integer stock quantity, only valid between 0 and maximum integer value.
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "StockQuantity must be positive.")]
        public int StockQuantity { get; set; }
    }
}
