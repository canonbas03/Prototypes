namespace HotelMVCPrototype.Models
{
    public class ServiceRequestItem
    {
        public int Id { get; set; }

        public int ServiceRequestId { get; set; }
        public ServiceRequest ServiceRequest { get; set; }

        public string ItemName { get; set; }   // Toothbrush, Iron, etc.
        public int Quantity { get; set; }
    }
}
