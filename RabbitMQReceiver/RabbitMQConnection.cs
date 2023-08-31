using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQReceiver
{
    public class RabbitMQConnection
    {
        private static IModel _connection;
        private RabbitMQConnection() { }
        public static IModel GetChannel()
        {
            if (_connection == null)
                _connection = Create();
            return _connection;
        }

        public static void CloseConnection()
        {
            if (_connection != null)
                _connection.Close();
        }

        private static IModel Create()
        {
            ConnectionFactory factory = new ConnectionFactory();
            factory.Uri = new Uri("amqp://localhost");
            //factory.ClientProvidedName = "Client-Provide-Name-1";
            IConnection conn = factory.CreateConnection();
            return conn.CreateModel();
        }

        private bool Return(IModel obj)
        {
            if (obj.IsOpen)
                return true;
            else
            {
                obj?.Dispose();
                return false;
            }
        }
    }
}
