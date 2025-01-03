namespace IASMSWebApi.Model
{
    public class Response
    {
        public int StatusCode { get;set; }
        public string StatusMessage { get;set; }
        public Product Product { get;set; } 
        public List<Product> ListProducts { get;set;}
        public string Errors { get; set; }
    }
}
