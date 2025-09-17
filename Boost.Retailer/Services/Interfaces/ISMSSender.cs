namespace Boost.Retail.Services.Interfaces
{
    public interface ISMSSender
    {
        Task SendSMSAsync(string phoneNumber, string message);
    }
}
