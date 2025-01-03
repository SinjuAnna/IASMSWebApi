using System.Threading.Tasks;
using IASMSWebApi.Model;
using Microsoft.Data.SqlClient;

namespace IASMSWebApi.Interface
{
    public interface IProductRepository
    {
        Task<Response> GetAllProducts();
        Task<Response> GetProductsById(int id);
        Task<Response> AddProduct(AddProductModel addProductModel);
        Task<Response> UpdateProduct(UpdateProductModel updateProductModel);
        Task<Response> DeleteProduct(int id);
    }
}
