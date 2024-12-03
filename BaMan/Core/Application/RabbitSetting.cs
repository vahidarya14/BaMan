namespace BaManPubSub.Core.Application;

public class RabbitSetting
{
    public string? HostName { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }
}

public static class QueueNames
{
    public static string ProductExchangeQueue => "exchange_products";
}
