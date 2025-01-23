using Domain.Models.Primitives;

namespace Domain.Models.DBModels
{
    public class PaymentPlanDBModel : EntityDBModel
    {
        public string Title { get; set; }
        public decimal MonthlyPrice { get; set; }
    }
}
