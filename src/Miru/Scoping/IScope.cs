using System.Threading;
using System.Threading.Tasks;

namespace Miru.Scoping
{
    public interface IScope
    {
        Task Handle(object scopedRequest, CancellationToken ct);
    }
}