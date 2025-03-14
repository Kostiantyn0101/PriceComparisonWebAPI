using Domain.Models.Primitives;

namespace Domain.Models.DBModels
{
    public class ProductDBModel : IEntity<int>
    {
        public int Id { get; set; }     
        public int BaseProductId { get; set; }            
        public string VariantName { get; set; }
        public string? ModelNumber { get; set; }                                        
        public string? GTIN { get; set; }                                               
        public string? UPC { get; set; }                                                
        public int? ColorId { get; set; }
        public bool IsDefault { get; set; }
        public bool IsUnderModeration { get; set; }    
        public DateTime AddedToDatabase { get; set; }  

        public BaseProductDBModel BaseProduct { get; set; }
        public ColorDBModel? Color { get; set; }

        public ICollection<ProductImageDBModel> ProductImages { get; set; }                               
        public ICollection<ProductCharacteristicDBModel> ProductCharacteristics { get; set; }             
        public ICollection<SellerProductDetailsDBModel> SellerProductDetails { get; set; }                
        public ICollection<PriceHistoryDBModel> PricesHistories { get; set; }                             
        public ICollection<ProductClicksDBModel> ProductClicks { get; set; }                              
        public ICollection<ProductReferenceClickDBModel> ProductSellerReferenceClicks { get; set; }       
    }
}
