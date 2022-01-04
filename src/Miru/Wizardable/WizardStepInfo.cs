using System;

namespace Miru.Wizardable;

public class WizardStepInfo
{
    public Func<IWizardRequest> GetCommand { get; set; }
    public string Name { get; set; }
    public Type Type { get; set; }
}