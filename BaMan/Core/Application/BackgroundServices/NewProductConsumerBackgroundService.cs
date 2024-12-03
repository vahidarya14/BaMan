using System.Text;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using BaManPubSub.Core.Application.Infra;
using BaManPubSub.Core.Domain;
using BaManPubSub.Core.Infrastructure.DB;

namespace BaManPubSub.Core.Application.BackgroundServices;

public class NewProductConsumerBackgroundService(IOptions<RabbitSetting> rabbitMqSetting, IServiceProvider serviceProvider) 
                                                                                                        : BackgroundService
{
    private readonly RabbitSetting _rabbitMqSetting = rabbitMqSetting.Value;
    private IConnection _connection;
    private IChannel _channel;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory
        {
            HostName = _rabbitMqSetting.HostName,
            UserName = _rabbitMqSetting.UserName,
            Password = _rabbitMqSetting.Password
        };
        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();


        await _channel.ExchangeDeclareAsync(exchange: QueueNames.ProductExchangeQueue, type: ExchangeType.Fanout);


        var queueDeclareResult = await _channel.QueueDeclareAsync();
        var queueName = queueDeclareResult.QueueName;
        await _channel.QueueBindAsync(queue: queueName, exchange: QueueNames.ProductExchangeQueue,
            routingKey: string.Empty);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            await ProcessMessageAsync(message);
        };

        await _channel.BasicConsumeAsync(queueName, autoAck: true, consumer: consumer);
    }

    private async Task<bool> ProcessMessageAsync(string message)
    {
        try
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<IAppSubscriber<Product>>();

            var link = JsonConvert.DeserializeObject<Product>(message);
            await context.OnMessageAsync(link);

            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public override void Dispose()
    {
        _channel.CloseAsync();
        _connection.CloseAsync();
        base.Dispose();
    }
}