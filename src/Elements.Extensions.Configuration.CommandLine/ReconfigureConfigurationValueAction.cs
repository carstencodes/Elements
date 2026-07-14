using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Threading;
using System.Threading.Tasks;

namespace HedgeCraft.Elements.Extensions.Configuration.CommandLine;

internal sealed class ReconfigureConfigurationValueAction(CommandLineConfigurationProvider configurationProvider, KeyValuePair<string, OptionHolderBase> binding, CommandLineAction? innerAction) : AsynchronousCommandLineAction
{
    public override async Task<int> InvokeAsync(ParseResult parseResult, CancellationToken cancellationToken = default)
    {
        int innerResult = innerAction switch
        {
            SynchronousCommandLineAction synchronousAction => synchronousAction.Invoke(parseResult),
            AsynchronousCommandLineAction asynchronousAction => await asynchronousAction.InvokeAsync(parseResult, cancellationToken).ConfigureAwait(false),
            _ => 0
        };

        string key = binding.Key;
        OptionHolderBase option = binding.Value;

        string? optionResult = option.GetResult(parseResult);

        configurationProvider.Set(key, optionResult);

        return innerResult;
    }
}