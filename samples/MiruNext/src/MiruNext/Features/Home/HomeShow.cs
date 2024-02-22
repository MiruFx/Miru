using MiruNext.Framework;

namespace MiruNext.Features.Home;

public class HomeShow
{
    [AllowAnonymous]
    [HttpGet("/")]
    public class Handler : EndpointWithoutRequest
    {
        public override async Task HandleAsync(CancellationToken ct)
        {
            await this.View<Show>();
        }
    }
    
    [AllowAnonymous]
    [HttpGet("/2")]
    public class Handler2 : EndpointWithoutRequest
    {
        public override async Task HandleAsync(CancellationToken ct)
        {
            await this.View<Show2>();
        }
    }
}
