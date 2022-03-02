<!-- 
intro

IInactivable

Entity : IInactivable

AddInactivable

.AsNoFilter()

-->

# Userfy Invitable

It adds support to a Miru project for adding an User using invitation by email.

The invited User sets it own password.

## Requirements

.NET >= 6.0  

Miru >= 0.8.22

## Installation

Add the library into your Miru App project:  
`miru app dotnet add package Miru.Userfy.Invitable`

Build the App project:
`miru app dotnet build`

Run UserfyInvitable installer:
`miru userfy.invitable.install`

It will add into your project:
```
/{app}/Database/Migrations/{datetime}_AlterUsersAddInvitable.cs

/{app}/Admin/Users/Invitations/_Invitation.mail.cshtml

/{app}/Users/Invitations/Register.cshtml
/{app}/Users/Invitations/UserRegister.cs
```

If you use different paths, you can move the files into your directory conventions.

## Configuration

### User Entity

Decorate your entity `User.cs` with `IInvitable`:

```csharp
public class User : UserfyUser, IInvitable
{
    public string InvitationToken { get; set; }
    public DateTime? InvitationAcceptedAt { get; set;  }
    
    // from IInactivable
    public bool IsInactive { get; set; }
}
```

Note that IInvitable depends on IInactivable.

### Configure Services

Add into your `Startup.cs` or `Program.cs` ConfigureServices:

```csharp
.AddUserfyInvitable<User>(opt =>
{
    // set true if the email will be queued and send later. otherwise, it will be sent now
    opt.SendEmailLater = true;
    
    // customize the invitation email
    opt.MailConfig((mail, user) =>
    {
        mail.TemplateAt("/Features/Accounts", "_Invitation", user);
    });
})
```

### Features

