namespace Miru.Mvc
{
    public class Lookup
    {
        public Lookup(object id, object description)
        {
            Id = id.ToString();
            Description = description.ToString();
        }

        public string Id { get; set; }
        public string Description { get; set; }
    }
}