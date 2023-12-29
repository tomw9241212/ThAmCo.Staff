namespace ThAmCo.Staff.Models {
    public class Order {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public List<Product> Products { get; set; }
        public DateTime RequestedDate { get; set; }
    }
}