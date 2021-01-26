namespace Miru.Html
{
    public static class ElementNamingExtensions
    {
        public static string SummaryId<TModel>(this ElementNaming naming, TModel model)
        {
            return $"{naming.Id(model)}-summary";
        }
    }
}