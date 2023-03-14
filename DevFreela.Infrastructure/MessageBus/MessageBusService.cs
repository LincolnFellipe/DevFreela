﻿using DevFreela.Core.Services;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevFreela.Infrastructure.MessageBus
{
    public class MessageBusService : IMessageBusService
    {
        private readonly ConnectionFactory _factory;
        public MessageBusService()
        {
            _factory = new ConnectionFactory 
            { 
                HostName = "localhost" 
            };
        }
        public void Publish(string queue, byte[] message)
        {
            using (var connection = _factory.CreateConnection()) 
            {
                using (var channel = connection.CreateModel()) 
                { 
                    //Garantindo que a fila esteja criada
                    channel.QueueDeclare(queue: queue, durable: false, exclusive: false, autoDelete: false, arguments: null);
                    //Se não tiver criada, criamos
                    channel.BasicPublish(exchange: "", routingKey: queue, basicProperties: null, body: message);                   

                }
            
            }
        }
    }
}
