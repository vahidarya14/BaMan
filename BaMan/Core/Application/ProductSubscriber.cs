using BaManPubSub.Core.Application.Infra;
using BaManPubSub.Core.Domain;
using BaManPubSub.Core.Infrastructure.DB;

namespace BaManPubSub.Core.Application;

public class ProductSubscriber(AppDbContext context):IAppSubscriber<Product>
{
    public async Task<object?> OnMessageAsync(Product message)
    {
        await context.Products.AddAsync(message);
        await context.SaveChangesAsync();
        return true;
    }

}