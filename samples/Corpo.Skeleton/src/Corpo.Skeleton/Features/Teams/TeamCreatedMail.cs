namespace Corpo.Skeleton.Features.Teams;

public class TeamCreatedMail : Mailable
{
    private readonly User _user;

    public TeamCreatedMail(User user)
    {
        _user = user;
    }

    public override void Build(Email mail)
    {
        mail.To(_user.Email, _user.Email)
            .Subject("Email Subject")
            .Template("_Created");
    }
}