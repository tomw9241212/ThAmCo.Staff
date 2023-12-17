using ThAmCo.Staff.Models;

namespace ThAmCo.Staff.ViewModel {
    public class OrderCustomerViewModel {
        public int OrderId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public DateTime SubmittedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
