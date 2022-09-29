using Miru.Core;

namespace Miru.Storage.Ftp;

public class FtpOptions : ServerOptions
{
    public FtpOptions()
    {
        Port = 21;
    }
}