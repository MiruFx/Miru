namespace Miru.Mailing
{
    public class SmtpOptions
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool Ssl { get; set; }
    }
}