using Infra.ApplicationServices.Utility.MessageQueuing.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using RabbitMQ.Client;
using System;
using System.Text;

namespace Infra.ApplicationServices.Utility.MessageQueuing.Implementations
{
    public sealed class MessagePusher : BaseMessageHandler, IMessagePusher
    {
        public MessagePusher(string amqpUri)
            : base(amqpUri)
        { }

        public void BindQueue(string queueName, string exchangeName, string routingKey)
        {
            CreateChannel();

            _channel.QueueBind(queueName, exchangeName, routingKey, null);
        }

        public void DeclareExchange(string name, string type)
        {
            CreateChannel();

            _channel.ExchangeDeclare(name, type, true, false, null);
        }

        public void DeclareQueue(string name)
        {
            CreateChannel();

            _channel.QueueDeclare(name, true, false, false, null);
        }

        public void Push(string exchangeName, string routingKey, object message, string expiration)
        {
            CreateChannel();

            var props = _channel.CreateBasicProperties();
            props.AppId = AppDomain.CurrentDomain.FriendlyName;
            props.ContentEncoding = Encoding.UTF8.BodyName;
            props.ContentType = "application/json";
            props.Expiration = expiration;

            var byteMessage = CreateExchangeBody(message);

            _channel.BasicPublish(exchangeName, routingKey, props, byteMessage);
        }

        public void DeleteQueue(string name)
        {
            CreateChannel();

            _channel.QueueDelete(name, false, true);
        }

        public void EmptyQueue(string name)
        {
            CreateChannel();

            _channel.QueuePurge(name);
        }

        public void DeleteExchange(string name)
        {
            CreateChannel();

            _channel.ExchangeDelete(name, false);
        }

        private byte[] CreateExchangeBody(object message)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
            };
            settings.Converters.Add(new StringEnumConverter());

            var jsonMessage = JsonConvert.SerializeObject(message, settings);
            var body = Encoding.UTF8.GetBytes(jsonMessage);
            return body;
        }
    }
}
