using IASMSWebApi.Model;
using Microsoft.AspNetCore.Mvc;
using IASMSWebApi.Interface;


namespace IASMSWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductRepository _productRepository;  
        public ProductController(ILogger<ProductController> logger, IProductRepository productRepository)
        {
            _logger = logger;
            _productRepository = productRepository;
        }      

        [HttpGet]
        [Route("GetAllProducts")]
        public async Task<Response> GetAllProducts()
        {
            _logger.LogInformation("GetAllProducts started at:" + DateTime.Now);         
            try
            {
                var response= await _productRepository.GetAllProducts();
                _logger.LogInformation("GetAllProducts completeled successfully at:" + DateTime.Now);
                return response;
            }
           catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new Response { StatusCode = 500, StatusMessage = "An error occurred while fetching products." };
            }
        }

        [HttpGet]
        [Route("GetProductsById/{Id}")]
        public async Task<Response> GetProductsById(int Id)
        {
            _logger.LogInformation("GetProductsById started at:" + DateTime.Now);
            try
            {
                var response = await _productRepository.GetProductsById(Id);
                _logger.LogInformation("GetProductsById completeled successfully at:" + DateTime.Now);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new Response { StatusCode = 500, StatusMessage = "An error occurred while fetching products." };
            }
        }

        [HttpPost]
        [Route("AddProduct")]
        public async Task<Response> AddProduct(AddProductModel addProductModel)
        {
            _logger.LogInformation("Add Product started at:" + DateTime.Now);
            Response response = new Response();
            try
            {
                if (!ModelState.IsValid)
                {
                    var errorMessages = string.Join(", ", ModelState.Values
                        .Where(v => v.Errors.Count > 0)
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));
                    response = new Response
                    {
                        StatusCode = 400, 
                        StatusMessage = "Validation failed.",
                        Errors = string.Join(", ", errorMessages) 
                    };
                    return response;
                }
                 response = await _productRepository.AddProduct(addProductModel);
                _logger.LogInformation("Add Product successfully at:" + DateTime.Now);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new Response { StatusCode = 500, StatusMessage = "An error occurred while fetching products." };
            }
            
        }

        [HttpPost]
        [Route("UpdateProduct")]
        public async Task<Response> UpdateProduct(UpdateProductModel updateProductModel)
        {
            _logger.LogInformation("Update Product started at:" + DateTime.Now);
            Response response = new Response();
            try
            {
                if (!ModelState.IsValid)
                {
                    var errorMessages = string.Join(", ", ModelState.Values
                        .Where(v => v.Errors.Count > 0)
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));
                    response = new Response
                    {
                        StatusCode = 400, 
                        StatusMessage = "Validation failed.",
                        Errors = string.Join(", ", errorMessages)
                    };
                    return response;
                }
                 response = await _productRepository.UpdateProduct(updateProductModel);
                _logger.LogInformation("UpdateProduct successfully at" + updateProductModel.ProductId + " at:" + DateTime.Now);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new Response { StatusCode = 500, StatusMessage = "An error occurred while fetching products." };
            }

        }
        [HttpDelete]
        [Route("DeleteProduct/{Id}")]
        public async Task<Response> DeleteProduct(int Id)
        {
            _logger.LogInformation("Delete Product started at:" + DateTime.Now);
            try
            {
                var response = await _productRepository.DeleteProduct(Id);
                _logger.LogInformation("Product deleted successfully for ProductId" + Id + " at:" + DateTime.Now);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new Response { StatusCode = 500, StatusMessage = "An error occurred while fetching products." };

            }
        }

    }
}