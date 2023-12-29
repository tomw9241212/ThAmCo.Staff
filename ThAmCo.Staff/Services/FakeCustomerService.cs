using ThAmCo.Staff.Models;

namespace ThAmCo.Staff.Services {
    public class FakeCustomerService : ICustomerService {

        // Make static to have data persist through requests to service, for delete faking
        private static readonly List<CustomerGetDto> _customers = new() {
            new CustomerGetDto { Id = 1, Name = "Bob Dylan", EmailAddress = "bob.d@gmail.com", PhoneNumber = "01234 567890", AddressLine1 = "123 Bobby Street", City = "Newton Aycliffe", County = "County Durham", PostCode = "DL57QE", AvailableFunds = 1583.20 },
            new CustomerGetDto { Id = 2, Name = "Alice Johnson", EmailAddress = "alice.j@example.com", PhoneNumber = "02345 678901", AddressLine1 = "456 Alice Avenue", City = "Bristol", County = "Avon", PostCode = "BS1 1AA", AvailableFunds = 13.24 },
            new CustomerGetDto { Id = 3, Name = "Michael Smith", EmailAddress = "mike.smith@example.net", PhoneNumber = "03456 789012", AddressLine1 = "789 Smith Road", City = "Leeds", County = "West Yorkshire", PostCode = "LS1 4PL", AvailableFunds = 1143.10 },
            new CustomerGetDto { Id = 4, Name = "Sarah Brown", EmailAddress = "sarah.b@example.org", PhoneNumber = "04567 890123", AddressLine1 = "1010 Brown Lane", City = "Cambridge", County = "Cambridgeshire", PostCode = "CB2 3PP", AvailableFunds = 95434.12 },
            new CustomerGetDto { Id = 5, Name = "Emma Wilson", EmailAddress = "emma.wilson@example.co.uk", PhoneNumber = "05678 901234", AddressLine1 = "1212 Wilson Street", City = "Manchester", County = "Greater Manchester", PostCode = "M1 1AE", AvailableFunds = 56782.11 },
            new CustomerGetDto { Id = 6, Name = "James Taylor", EmailAddress = "james.t@example.com", PhoneNumber = "06678 901235", AddressLine1 = "1313 Taylor Road", City = "York", County = "North Yorkshire", PostCode = "YO1 8ZB", AvailableFunds = 92.33 },
            new CustomerGetDto { Id = 7, Name = "Olivia Green", EmailAddress = "olivia.g@example.net", PhoneNumber = "07789 012345", AddressLine1 = "1414 Green Lane", City = "Liverpool", County = "Merseyside", PostCode = "L1 4LN", AvailableFunds = 10.01 },
            new CustomerGetDto { Id = 8, Name = "Ethan White", EmailAddress = "ethan.w@example.co.uk", PhoneNumber = "08890 123456", AddressLine1 = "1515 White Avenue", City = "Norwich", County = "Norfolk", PostCode = "NR1 3QT", AvailableFunds = 321.44 },
            new CustomerGetDto { Id = 9, Name = "Sophia Hall", EmailAddress = "sophia.h@example.com", PhoneNumber = "09901 234567", AddressLine1 = "1616 Hall Street", City = "Sheffield", County = "South Yorkshire", PostCode = "S1 2BJ", AvailableFunds = 0 },
            new CustomerGetDto { Id = 10, Name = "Mason Lee", EmailAddress = "mason.l@example.net", PhoneNumber = "01112 345678", AddressLine1 = "1717 Lee Boulevard", City = "Birmingham", County = "West Midlands", PostCode = "B1 1AA", AvailableFunds = -145.30 }
        };

        public async Task<List<CustomerGetDto>> GetCustomersAsync() {
            return await Task.FromResult(_customers);
        }

        public async Task<CustomerGetDto?> GetCustomerAsync(int id) {
            var customer = _customers.FirstOrDefault(x => x.Id == id);
            return await Task.FromResult(customer);
        }

        public async Task<bool> DeleteCustomerAsync(int id) {
            var customer = _customers.FirstOrDefault(c => c.Id == id);
            if (customer == null) {
                // Customer not found
                return false;
            }

            // Anonymise instead of deleting to keep order link
            customer.Name = "Anonymous";
            customer.EmailAddress = "anonymous@example.com";
            customer.PhoneNumber = "00000 000000";
            customer.AddressLine1 = "N/A";
            customer.AddressLine2 = "N/A";
            customer.City = "N/A";
            customer.County = "N/A";
            customer.PostCode = "N/A";
            customer.AvailableFunds = 0;
            return true;
        }


        private void AnonymiseCustomerData(int id) {
            var customer = _customers.FirstOrDefault(c => c.Id == id);
            if (customer != null) {
                // Replace personal information with anonymous data
                customer.Name = "Anonymous";
                customer.EmailAddress = "anonymous@example.com";
                customer.PhoneNumber = "000-000-0000";
                customer.AddressLine1 = "N/A";
                customer.City = "N/A";
                customer.County = "N/A";
                customer.PostCode = "N/A";
                // Optionally reset or anonymize other fields as needed
                // customer.AvailableFunds = 0; // For example, if you want to reset the funds
            }
        }


    }
}
