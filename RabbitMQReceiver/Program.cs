using Rebus.Activation;
using Rebus.Config;
using System;
using RabbitMQReceiver.Rebus;
using Rebus.Sagas;
using Rebus.Retry.Simple;
using Rebus.Routing.TypeBased;

namespace RabbitMQReceiver
{
    class Program
    {
        private static readonly string GenericMessageQueue = "Generic_Message_Queue";
        private static string TestQueue = "testQueue";
        private static string SQLConnectionString = " Data Source=KFWIMTHUS\\SQL2016;Initial Catalog=MessageQueue;User Id=sa;Password=bqeko@123;MultipleActiveResultSets=true;Enlist=true;TrustServerCertificate=True";
        private static string RabbitConnectionString = "amqp://localhost";
        static void Main(string[] args)
        {
            //RabbitMQListener.Consume(GenericMessageQueue);


            using var activator = new BuiltinHandlerActivator();
            activator.Register(() => new UserCreatedEventHandler());

            var activator2 = new BuiltinHandlerActivator();
            activator2.Register(() => new UserDeletedEventHandler());

            var subscriber = Configure.With(activator)
                .Transport(t => t.UseRabbitMq(RabbitConnectionString, GenericMessageQueue))
                .Start();


            var subscriber2 = Configure.With(activator2)
                .Transport(t =>
                {
                    var options = new SqlServerTransportOptions(SQLConnectionString);
                    t.UseSqlServer(options, TestQueue);
                })
                .Routing(r => r.TypeBased().Map<UserDeletedEvent>(TestQueue))
                .Options(o => o.SimpleRetryStrategy(maxDeliveryAttempts: 5, secondLevelRetriesEnabled: true))
                .Subscriptions(t => t.StoreInSqlServer(SQLConnectionString, "Subscriptions", isCentralized: true))
                .Start();

            //using var sqlBus = Configure.OneWayClient()
            //    .Transport(t =>
            //    {
            //        var options = new SqlServerTransportOptions(SQLConnectionString);
            //        t.UseSqlServerAsOneWayClient(options);
            //    })
            //    .Subscriptions(t => t.StoreInSqlServer(SQLConnectionString, "Subscriptions", isCentralized: true))
            //    .Start();

            /* redundant code 
            subscriber.Subscribe<UserCreatedEvent>();
            subscriber2.Subscribe<UserDeletedEvent>();
            */

            activator.Bus.SendLocal(new UserCreatedEvent("Imtiyaz Hussain"));
            activator2.Bus.SendLocal(new UserDeletedEvent("Danish Wadoo"));

            Console.ReadLine();
        }
    }
}
