using System.Collections.Generic;
using MiruNext.Framework.Validation;

namespace MiruNext.Framework.Views;

public class PageState
{
    public List<ValidationError> Errors { get; set; } = new();
}