namespace Domain.Models.Response.Seller
{
    public class SellerProductDetailsResponseModel
    {
        public int SellerId { get; set; }
        public string StoreName { get; set; }   // from SellerDBModel
        public string LogoImageUrl { get; set; }  // from SellerDBModel
        public decimal PriceValue { get; set; }  //  from SellerProductDetailsDBModel
        public string ProductStoreUrl { get; set; } // from SellerProductDetailsDBModel
        public decimal StoreUrlClickRate { get; set; } // //  from AuctionClickRateDBModel if exist or from SellerAccountConfiguration class (as IOption) - by default
    }
}
