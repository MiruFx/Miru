<!--
Introduction
  fluentemail
  mailable, template, sender
  make
TODO: Adding .AddMailing
Mailable
  from
  to
  subject
  body
  attachment
    CANT QUEUE email with attachment
Template
  TODO: _layout template
Sending
  TODO: smtp driver
  TODO: storage driver
TODO: Testing

https://playground.dyspatch.io/#/oxygen/new
-->

[[toc]]

# Mailing

Miru has some Mailing facilities on top of [FluentEmail](https://github.com/lukencode/FluentEmail).

## Create

An easy way to create a Mailable and an email template is using a Maker from MiruCli:

```shell
miru make:mail Accounts Account Created
```

![](/Queueing-Make.png)

## Mailable

A Mailable is the object that will be passed to the Sender or to the Queue. It has the email properties like subject, body, sender, receiver, and etc.

@[code lang=csharp transcludeWith=#mailable](@/samples/Mong/src/Mong/Features/Accounts/AccountRegisteredMail.cs)

## Template

The Template is a Razor file that will be transformed into html before send the email. Markdown can be used between the tags ```<markdown></markdown>```.

@[code lang=html](@/samples/Mong/src/Mong/Features/Accounts/AccountRegisteredMail.cshtml)

## Sending

Send email is done using ```Miru.Mailing.IMailer```:

```csharp
var user = _db.Users.ById(id);

// send now synchronously
await _mailer.SendNowAsync(new AccountRegisteredMail(user));

// queue to send asynchronously
await _mailer.SendLaterAsync(new AccountRegisteredMail(user));
```