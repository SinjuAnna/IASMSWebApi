using System.ComponentModel.DataAnnotations;

namespace IASMSWebApi.Model
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Price { get; set; }
        public string Quantity { get; set; }
    }
    public class ProductById
    {
        public int ProductId { get; set; }
    }
    public class AddProductModel
    {
        [Required(ErrorMessage = "Product Name is required.")]
        [StringLength(50, ErrorMessage = "Product Name cannot exceed 50 characters.")]
        public string ProductName { get; set; }
        [Required(ErrorMessage = "Price is required.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Price must be a valid number (up to two decimal places).")]
        public string Price { get; set; }
        [Required(ErrorMessage = "Quantity is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be a positive number.")]
        public string Quantity { get; set; }
    }
    public class UpdateProductModel
    {
        [Required(ErrorMessage = "ProductId is required.")]
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Product Name is required.")]
        [StringLength(50, ErrorMessage = "Product Name cannot exceed 50 characters.")]
        public string ProductName { get; set; }
        [Required(ErrorMessage = "Price is required.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Price must be a valid number (up to two decimal places).")]
        public string Price { get; set; }
        [Required(ErrorMessage = "Quantity is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be a positive number.")]
        public string Quantity { get; set; }
    }
}
