namespace Miru.Mailing
{
    public abstract class Mailable : IMailable
    {
        public MailingOptions MailingOptions { get; set; }

        public abstract void Build(Email mail);
    }
}