namespace Domain.Models.Response.Products
{
    public class ProductGroupModifiedResponseModel
    {
        public int Id { get; set; }
        public int FirstProductId { get; set; }
        public string Name { get; set; }
        public int DisplayOrder { get; set; }
    }
}
