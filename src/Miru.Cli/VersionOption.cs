using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using System.Linq;

namespace Miru.Cli;

public class MiruVersionOption : Option<bool>
{
    private readonly CommandLineBuilder _builder;
    private string _description;

    public MiruVersionOption(CommandLineBuilder builder) : base("--version")
    {
        _builder = builder;
            
        // DisallowBinding = true;

        AddValidators();
    }

    public MiruVersionOption(string[] aliases, CommandLineBuilder builder) : base(aliases)
    {
        _builder = builder;

        // DisallowBinding = true;

        AddValidators();
    }

    private void AddValidators()
    {
        AddValidator(result =>
        {
            if (result.Parent is { } parent &&
                parent.Children.Where(r => r.Symbol is not MiruVersionOption)
                    .Any(IsNotImplicit))
            {
                result.ErrorMessage =  "{0} option cannot be combined with other arguments.";
            }
        });
    }

    private static bool IsNotImplicit(SymbolResult symbolResult)
    {
        return symbolResult switch
        {
            ArgumentResult argumentResult => !(argumentResult.Argument.HasDefaultValue && argumentResult.Tokens.Count == 0),
            OptionResult optionResult => !optionResult.IsImplicit,
            _ => true
        };
    }

    public override string Description
    {
        get => _description ??= "Show version information";
        set => _description = value;
    }

    public override bool Equals(object obj)
    {
        return obj is MiruVersionOption;
    }

    public override int GetHashCode()
    {
        return typeof(MiruVersionOption).GetHashCode();
    }
}