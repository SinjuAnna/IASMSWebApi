
using IASMSWebApi.Interface;
using IASMSWebApi.Model;
using Microsoft.Data.SqlClient;
using Microsoft.OpenApi.Writers;
using System.Data;

namespace IASMSWebApi.DAL
{
    public class ProductRepository : IProductRepository, IDisposable
    {
        private readonly IConfiguration _configuration;
        private SqlConnection _connection;

        public ProductRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }

        private async Task<SqlConnection> GetConnectionAsync()
        {
            if(_connection.State==ConnectionState.Closed)
            {
                await _connection.OpenAsync();
            }
            return _connection;
        }

        public async Task<Response> GetAllProducts()
        {
            Response response = new Response();
            List<Product> lstproducts = new List<Product>();
            try
            {
                using (var con = await GetConnectionAsync())
                {
                    SqlCommand cmd = new SqlCommand("Get_ProductDetails", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            Product product = new Product
                            {
                                ProductId = Convert.ToInt32(dr["ProductId"]),
                                ProductName = Convert.ToString(dr["ProductName"]),
                                Price = Convert.ToString(dr["Price"]),
                                Quantity = Convert.ToString(dr["Quantity"])
                            };
                            lstproducts.Add(product);
                        }
                    }

                    if (lstproducts.Count > 0)
                    {
                        response.StatusCode = 200;
                        response.StatusMessage = "Data Loaded";
                        response.ListProducts = lstproducts;
                    }
                    else
                    {
                        response.StatusCode = 100;
                        response.StatusMessage = "No Data Loaded";
                        response.ListProducts = null;
                    }
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.StatusMessage = "Exception : " + ex.Message;
                response.ListProducts = null;
            }
            return response;
        }

        public async Task<Response> GetProductsById(int Id)
        {
            Response response = new Response();
            try
            {
                using (var con = await GetConnectionAsync())
                {

                    SqlCommand cmd = new SqlCommand("Get_ProductDetails", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ProductId", Id);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        Product product = new Product
                        {
                            ProductId = Convert.ToInt32(dt.Rows[0]["ProductId"]),
                            ProductName = Convert.ToString(dt.Rows[0]["ProductName"]),
                            Price = Convert.ToString(dt.Rows[0]["Price"]),
                            Quantity = Convert.ToString(dt.Rows[0]["Quantity"])
                        };
                        response.StatusCode = 200;
                        response.StatusMessage = "Data Loaded";
                        response.Product = product;
                    }
                    else
                    {
                        response.StatusCode = 100;
                        response.StatusMessage = "No Data Loaded";
                        response.Product = null;
                    }
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.StatusMessage = "Exception : " + ex.Message;
                response.ListProducts = null;
            }
            return response;
        }

        public async Task<Response> AddProduct(AddProductModel addProductModel)
        {
            Response response = new Response();
            try
            {
                using (var con = await GetConnectionAsync())
                {

                    SqlCommand cmd = new SqlCommand("Add_ProductDetails", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ProductName", addProductModel.ProductName);
                    cmd.Parameters.AddWithValue("@Price", Convert.ToDecimal(addProductModel.Price));
                    cmd.Parameters.AddWithValue("@Quantity", Convert.ToInt32(addProductModel.Quantity));
                    cmd.Parameters.AddWithValue("@UpdatedBy", "SYSTEM");
                    int i =await cmd.ExecuteNonQueryAsync();
                    if (i > 0)
                    {
                        response.StatusCode = 200;
                        response.StatusMessage = "Product Added";
                    }
                    else
                    {
                        response.StatusCode = 200;
                        response.StatusMessage = "No data Inserted";
                    }

                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.StatusMessage = "Exception : " + ex.Message;
                response.ListProducts = null;
            }
            return response;
        }

        public async Task<Response> UpdateProduct(UpdateProductModel updateProductModel)
        {
            Response response = new Response();
            try
            {
                using (var con = await GetConnectionAsync())
                {
                    SqlCommand cmd = new SqlCommand("Add_ProductDetails", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ProductId", updateProductModel.ProductId);
                    cmd.Parameters.AddWithValue("@ProductName", updateProductModel.ProductName);
                    cmd.Parameters.AddWithValue("@Price", Convert.ToDecimal(updateProductModel.Price));
                    cmd.Parameters.AddWithValue("@Quantity", Convert.ToInt32(updateProductModel.Quantity));
                    cmd.Parameters.AddWithValue("@UpdatedBy", "SYSTEM");
                    int i =await cmd.ExecuteNonQueryAsync();
                    if (i > 0)
                    {
                        response.StatusCode = 200;
                        response.StatusMessage = "Product Updated";
                    }
                    else
                    {
                        response.StatusCode = 100;
                        response.StatusMessage = "No data Updated";
                    }                 
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.StatusMessage = "Exception : " + ex.Message;
                response.ListProducts = null;
            }
            return response;

        }

        public async Task<Response> DeleteProduct(int Id)
        {
            Response response = new Response();
            using (var con = await GetConnectionAsync())
            {            
                SqlCommand cmd = new SqlCommand("Delete_ProductDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProductId", Id);
                int i =await cmd.ExecuteNonQueryAsync();
                if (i > 0)
                {
                    response.StatusCode = 200;
                    response.StatusMessage = "Product Deleted";
                }
                else
                {
                    response.StatusCode = 100;
                    response.StatusMessage = "No data Deleted";
                }
                return response;
            }
        }

        public void Dispose()
        {
            if (_connection != null && _connection.State == ConnectionState.Open)
            {
                _connection.Close();
                _connection.Dispose();
            }
        }

    }
}
