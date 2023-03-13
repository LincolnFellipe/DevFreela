using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using DevFreela.Core.Services;
using DevFreela.Core.DTOs;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace DevFreela.Infrastructure.Payments
{
    public class PaymentService : IPaymentService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _paymentsBaseUrl;
        public PaymentService(IHttpClientFactory httpClientFactory, IConfiguration configuration) 
        { 
        _httpClientFactory = httpClientFactory;
        _paymentsBaseUrl = configuration.GetSection("Services:Payments").Value;
        }

        public async Task<bool> ProcessPayment(PaymentInfoDTO paymentInfoDTO)
        {
            var url = $"{_paymentsBaseUrl}/api/payments";
            var paymentInfoJSON = JsonSerializer.Serialize(paymentInfoDTO);
            var paymentInfoContent = new StringContent(paymentInfoJSON,Encoding.UTF8,"application/json");

            var httpClient = _httpClientFactory.CreateClient("Payments");
            var response = await httpClient.PostAsync(url, paymentInfoContent);

            return response.IsSuccessStatusCode;
        }
    }
}
