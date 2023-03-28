using Microsoft.AspNetCore.Http;

namespace Miru.Urls;

public class UrlOptions
{
    public QueryStringConfig QueryStrings { get; } = new();
        
    public string Base { get; set; }
}