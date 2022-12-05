using Miru.Html.HtmlConfigs.Core;

namespace Miru.Html.HtmlConfigs;

public class HtmlConventions
{
    public ITagConventions Inputs { get; } = new TagConventions();
    public ITagConventions Labels { get; } = new TagConventions();
    public ITagConventions Displays { get; } = new TagConventions();
    public ITagConventions DisplayLabels { get; } = new TagConventions();
    public ITagConventions FormSummaries { get; } = new TagConventions();
    public ITagConventions Tables { get; } = new TagConventions();
    public ITagConventions TableHeaders { get; } = new TagConventions();
    public ITagConventions TableCells { get; } = new TagConventions();
    public ITagConventions Selects { get; } = new TagConventions();
    public ITagConventions Submits { get; } = new TagConventions();
    public ITagConventions ValidationMessages { get; } = new TagConventions();
    public ITagConventions Forms { get; } = new TagConventions();
}