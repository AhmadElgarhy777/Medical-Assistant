namespace Services.PaymentServices
{
    public interface IPaymentService
    {
        Task<string> GetAuthTokenAsync();
        Task<int> CreateOrderAsync(string authToken, decimal amount);
        Task<string> GetPaymentKeyAsync(string authToken, int orderId, decimal amount);
        string GetPaymentUrl(string paymentKey);
    }

}