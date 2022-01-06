namespace Scripts.StubsExport;

public class MailableStubExport : StubExport
{
    public MailableStubExport(StubParams param) : base(param)
    {
    }

    public override void Export()
    {
        ExportFile(
            Params.SkeletonDir / "src" / "Corpo.Skeleton" / "Features" / "Teams" / "TeamCreatedMail.cs", 
            "Mailable", 
            templateKey: "Email");
        
        ExportFile(
            Params.SkeletonDir / "src" / "Corpo.Skeleton" / "Features" / "Teams" / "_Created.mail.cshtml", 
            "MailTemplate", 
            templateKey: "Email");
    }
}