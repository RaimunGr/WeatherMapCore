namespace Infra.ApplicationServices.Utility.MessageQueuing.Abstractions
{
    public interface IMessagePusher
    {
        void DeclareQueue(string name);
        void DeclareExchange(string name, string type);
        void BindQueue(string queueName, string exchangeName, string routingKey);
        void Push(string exchangeName, string routingKey, object message, string expiration);
        void DeleteQueue(string name);
        void EmptyQueue(string name);
        void DeleteExchange(string name);
    }
}
