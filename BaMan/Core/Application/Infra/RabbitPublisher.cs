using System.Text;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using BaManPubSub.Core.Application.Infra;

namespace BaManPubSub.Core.Application;

public class RabbitPublisher<T>(IOptions<RabbitSetting> rabbitMqSetting) : IMessagePublisher<T>
{
    private readonly RabbitSetting _rabbitMqSetting = rabbitMqSetting.Value;

    public async Task PublishMessageAsync(T message, string queueName)
    {

        var factory = new ConnectionFactory
        {
            HostName = _rabbitMqSetting.HostName,
            UserName = _rabbitMqSetting.UserName,
            Password = _rabbitMqSetting.Password
        };

        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();
        await channel.ExchangeDeclareAsync(exchange: QueueNames.ProductExchangeQueue, type: ExchangeType.Fanout);

        var messageJson = JsonConvert.SerializeObject(message);
        var body = Encoding.UTF8.GetBytes(messageJson);


        await channel.BasicPublishAsync(exchange: QueueNames.ProductExchangeQueue, routingKey: string.Empty, body: body);
    }
}