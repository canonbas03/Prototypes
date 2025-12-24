namespace HotelMVCPrototype.Models
{
    public class ServiceRequestItem
    {
        public int Id { get; set; }

        public int ServiceRequestId { get; set; }
        public ServiceRequest ServiceRequest { get; set; }

        public int RequestItemId { get; set; }
        public RequestItem RequestItem { get; set; } = null!;
        public int Quantity { get; set; }
    }
}
