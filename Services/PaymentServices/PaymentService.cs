using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;

namespace Services.PaymentServices
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public PaymentService(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }

        // 1. جيب الـ Auth Token من Paymob
        public async Task<string> GetAuthTokenAsync()
        {
            var apiKey = _configuration["Paymob:ApiKey"];
            var body = new { api_key = apiKey };
            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(
                "https://accept.paymob.com/api/auth/tokens", content);

            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<JsonElement>(responseString);
            return result.GetProperty("token").GetString()!;
        }

        // 2. عمل Order في Paymob
        public async Task<int> CreateOrderAsync(string authToken, decimal amount)
        {
            var body = new
            {
                auth_token = authToken,
                delivery_needed = false,
                amount_cents = (int)(amount * 100),
                currency = "EGP",
                items = new object[] { }
            };

            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(
                "https://accept.paymob.com/api/ecommerce/orders", content);

            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<JsonElement>(responseString);
            return result.GetProperty("id").GetInt32();
        }

        // 3. جيب الـ Payment Key
        public async Task<string> GetPaymentKeyAsync(string authToken, int orderId, decimal amount)
        {
            var integrationId = int.Parse(_configuration["Paymob:IntegrationId"]!);

            var body = new
            {
                auth_token = authToken,
                amount_cents = (int)(amount * 100),
                expiration = 3600,
                order_id = orderId,
                billing_data = new
                {
                    email = "patient@medix.com",
                    first_name = "Medix",
                    last_name = "Patient",
                    phone_number = "01000000000",
                    apartment = "NA",
                    floor = "NA",
                    street = "NA",
                    building = "NA",
                    shipping_method = "NA",
                    postal_code = "NA",
                    city = "Cairo",
                    country = "EG",
                    state = "NA"
                },
                currency = "EGP",
                integration_id = integrationId
            };

            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(
                "https://accept.paymob.com/api/acceptance/payment_keys", content);

            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<JsonElement>(responseString);
            return result.GetProperty("token").GetString()!;
        }

        // 4. جيب رابط الدفع
        public string GetPaymentUrl(string paymentKey)
        {
            return $"https://accept.paymob.com/api/acceptance/iframes/926213?payment_token={paymentKey}";
        }
    }
}