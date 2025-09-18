using System.Net.Mail;
using System.Net;
using Boost.Retail.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Boost.Retail.Services
{
    
    public class SMSSender : ISMSSender
    {
        private readonly IConfiguration _config;

        public SMSSender(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendSMSAsync(string phoneNuber, string message)
        {
            
        }
    }
}
