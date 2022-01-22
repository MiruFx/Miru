using Corpo.Skeleton.Config;
using Microsoft.Extensions.Options;
using Miru.Scopables;
using Miru.Userfy;

namespace Corpo.Skeleton.Features;

public class CurrentScope : ICurrentScope
{
    private readonly Current _current;
    private readonly AppDbContext _db;
    private readonly ICurrentUser _currentUser;
    private readonly AppOptions _appOptions;

    public CurrentScope(
        Current current,
        AppDbContext db, 
        ICurrentUser currentUser, 
        IOptions<AppOptions> appOptions)
    {
        _current = current;
        _db = db;
        _currentUser = currentUser;
        _appOptions = appOptions.Value;
    }

    public async Task BeforeAsync<TRequest>(TRequest request, CancellationToken ct)
    {
        if (_current.Loaded == false)
        {
            _current.IsAuthenticated = _currentUser.IsAuthenticated;
        
            if (_currentUser.IsAuthenticated)
            {
                _current.User = await _db.Users.ByIdAsync(_currentUser.Id, ct);
            }

            _current.AppOptions = _appOptions;   
        }
    }
}