namespace MiruNext.Features.Home;

public class HomeIndex
{
    [AllowAnonymous]
    [HttpGet("/")]
    public class Index : EndpointWithoutRequest
    {
        public override async Task HandleAsync(CancellationToken ct)
        {
            await this.SendViewAsync("Index", this);
        }
    }
}