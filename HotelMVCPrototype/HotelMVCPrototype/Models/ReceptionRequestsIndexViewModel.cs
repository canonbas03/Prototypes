namespace HotelMVCPrototype.Models
{
    public class ReceptionRequestsIndexViewModel
    {
        public List<ServiceRequest> NewRequests { get; set; } = new();
        public List<ServiceRequest> CompletedRequests { get; set; } = new();
    }

}
