using Domain.Models.Primitives;

namespace Domain.Models.DBModels
{
    public class SellerPaymentPlanDBModel : EntityDBModel
    {
        public int SellerId { get; set; }
        public SellerDBModel Seller { get; set; }

        public int PaymentPlanId { get; set; }
        public PaymentPlanDBModel PaymentPlan { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public bool IsActive { get; set; }
    }
}
