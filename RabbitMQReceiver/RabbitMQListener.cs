using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQReceiver
{
    public static class RabbitMQListener
    {
        private static IModel _channel = RabbitMQConnection.GetChannel();

        public static void Consume(string queueName)
        {
            if (string.IsNullOrEmpty(queueName))
                return;
            try
            {
                //_channel.ExchangeDeclare(exchangeName, ExchangeType.Direct, true, false, null);
                _channel.QueueDeclare(queueName, false, false, false, null);
                //_channel.QueueBind(queueName, exchangeName, routeKey, null);
                //_channel.BasicQos(1, 1, false);

                var consumer = new EventingBasicConsumer(_channel);
                consumer.Received += (sender, args) => ReceivedEventHandler(sender, args);

                string consumerTag = _channel.BasicConsume(queueName, false, consumer);
                
                 Console.ReadLine();

                _channel.BasicCancel(consumerTag);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //_objectPool.Return(channel);
                _channel.Close();
                RabbitMQConnection.CloseConnection();
            }
        }

        public static void CloseChannel(string queueName)
        {

        }
        private static void ReceivedEventHandler(object sender, BasicDeliverEventArgs args)
        {
            var body = args.Body.ToArray();
            string message = Encoding.UTF8.GetString(body);
            Console.WriteLine($"message received {message}");
            _channel.BasicAck(args.DeliveryTag, false);
        }
    }
}
