namespace HedgeCraft.Elements.Extensions.DependencyInjection.KeyedServices;

public interface IServiceKeyCollection<in TService>: IReadOnlyCollection<object> where TService: notnull
{
    Type AffectedServiceType { get; }

    bool HasType<TConcrete>() where TConcrete : TService;
}