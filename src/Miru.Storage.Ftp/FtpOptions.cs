using Miru.Core;

namespace Miru.Storage.Ftp;

public class FtpOptions : ServerOptions
{
    public string Root { get; set; }
    
    public FtpOptions()
    {
        Port = 21;
    }
}