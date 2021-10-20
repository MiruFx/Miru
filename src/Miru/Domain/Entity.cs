using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations.Schema;

namespace Miru.Domain
{
    public abstract class Entity : IEntity, IHasId
    {
        [NotMapped]
        private readonly ConcurrentQueue<IDomainEvent> _domainEvents = new();

        [NotMapped]
        public IProducerConsumerCollection<IDomainEvent> DomainEvents => _domainEvents;

        protected void PublishEvent(IDomainEvent @event)
        {
            _domainEvents.Enqueue(@event);
        }
        
        /// <remarks>
        ///     To help ensure hashcode uniqueness, a carefully selected random number multiplier 
        ///     is used within the calculation.  Goodrich and Tamassia's Data Structures and
        ///     Algorithms in Java asserts that 31, 33, 37, 39 and 41 will produce the fewest number
        ///     of collissions.  See http://computinglife.wordpress.com/2008/11/20/why-do-hash-functions-use-prime-numbers/
        ///     for more information.
        /// </remarks>
        private const int HashMultiplier = 31;

        private int? _cachedHashCode;

        public long Id
        {
            get;
            set;
        }

        public static bool operator ==(Entity x, Entity y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(Entity x, Entity y)
        {
            return (x == y) == false;
        }

        public override bool Equals(object obj)
        {
            if (this.IsNew() == false)
            {
                var entity = obj as Entity;
                return (entity != null) && (IdsAreEqual(entity));
            }

            return ReferenceEquals(this, obj);
        }

        protected bool IdsAreEqual(Entity entity)
        {
            return Equals(Id, entity.Id);
        }

        public override int GetHashCode()
        {
            if (_cachedHashCode.HasValue)
            {
                return _cachedHashCode.Value;
            }

            if (this.IsNew())
            {
                _cachedHashCode = base.GetHashCode();
            }
            else
            {
                unchecked
                {
                    // It's possible for two objects to return the same hash code based on 
                    // identically valued properties, even if they're of two different types, 
                    // so we include the object's type in the hash calculation
                    var hashCode = GetType().GetHashCode();
                    _cachedHashCode = (hashCode * HashMultiplier) ^ Id.GetHashCode();
                }
            }

            return _cachedHashCode.Value;
        }

        public override string ToString()
        {
            if (this.IsNew())
            {
                return $"{base.ToString()}@{GetHashCode()}";
            }

            return $"{base.ToString()}#{Id}";
        }
    }
}