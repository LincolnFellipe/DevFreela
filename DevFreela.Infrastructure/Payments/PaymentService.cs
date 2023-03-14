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
        private readonly IMessageBusService _messageBusService;
        private const string queue_name = "Payments"; // Lembrar de colocar uma classe só para as constantes do projeto..
        public PaymentService(IMessageBusService messageBusService) 
        { 
            _messageBusService = messageBusService;
        }

        public void ProcessPayment(PaymentInfoDTO paymentInfoDTO)
        {
            //Transformo em JSON
            var paymentInfoJSON = JsonSerializer.Serialize(paymentInfoDTO);
            //Transforme em bytes para o broker
            var paymentInfoBytes = Encoding.UTF8.GetBytes(paymentInfoJSON);
            //Publicando a mensagem na fila payments
            _messageBusService.Publish(queue_name,paymentInfoBytes);
        }
    }
}
