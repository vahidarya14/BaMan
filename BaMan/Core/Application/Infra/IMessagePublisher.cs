namespace BaManPubSub.Core.Application.Infra;

public interface IMessagePublisher<T>
{
    Task PublishMessageAsync(T message, string queueName);
}