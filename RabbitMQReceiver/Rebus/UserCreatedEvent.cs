using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQReceiver.Rebus
{
    public class UserCreatedEvent
    {
        public UserCreatedEvent(string userName)
        {
            UserName = userName;
        }
        public string UserName { get; }
    }

    public class UserDeletedEvent
    {
        public UserDeletedEvent(string userName)
        {
            this.UserName = userName;
        }
        public string UserName { get; set; }
    }
}
