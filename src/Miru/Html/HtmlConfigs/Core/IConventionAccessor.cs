using System.Collections.Generic;

namespace Miru.Html.HtmlConfigs.Core;

public interface IConventionAccessor
{
    IList<Modifier> Modifiers { get; }
}