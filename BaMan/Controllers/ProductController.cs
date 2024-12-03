using Microsoft.AspNetCore.Mvc;
using BaManPubSub.Core.Application;

namespace BaManPubSub.Controllers;

[ApiController]
[Route("product")]
public class ProductController(ProductService productService) : ControllerBase
{

    [HttpGet("{id}")]
    public async Task<object> Get(long id) => await productService.Get(id);

    [HttpPost]
    public async Task Get(ProductCreationDto shortUri) => await productService.Add(shortUri);


}
