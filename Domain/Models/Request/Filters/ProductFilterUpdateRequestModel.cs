namespace Domain.Models.Request.Filters
{
    public class ProductFilterUpdateRequestModel
    {
        public int OldProductId { get; set; }
        public int OldFilterId { get; set; }

        public int NewProductId { get; set; }
        public int NewFilterId { get; set; }
    }
}
