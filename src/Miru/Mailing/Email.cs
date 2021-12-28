using System.Text.Json.Serialization;
using FluentEmail.Core.Models;

namespace Miru.Mailing
{
    public class Email : EmailData
    {
        [Newtonsoft.Json.JsonIgnore, JsonIgnore]
        public EmailTemplate Template { get; set; }
    }
}