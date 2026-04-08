

//using Newtonsoft.Json;
//using System.Net.Http;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.Extensions.Configuration;

//namespace HostelManage.Services
//{
//    public class PaymentService : IPaymentService
//    {
//        private readonly IConfiguration _configuration;
//        private readonly HttpClient _httpClient;

//        public PaymentService(IConfiguration configuration, HttpClient httpClient)
//        {
//            _configuration = configuration;
//            _httpClient = httpClient;
//        }

//        /// <summary>
//        /// Initiates a payment request to Khalti.
//        /// </summary>
//        public async Task<string> InitiatePaymentAsync(decimal amount, string orderId, string orderName, string customerName, string customerEmail, string customerPhone)
//        {
//            var khaltiApiKey = _configuration["Khalti:ApiKey"];
//            var khaltiInitiateUrl = "https://dev.khalti.com/api/v2/epayment/initiate/";

//            var data = new
//            {
//                return_url = "http://localhost:3000/verify-payment",  // Update return URL to our verification API
//                website_url = "http://localhost:5178/",
//                amount = amount * 100, // Convert to paisa
//                purchase_order_id = orderId,
//                purchase_order_name = orderName,
//                customer_info = new
//                {
//                    name = customerName,
//                    email = customerEmail,
//                    phone = customerPhone
//                }
//            };
//            Console.WriteLine($"Return URL: {data.return_url}");

//            var jsonPayload = JsonConvert.SerializeObject(data);
//            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

//            var requestMessage = new HttpRequestMessage(HttpMethod.Post, khaltiInitiateUrl)
//            {
//                Content = content
//            };
//            requestMessage.Headers.Add("Authorization", $"Key {khaltiApiKey}");

//            var response = await _httpClient.SendAsync(requestMessage);

//            if (!response.IsSuccessStatusCode)
//            {
//                return null; // Payment initiation failed
//            }

//            var responseContent = await response.Content.ReadAsStringAsync();
//            var khaltiResponse = JsonConvert.DeserializeObject<dynamic>(responseContent);

//            return khaltiResponse?.payment_url; // Return the payment URL from the response
//        }

//        /// <summary>
//        /// Verifies a payment using the pidx received from Khalti.
//        /// </summary>
//        public async Task<bool> VerifyPaymentAsync(string pidx, decimal amount)
//        {
//            var khaltiApiKey = _configuration["Khalti:ApiKey"];
//            var khaltiVerificationUrl = "https://dev.khalti.com/api/v2/epayment/lookup/";

//            var data = new
//            {
//                pidx = pidx,
//            };

//            var jsonPayload = JsonConvert.SerializeObject(data);
//            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

//            var requestMessage = new HttpRequestMessage(HttpMethod.Post, khaltiVerificationUrl)
//            {
//                Content = content
//            };
//            requestMessage.Headers.Add("Authorization", $"Key {khaltiApiKey}");

//            var response = await _httpClient.SendAsync(requestMessage);

//            if (!response.IsSuccessStatusCode)
//            {
//                return false; // Payment verification failed
//            }

//            var responseContent = await response.Content.ReadAsStringAsync();
//            var khaltiResponse = JsonConvert.DeserializeObject<dynamic>(responseContent);

//            return khaltiResponse != null && khaltiResponse.status == "Completed"; // Verify if the payment was successful
//        }
//    }
//}




using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HostelManage.Data;

namespace HostelManage.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly HttpClient _httpClient;
        private readonly AppDbContext _context;

        private const string SECRET_KEY = "2829e52e041b435fb9393e592882385d";

        public PaymentService(HttpClient httpClient, AppDbContext context)
        {
            _httpClient = httpClient;
            _context = context;
        }

        public async Task<string> InitiatePaymentAsync(decimal amount, string orderId, string orderName)
        {
            var payload = new
            {
                return_url = "http://localhost:3000/payment-success",
                website_url = "http://localhost:3000",
                amount,
                purchase_order_id = orderId,
                purchase_order_name = orderName
            };

            var json = JsonConvert.SerializeObject(payload);

            var request = new HttpRequestMessage(HttpMethod.Post, "https://a.khalti.com/api/v2/epayment/initiate/")
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            request.Headers.Add("Authorization", $"Key {SECRET_KEY}");

            var response = await _httpClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            dynamic result = JsonConvert.DeserializeObject(responseContent);

            return result?.payment_url;
        }

        public async Task<bool> VerifyPaymentAsync(string pidx)
        {
            var payload = new { pidx };

            var json = JsonConvert.SerializeObject(payload);

            var request = new HttpRequestMessage(HttpMethod.Post, "https://a.khalti.com/api/v2/epayment/lookup/")
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            request.Headers.Add("Authorization", $"Key {SECRET_KEY}");

            var response = await _httpClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            dynamic result = JsonConvert.DeserializeObject(responseContent);

            return result?.status == "Completed";
        }

        public async Task MarkBookingAsPaid(int bookingId)
        {
            // ✅ FIXED DbSet name
            var booking = await _context.Booking.FindAsync(bookingId);

            // ✅ FIXED: Status is int (1 = Paid)
            if (booking == null || booking.Status == 1)
                return;

            // ✅ mark as paid
            booking.Status = 1;

            // ✅ FIXED HostelID name
            var hostel = await _context.Hostel.FindAsync(booking.HostelID);

            if (hostel != null)
            {
                if (booking.RoomType == hostel.RoomType1 && hostel.RoomType1Count > 0)
                    hostel.RoomType1Count--;

                else if (booking.RoomType == hostel.RoomType2 && hostel.RoomType2Count > 0)
                    hostel.RoomType2Count--;

                else if (booking.RoomType == hostel.RoomType3 && hostel.RoomType3Count > 0)
                    hostel.RoomType3Count--;
            }

            await _context.SaveChangesAsync();
        }
    }
}