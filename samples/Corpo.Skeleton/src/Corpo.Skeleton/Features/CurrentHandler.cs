using Microsoft.Extensions.Options;
using Miru.Currentable;
using Miru.Userfy;

namespace Corpo.Skeleton.Features;

public class CurrentHandler(
    Current current,
    AppDbContext db,
    ICurrentUser currentUser,
    IOptions<AppOptions> appOptions)
    : ICurrentHandler
{
    private readonly AppOptions _appOptions = appOptions.Value;

    public async Task Handle<TRequest>(TRequest request, CancellationToken ct)
    {
        if (current.Loaded == false)
        {
            current.IsAuthenticated = currentUser.IsAuthenticated;
        
            if (currentUser.IsAuthenticated)
            {
                current.User = await db.Users.ByIdAsync(currentUser.Id, ct);
            }

            current.AppOptions = _appOptions;   
        }
    }
}