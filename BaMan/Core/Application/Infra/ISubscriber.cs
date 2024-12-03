namespace BaManPubSub.Core.Application.Infra;

public interface IAppSubscriber<T> where T:new()
{
    Task<object?> OnMessageAsync(T message);
    
}