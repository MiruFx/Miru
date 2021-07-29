namespace Miru.Storages.Ftp
{
    public class FtpOptions
    {
        public string FtpServer { get; set; }
        public string FtpUser { get; set; }
        public string FtpPassword { get; set; }
        public int FtpPort { get; set; } = 21;
    }
}