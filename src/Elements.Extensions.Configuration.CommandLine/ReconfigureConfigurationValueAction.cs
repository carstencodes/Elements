// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Threading;
using System.Threading.Tasks;

namespace HedgeCraft.Elements.Extensions.Configuration.CommandLine;

/// <summary>
/// Wraps a command-line action to update the configuration provider with the parsed option value upon invocation.
/// </summary>
/// <param name="configurationProvider">The configuration provider to update.</param>
/// <param name="binding">The configuration key and option holder pair.</param>
/// <param name="innerAction">The inner action to invoke, if any.</param>
internal sealed class ReconfigureConfigurationValueAction(CommandLineConfigurationProvider configurationProvider, KeyValuePair<string, OptionHolderBase> binding, CommandLineAction? innerAction) : AsynchronousCommandLineAction
{
    /// <summary>
    /// Invokes the command-line action asynchronously, setting the parsed option value into the configuration provider.
    /// </summary>
    /// <param name="parseResult">The command-line parse result.</param>
    /// <param name="cancellationToken">A cancellation token to observe.</param>
    /// <returns>A task returning the exit code from the action invocation.</returns>
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
