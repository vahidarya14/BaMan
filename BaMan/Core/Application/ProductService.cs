using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using BaManPubSub.Core.Application.Infra;
using BaManPubSub.Core.Domain;
using BaManPubSub.Core.Infrastructure;
using BaManPubSub.Core.Infrastructure.DB;

namespace BaManPubSub.Core.Application;

public class ProductService(AppDbContext context, IMessagePublisher<Product> publisher)
{
    public async Task<long> Add(ProductCreationDto request)
    {
        var link = new Product
        {
            Name = request.Name,    
            Description = request.Description,
            Price = request.price
        };
        publisher.PublishMessageAsync(link, QueueNames.ProductExchangeQueue);


        return link.Id;
    }

    public async Task<Product> Get(long id)
    {
        return await context.Products.FirstAsync(x => x.Id == id)!;
    }
}

public record ProductCreationDto(string Name, string Description,long price);