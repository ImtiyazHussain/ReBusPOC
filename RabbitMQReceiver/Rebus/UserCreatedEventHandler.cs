using Rebus.Handlers;
using Rebus.Retry.Simple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQReceiver.Rebus
{
    public class UserCreatedEventHandler : IHandleMessages<UserCreatedEvent>, IHandleMessages<IFailed<UserCreatedEvent>>
    {
        public async Task Handle(UserCreatedEvent message)
        {
            Console.WriteLine($"{nameof(UserCreatedEvent)} received. Username: {message.UserName}");
        }

        public Task Handle(IFailed<UserCreatedEvent> message)
        {
            throw new NotImplementedException();
        }
    }

    public class UserDeletedEventHandler : IHandleMessages<UserDeletedEvent>, IHandleMessages<IFailed<UserDeletedEvent>>
    {
        public async Task Handle(UserDeletedEvent message)
        {
            Console.WriteLine($"{nameof(UserDeletedEvent)} received. Username: {message.UserName}");
        }
        public Task Handle(IFailed<UserDeletedEvent> message)
        {
            throw new NotImplementedException();
        }

    }
}
