using System.Threading.Tasks;

namespace Miru.Mailing;

public abstract class Mailable : IMailable
{
    public MailingOptions MailingOptions { get; set; }

    public virtual void Build(Email mail)
    {
    }

    public virtual async Task BuildAsync(Email mail)
    {
        await Task.CompletedTask;
    }
}