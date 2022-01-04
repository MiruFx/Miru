using MediatR;

namespace Miru.Wizardable;

public abstract class WizardRequest<TResponse> : IRequest<TResponse>, IWizardRequest
{
    public bool Review { get; set; }
    
    public bool Filled { get; set; }
}