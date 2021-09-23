using RabbitMQ.Client;

namespace Infra.ApplicationServices.Utility.MessageQueuing.Implementations
{
    public abstract class BaseMessageHandler
    {
        private byte _counter = 0;
        protected readonly string _amqpUri;
        protected IModel _channel;

        public BaseMessageHandler(string amqpUri)
        {
            _amqpUri = amqpUri;
        }

        protected void CreateChannel()
        {
            _counter++;

            if (_channel != null && _channel.IsOpen && _counter < byte.MaxValue)
            {
                return;
            }

            if (_counter == byte.MaxValue)
            {
                _counter = 0;
                DisposeChannel(_channel);
            }

            var connection = CreateConnection();
            _channel = connection.CreateModel();
        }

        private IConnection CreateConnection()
        {
            var splittedByAt = _amqpUri.Split("@");
            var splittedBySlashes = splittedByAt[0].Split(@"//");
            var splittedByDoubleDots = splittedBySlashes[1].Split(":");
            var username = splittedByDoubleDots[0];
            var password = splittedByDoubleDots[1];
            var splittedHostUriByDoubleDots = splittedByAt[1].Split(":");
            var hostName = splittedHostUriByDoubleDots[0];
            var port = int.Parse(splittedHostUriByDoubleDots[1]);

            var factory = new ConnectionFactory
            {
                UserName = username,
                Password = password,
                HostName = hostName,
                Port = port,
            };

            return factory.CreateConnection();
        }

        private static void DisposeChannel(IModel channel)
        {
            if (channel is object)
            {
                channel.Close();
                channel.Dispose();
            }
        }
    }
}
