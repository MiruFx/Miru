using MediatR;
using Miru.Security;

namespace Miru.Testing
{
    public static class AuthorizationTestFixtureExtensions
    {
        public static async Task ShouldNotAuthorize<TResult>(
            this ITestFixture _, 
            IRequest<TResult> request)
        {
            await Should.ThrowAsync<UnauthorizedException>(async () => await _.SendAsync(request));
        }
    }
}