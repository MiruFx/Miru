namespace MiruNext.Features;

public abstract class Endpoint2<TRequest> : Endpoint<TRequest> where TRequest : new()
{
    public async Task RespondAsync<TModel>(TModel response)
    {
        await this.SendViewAsync(GetType().Name, response);
    }
}