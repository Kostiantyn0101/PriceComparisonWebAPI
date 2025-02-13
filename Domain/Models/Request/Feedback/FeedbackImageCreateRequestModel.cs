using Microsoft.AspNetCore.Http;

namespace Domain.Models.Request.Feedback
{
    public class FeedbackImageCreateRequestModel
    {
        public int FeedbackId { get; set; }
        public List<IFormFile> Images { get; set; }
    }
}
