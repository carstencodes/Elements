// SPDX-Identifier: MIT
//
// (C) 2023-2026 Carsten Igel.
// Published under MIT License

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;

namespace HedgeCraft.Elements.SourceGenerators.ConfigurationPath;

// AI GENERATED START - model: Copilot
/// <summary>Discovers configuration object models and emits their local marker attribute and context shells.</summary>
[Generator]
public sealed class ConfigurationPathGenerator : IIncrementalGenerator
{
    /// <summary>Provides the fallback namespace when the consuming project does not define a root namespace.</summary>
    private const string DefaultNamespace = "HedgeCraft.Extensions.Configuration.UserSettings.Attributes";

    /// <summary>Identifies the framework attribute that overrides a property's configuration key.</summary>
    private const string KeyNameAttribute = "Microsoft.Extensions.Configuration.ConfigurationKeyNameAttribute";

    /// <summary>Formats type symbols with global qualification so emitted code is unambiguous.</summary>
    private static readonly SymbolDisplayFormat FullyQualifiedFormat = SymbolDisplayFormat.FullyQualifiedFormat;

    /// <summary>Registers incremental discovery of marked models and emits their attribute and contexts.</summary>
    /// <param name="context">The initialization context supplied by the compiler.</param>
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // Keeping namespace selection and marked-type discovery separate lets Roslyn cache each pipeline branch.
        IncrementalValueProvider<string> rootNamespace = context.AnalyzerConfigOptionsProvider
            .Select(static (provider, _) => GetRootNamespace(provider));

        IncrementalValuesProvider<INamedTypeSymbol> markedTypes = context.SyntaxProvider
            .CreateSyntaxProvider(IsMarkedTypeCandidate, GetMarkedType)
            .Where(static type => type is not null)
            .Select(static (type, _) => type!)
            .WithComparer(SymbolEqualityComparer.Default);

        context.RegisterSourceOutput(rootNamespace, ConfigurationObjectAttributeEmitter.Emit);
        context.RegisterSourceOutput(markedTypes.Collect(), EmitContexts);
    }

    /// <summary>Finds a recognized dictionary contract among a type and its implemented interfaces.</summary>
    /// <param name="type">The candidate collection type.</param>
    /// <returns>The generic dictionary contract, or <see langword="null"/> when none is implemented.</returns>
    internal static INamedTypeSymbol? GetDictionaryContract(INamedTypeSymbol type)
    {
        return type.AllInterfaces
            .Prepend(type)
            .FirstOrDefault(IsGenericDictionary);
    }

    /// <summary>Filters syntax nodes before semantic analysis to keep incremental discovery inexpensive.</summary>
    /// <param name="node">The syntax node to inspect.</param>
    /// <param name="cancellationToken">The token supplied by the compiler for cancellable analysis.</param>
    /// <returns><see langword="true"/> when the declaration uses the configuration-object marker.</returns>
    private static bool IsMarkedTypeCandidate(SyntaxNode node, CancellationToken cancellationToken)
    {
        if (node is not BaseTypeDeclarationSyntax declaration)
        {
            return false;
        }

        return declaration.AttributeLists
            .SelectMany(static list => list.Attributes)
            .Any(static attribute =>
                GetRightmostName(attribute.Name) is "ConfigurationObject" or "ConfigurationObjectAttribute");
    }

    /// <summary>Resolves a marker candidate to its declared type symbol.</summary>
    /// <param name="context">The semantic context associated with the candidate declaration.</param>
    /// <param name="cancellationToken">The token supplied by the compiler for cancellable analysis.</param>
    /// <returns>The marked named type, or <see langword="null"/> for unsupported declarations.</returns>
    private static INamedTypeSymbol? GetMarkedType(GeneratorSyntaxContext context, CancellationToken cancellationToken)
    {
        return context.Node is BaseTypeDeclarationSyntax declaration
            ? context.SemanticModel.GetDeclaredSymbol(declaration, cancellationToken) as INamedTypeSymbol
            : null;
    }

    /// <summary>Extracts the final identifier from qualified, aliased, or simple attribute names.</summary>
    /// <param name="name">The attribute's syntactic name.</param>
    /// <returns>The final identifier, or an empty string for an unsupported name form.</returns>
    private static string GetRightmostName(NameSyntax name)
    {
        return name switch
        {
            IdentifierNameSyntax identifier => identifier.Identifier.ValueText,
            QualifiedNameSyntax qualified => GetRightmostName(qualified.Right),
            AliasQualifiedNameSyntax aliasQualified => GetRightmostName(aliasQualified.Name),
            _ => string.Empty,
        };
    }

    /// <summary>Gets a valid root namespace or falls back to the generator's documented default namespace.</summary>
    /// <param name="provider">The analyzer configuration options for the consuming compilation.</param>
    /// <returns>A syntactically valid, escaped namespace name.</returns>
    private static string GetRootNamespace(AnalyzerConfigOptionsProvider provider)
    {
        if (!provider.GlobalOptions.TryGetValue("build_property.RootNamespace", out string? rootNamespace) ||
            string.IsNullOrWhiteSpace(rootNamespace))
        {
            return DefaultNamespace;
        }

        string[] segments = rootNamespace.Split('.');
        return segments.All(IsNamespaceIdentifier)
            ? string.Join(".", segments.Select(EscapeIdentifier))
            : DefaultNamespace;

        // Keywords are accepted here because EscapeIdentifier makes them legal in the emitted namespace.
        static bool IsNamespaceIdentifier(string segment)
        {
            return SyntaxFacts.IsValidIdentifier(segment) ||
                SyntaxFacts.GetKeywordKind(segment) != SyntaxKind.None ||
                SyntaxFacts.GetContextualKeywordKind(segment) != SyntaxKind.None;
        }
    }

    /// <summary>Discovers reachable model types and emits one context source for each model requiring one.</summary>
    /// <param name="context">The source-production context used to register generated files.</param>
    /// <param name="markedTypes">The types directly marked by the consumer.</param>
    private static void EmitContexts(SourceProductionContext context, ImmutableArray<INamedTypeSymbol> markedTypes)
    {
        HashSet<INamedTypeSymbol> contextTypes = new(SymbolEqualityComparer.Default);
        HashSet<INamedTypeSymbol> activeTypes = new(SymbolEqualityComparer.Default);

        foreach (INamedTypeSymbol markedType in markedTypes)
        {
            VisitType(markedType, contextTypes, activeTypes);
        }

        foreach (INamedTypeSymbol type in contextTypes.OrderBy(
                     static type => type.ToDisplayString(FullyQualifiedFormat),
                     StringComparer.Ordinal))
        {
            if (FindExistingContext(type) is not null)
            {
                continue;
            }

            string source = ConfigurationContextEmitter.CreateSource(type);
            context.AddSource(
                ConfigurationContextEmitter.CreateHintName(type),
                SourceText.From(source, Encoding.UTF8));
        }
    }

    /// <summary>Adds non-scalar model types reachable through properties, arrays, and collection values.</summary>
    /// <param name="type">The current property or element type.</param>
    /// <param name="contextTypes">The set of model definitions that require generated contexts.</param>
    /// <param name="activeTypes">The recursion stack used to stop cycles in model graphs.</param>
    private static void VisitType(
        ITypeSymbol type,
        ISet<INamedTypeSymbol> contextTypes,
        ISet<INamedTypeSymbol> activeTypes)
    {
        if (type is IArrayTypeSymbol arrayType)
        {
            VisitArrayType(arrayType, contextTypes, activeTypes);
            return;
        }

        if (type is INamedTypeSymbol namedType)
        {
            VisitNamedType(namedType, contextTypes, activeTypes);
        }
    }

    /// <summary>Visits array elements unless the array is a scalar byte buffer.</summary>
    /// <param name="arrayType">The array type to inspect.</param>
    /// <param name="contextTypes">The set of model definitions that require generated contexts.</param>
    /// <param name="activeTypes">The recursion stack used to stop cycles in model graphs.</param>
    private static void VisitArrayType(
        IArrayTypeSymbol arrayType,
        ISet<INamedTypeSymbol> contextTypes,
        ISet<INamedTypeSymbol> activeTypes)
    {
        if (!IsByteArray(arrayType))
        {
            VisitType(arrayType.ElementType, contextTypes, activeTypes);
        }
    }

    /// <summary>Routes nullable, scalar, collection, and model types to their appropriate traversal.</summary>
    /// <param name="namedType">The named type to classify.</param>
    /// <param name="contextTypes">The set of model definitions that require generated contexts.</param>
    /// <param name="activeTypes">The recursion stack used to stop cycles in model graphs.</param>
    private static void VisitNamedType(
        INamedTypeSymbol namedType,
        ISet<INamedTypeSymbol> contextTypes,
        ISet<INamedTypeSymbol> activeTypes)
    {
        if (IsNullableValueType(namedType))
        {
            VisitType(namedType.TypeArguments[0], contextTypes, activeTypes);
            return;
        }

        if (IsSimpleType(namedType))
        {
            return;
        }

        if (TryGetCollectionValues(namedType, out ImmutableArray<ITypeSymbol> collectionValues))
        {
            VisitCollectionValues(collectionValues, contextTypes, activeTypes);
            return;
        }

        VisitModelType(namedType, contextTypes, activeTypes);
    }

    /// <summary>Visits the value or element types exposed by a supported collection.</summary>
    /// <param name="collectionValues">The collection value and element types to visit.</param>
    /// <param name="contextTypes">The set of model definitions that require generated contexts.</param>
    /// <param name="activeTypes">The recursion stack used to stop cycles in model graphs.</param>
    private static void VisitCollectionValues(
        ImmutableArray<ITypeSymbol> collectionValues,
        ISet<INamedTypeSymbol> contextTypes,
        ISet<INamedTypeSymbol> activeTypes)
    {
        foreach (ITypeSymbol valueType in collectionValues)
        {
            VisitType(valueType, contextTypes, activeTypes);
        }
    }

    /// <summary>Adds a model definition and traverses its bindable properties once per active path.</summary>
    /// <param name="namedType">The model type whose properties should be visited.</param>
    /// <param name="contextTypes">The set of model definitions that require generated contexts.</param>
    /// <param name="activeTypes">The recursion stack used to stop cycles in model graphs.</param>
    private static void VisitModelType(
        INamedTypeSymbol namedType,
        ISet<INamedTypeSymbol> contextTypes,
        ISet<INamedTypeSymbol> activeTypes)
    {
        INamedTypeSymbol definition = namedType.OriginalDefinition;
        contextTypes.Add(definition);
        // A repeated model definition is still emitted once, but its properties need no second graph traversal.
        if (!activeTypes.Add(definition))
        {
            return;
        }

        try
        {
            foreach (IPropertySymbol property in GetBindableProperties(namedType))
            {
                VisitType(property.Type, contextTypes, activeTypes);
            }
        }
        finally
        {
            activeTypes.Remove(definition);
        }
    }

    /// <summary>Returns element types from dictionary values or distinct generic enumerable contracts.</summary>
    /// <param name="type">The collection type being examined.</param>
    /// <param name="collectionValues">The discovered value or element types.</param>
    /// <returns><see langword="true"/> when the type exposes one or more recognized value types.</returns>
    internal static bool TryGetCollectionValues(
        INamedTypeSymbol type,
        out ImmutableArray<ITypeSymbol> collectionValues)
    {
        INamedTypeSymbol? dictionary = GetDictionaryContract(type);
        if (dictionary is not null)
        {
            // Configuration paths name dictionary entries by key, so key types do not need generated contexts.
            collectionValues = [dictionary.TypeArguments[1]];
            return true;
        }

        IEnumerable<INamedTypeSymbol> contracts = type.AllInterfaces.Prepend(type);
        ImmutableArray<ITypeSymbol>.Builder values = ImmutableArray.CreateBuilder<ITypeSymbol>();
        HashSet<ITypeSymbol> seen = new(SymbolEqualityComparer.Default);
        foreach (INamedTypeSymbol contract in contracts
                     .Where(IsGenericEnumerable)
                     .Where(contract => seen.Add(contract.TypeArguments[0])))
        {
            values.Add(contract.TypeArguments[0]);
        }

        collectionValues = values.ToImmutable();
        return values.Count > 0;
    }

    /// <summary>Tests whether a named type is one of the supported generic dictionary definitions.</summary>
    /// <param name="type">The contract to inspect.</param>
    /// <returns><see langword="true"/> for a recognized two-argument generic dictionary contract.</returns>
    private static bool IsGenericDictionary(INamedTypeSymbol type)
    {
        return type.Arity == 2 &&
            string.Equals(
                type.ContainingNamespace.ToDisplayString(),
                "System.Collections.Generic",
                StringComparison.Ordinal) &&
            type.MetadataName is "IDictionary`2" or "IReadOnlyDictionary`2" or "Dictionary`2";
    }

    /// <summary>Tests whether a named type is the framework's generic enumerable contract.</summary>
    /// <param name="type">The contract to inspect.</param>
    /// <returns><see langword="true"/> for <see cref="System.Collections.Generic.IEnumerable{T}"/>.</returns>
    private static bool IsGenericEnumerable(INamedTypeSymbol type)
    {
        return type.Arity == 1 &&
            string.Equals(
                type.ContainingNamespace.ToDisplayString(),
                "System.Collections.Generic",
                StringComparison.Ordinal) &&
            string.Equals(type.MetadataName, "IEnumerable`1", StringComparison.Ordinal);
    }

    /// <summary>Checks for the CLR nullable-value-type wrapper rather than nullable-reference annotations.</summary>
    /// <param name="type">The named type to inspect.</param>
    /// <returns><see langword="true"/> when the type is <see cref="Nullable{T}"/>.</returns>
    private static bool IsNullableValueType(INamedTypeSymbol type)
    {
        return type.OriginalDefinition.SpecialType == SpecialType.System_Nullable_T;
    }

    /// <summary>
    /// Identifies byte arrays, which configuration treats as scalar binary values rather than sequences.
    /// </summary>
    /// <param name="type">The array type to inspect.</param>
    /// <returns><see langword="true"/> only for a one-dimensional array of bytes.</returns>
    internal static bool IsByteArray(IArrayTypeSymbol type)
    {
        return type.Rank == 1 && type.ElementType.SpecialType == SpecialType.System_Byte;
    }

    /// <summary>Recognizes scalar types that are emitted as one configuration value instead of traversed.</summary>
    /// <param name="type">The named type to classify.</param>
    /// <returns><see langword="true"/> for primitives, enums, and supported framework scalar types.</returns>
    internal static bool IsSimpleType(INamedTypeSymbol type)
    {
        if (type.TypeKind == TypeKind.Enum || type.SpecialType != SpecialType.None)
        {
            return true;
        }

        string name = type.ToDisplayString(FullyQualifiedFormat);
        return name is
            "global::System.DateTime" or
            "global::System.DateTimeOffset" or
            "global::System.TimeSpan" or
            "global::System.Guid" or
            "global::System.Uri";
    }

    /// <summary>Gets public instance properties with public getters, preferring the most-derived declaration.</summary>
    /// <param name="type">The model whose bindable properties are requested.</param>
    /// <returns>Properties sorted by name for deterministic generated output.</returns>
    internal static IEnumerable<IPropertySymbol> GetBindableProperties(INamedTypeSymbol type)
    {
        HashSet<string> names = new(StringComparer.Ordinal);
        List<IPropertySymbol> properties = [];

        for (INamedTypeSymbol? currentType = type; currentType is not null; currentType = currentType.BaseType)
        {
            AddProperties(currentType.GetMembers().OfType<IPropertySymbol>());
        }

        if (type.TypeKind == TypeKind.Interface)
        {
            foreach (INamedTypeSymbol interfaceType in type.AllInterfaces)
            {
                AddProperties(interfaceType.GetMembers().OfType<IPropertySymbol>());
            }
        }

        return properties.OrderBy(static property => property.Name, StringComparer.Ordinal);

        // Visit derived declarations first so a hidden base property cannot replace the effective member.
        void AddProperties(IEnumerable<IPropertySymbol> candidates)
        {
            foreach (IPropertySymbol property in candidates.Where(IsBindableProperty))
            {
                if (!names.Add(property.Name))
                {
                    continue;
                }

                properties.Add(property);
            }
        }

        // The getter must be public because generated dump methods read the property directly.
        static bool IsBindableProperty(IPropertySymbol property)
        {
            return !property.IsStatic &&
                !property.IsIndexer &&
                property.GetMethod is not null &&
                property.GetMethod.DeclaredAccessibility == Accessibility.Public;
        }
    }

    /// <summary>Gets a property's configured key name, falling back to its source identifier.</summary>
    /// <param name="property">The property whose configuration key is requested.</param>
    /// <returns>The key name declared by <c>ConfigurationKeyNameAttribute</c>, or the property name.</returns>
    internal static string GetKeyName(IPropertySymbol property)
    {
        AttributeData? keyNameAttribute = property.GetAttributes()
            .FirstOrDefault(static attribute =>
                string.Equals(
                    attribute.AttributeClass?.ToDisplayString(),
                    KeyNameAttribute,
                    StringComparison.Ordinal));

        return keyNameAttribute?.ConstructorArguments.FirstOrDefault().Value as string ?? property.Name;
    }

    /// <summary>Collects generic parameters from the outermost containing type through the supplied type.</summary>
    /// <param name="type">The nested or top-level type being emitted.</param>
    /// <returns>Type parameters in declaration order, suitable for a generated sibling type.</returns>
    internal static ImmutableArray<ITypeParameterSymbol> GetTypeParameters(INamedTypeSymbol type)
    {
        Stack<INamedTypeSymbol> containingTypes = new();
        for (INamedTypeSymbol? current = type; current is not null; current = current.ContainingType)
        {
            containingTypes.Push(current);
        }

        return containingTypes.SelectMany(static current => current.TypeParameters).ToImmutableArray();
    }

    /// <summary>Collects constructed generic arguments from containing types through the supplied type.</summary>
    /// <param name="type">The nested or top-level constructed type being referenced.</param>
    /// <returns>Type arguments in the same order as <see cref="GetTypeParameters"/>.</returns>
    internal static ImmutableArray<ITypeSymbol> GetTypeArguments(INamedTypeSymbol type)
    {
        Stack<INamedTypeSymbol> containingTypes = new();
        for (INamedTypeSymbol? current = type; current is not null; current = current.ContainingType)
        {
            containingTypes.Push(current);
        }

        return containingTypes.SelectMany(static current => current.TypeArguments).ToImmutableArray();
    }

    /// <summary>Creates a stable sibling context name, including generic parameter names when needed.</summary>
    /// <param name="type">The model definition represented by the context.</param>
    /// <param name="typeParameters">The containing and local generic parameters of the model.</param>
    /// <returns>The generated context class identifier.</returns>
    internal static string CreateContextName(
        INamedTypeSymbol type,
        ImmutableArray<ITypeParameterSymbol> typeParameters)
    {
        string suffix = typeParameters.Length == 0
            ? type.Name
            : string.Concat(
                type.Name,
                "_",
                string.Join("_", typeParameters.Select(static parameter => parameter.Name)));
        return string.Concat("ConfigurationContextOf", suffix);
    }

    /// <summary>Formats a generic parameter's constraints for a generated context declaration.</summary>
    /// <param name="parameter">The parameter whose constraints must be preserved.</param>
    /// <returns>A complete <c>where</c> clause, or an empty string if the parameter is unconstrained.</returns>
    internal static string CreateConstraintClause(ITypeParameterSymbol parameter)
    {
        List<string> constraints = [];
        if (parameter.HasUnmanagedTypeConstraint)
        {
            constraints.Add("unmanaged");
        }
        else if (parameter.HasValueTypeConstraint)
        {
            constraints.Add("struct");
        }
        else if (parameter.HasReferenceTypeConstraint)
        {
            constraints.Add(parameter.ReferenceTypeConstraintNullableAnnotation == NullableAnnotation.Annotated
                ? "class?"
                : "class");
        }
        else if (parameter.HasNotNullConstraint)
        {
            constraints.Add("notnull");
        }

        constraints.AddRange(
            parameter.ConstraintTypes.Select(static type => type.ToDisplayString(FullyQualifiedFormat)));
        if (parameter.HasConstructorConstraint &&
            !parameter.HasUnmanagedTypeConstraint &&
            !parameter.HasValueTypeConstraint)
        {
            constraints.Add("new()");
        }

        return constraints.Count == 0
            ? string.Empty
            : string.Concat("    where ", EscapeIdentifier(parameter.Name), " : ", string.Join(", ", constraints));
    }

    /// <summary>Escapes a keyword identifier so it remains legal in generated C# source.</summary>
    /// <param name="identifier">The unescaped identifier.</param>
    /// <returns>The identifier prefixed with <c>@</c> when required.</returns>
    internal static string EscapeIdentifier(string identifier)
    {
        return SyntaxFacts.GetKeywordKind(identifier) != SyntaxKind.None ||
            SyntaxFacts.GetContextualKeywordKind(identifier) != SyntaxKind.None
            ? string.Concat("@", identifier)
            : identifier;
    }

    /// <summary>Finds a consumer-defined context that already derives from the supported context base.</summary>
    /// <param name="modelType">The model definition for which to search.</param>
    /// <returns>The unique matching context, or <see langword="null"/> when none or multiple are suitable.</returns>
    internal static INamedTypeSymbol? FindExistingContext(INamedTypeSymbol modelType)
    {
        ImmutableArray<ITypeParameterSymbol> typeParameters = GetTypeParameters(modelType);
        string contextName = CreateContextName(modelType, typeParameters);
        INamedTypeSymbol[] matches = modelType.ContainingNamespace.GetTypeMembers()
            .Where(contextType => IsContextCandidate(contextType, typeParameters.Length))
            .Where(contextType => ImplementsModel(contextType, modelType.OriginalDefinition))
            .ToArray();

        // Prefer the generator's predictable name when several valid contexts implement this model.
        INamedTypeSymbol? namedMatch = matches.FirstOrDefault(contextType =>
            string.Equals(contextType.Name, contextName, StringComparison.Ordinal));
        if (namedMatch is not null)
        {
            return namedMatch;
        }

        // A differently named context is reusable only when it is the sole valid match.
        return matches.Length == 1 ? matches[0] : null;
    }

    /// <summary>Filters context declarations by shape before checking their inheritance chain.</summary>
    /// <param name="contextType">The type declaration being considered for reuse.</param>
    /// <param name="arity">The number of generic parameters required by the model context.</param>
    /// <returns><see langword="true"/> when the declaration can represent a generated context.</returns>
    private static bool IsContextCandidate(INamedTypeSymbol contextType, int arity)
    {
        return contextType.TypeKind == TypeKind.Class &&
            !contextType.IsAbstract &&
            contextType.TypeParameters.Length == arity;
    }

    /// <summary>Checks whether a context derives from the supported base instantiated for the model.</summary>
    /// <param name="contextType">The candidate context declaration.</param>
    /// <param name="modelDefinition">The original model definition to match.</param>
    /// <returns><see langword="true"/> when a base type links the candidate to the model.</returns>
    private static bool ImplementsModel(INamedTypeSymbol contextType, INamedTypeSymbol modelDefinition)
    {
        for (INamedTypeSymbol? baseType = contextType.BaseType;
             baseType is not null;
             baseType = baseType.BaseType)
        {
            if (IsContextBase(baseType) &&
                SymbolEqualityComparer.Default.Equals(
                    baseType.TypeArguments[0].OriginalDefinition,
                    modelDefinition))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Identifies the shared context base by metadata identity, independent of aliases and using directives.
    /// </summary>
    /// <param name="type">The base type to identify.</param>
    /// <returns><see langword="true"/> when the type is the supported configuration context base.</returns>
    private static bool IsContextBase(INamedTypeSymbol type)
    {
        return string.Equals(type.MetadataName, "ConfigurationObjectContextBase`1", StringComparison.Ordinal) &&
            string.Equals(
                type.ContainingNamespace.ToDisplayString(),
                "HedgeCraft.Elements.Extensions.Configuration.UserSettings.Contexts",
                StringComparison.Ordinal);
    }
}
// AI GENERATED END
