namespace Miru.Wizardable;

public interface IWizardRequest
{
    bool Review { get; set; }
    
    bool Filled { get; set; }
}