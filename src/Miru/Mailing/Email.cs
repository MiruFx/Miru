using FluentEmail.Core.Models;

namespace Miru.Mailing
{
    public class Email : EmailData
    {
        public EmailTemplate Template { get; set; }
    }
}