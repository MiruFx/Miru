using System;
using System.Linq.Expressions;

namespace Miru.Domain
{
    public class Effective
    {
        public static Expression<Func<IEffectivable, bool>> On(DateTime date)
        {
            return x => (x.Effective.From.HasValue == false || date >= x.Effective.From.Value) && (x.Effective.To.HasValue == false || date <= x.Effective.To.Value);
        }

        public Effective(DateTime from, DateTime to)
        {
            To = to;
            From = from;
        }

        public Effective(DateTime from)
        {
            From = from;
            To = null;
        }

        private Effective()
        {
        }

        public DateTime? From { get; private set; }
        public DateTime? To { get; private set; }

        public bool IsEffectiveFor(DateTime date)
        {
            if (From.HasValue && date < From)
                return false;

            if (To.HasValue && date > To)
                return false;

            return true;
        }
    }
}