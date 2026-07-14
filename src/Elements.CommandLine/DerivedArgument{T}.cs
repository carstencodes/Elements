using System;
using System.CommandLine;

namespace HedgeCraft.Elements.CommandLine;

public class DerivedArgument<T, U>: Argument<U> where U: notnull, IParsable<U>
{
    private readonly ArgumentResultConverter<T, U> argumentResultConverter;

    public DerivedArgument(string name, Argument<T> argument, Func<T, U> converter, U defaultValue) : this(name, argument, converter, () => defaultValue)
    {
    }

    public DerivedArgument(string name, Argument<T> argument, Func<T, U> converter, Func<U> defaultValueFactory) : base(name)
    {
        this.argumentResultConverter = new(argument, converter, defaultValueFactory);
        
        base.CustomParser = this.argumentResultConverter.ParseFromArgument;
        base.DefaultValueFactory = this.argumentResultConverter.ParseFromArgumentOrDefault;
        base.Arity = ArgumentArity.ZeroOrOne;
    }

    public IFormatProvider FormatProvider
    {
        get
        {
            return this.argumentResultConverter.FormatProvider;
        }
        set
        {
            this.argumentResultConverter.FormatProvider = value;
        }
    }
}