using FluentEmail.Core.Models;
using Newtonsoft.Json;

namespace Miru.Mailing
{
    public class Email : EmailData
    {
        [JsonIgnore]
        public EmailTemplate Template { get; set; }
    }
}