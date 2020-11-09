using System.Linq;
using Miru.Domain;
using Miru.Html;

namespace Mong.Domain
{
    public class Provider : Entity, ILookupable
    {
        public string Amounts { get; set; }
        public string Name { get; set; }

        public bool SupportsAmount(decimal requestAmount) => AllAmounts().Contains(requestAmount.ToString());

        public string[] AllAmounts() => Amounts.Split(",");

        public string Value => Id.ToString();
        public string Display => Name;
    }
}