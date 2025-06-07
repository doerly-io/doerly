using Doerly.Domain.Factories;

namespace Doerly.Module.Catalog.Api.ModuleWrapper;

public interface ICatalogModuleWrapper
{
    
}

public class CatalogModuleWrapper : ICatalogModuleWrapper
{
    private readonly IHandlerFactory _handlerFactory;

    public CatalogModuleWrapper(IHandlerFactory handlerFactory)
    {
        _handlerFactory = handlerFactory;
    }

    
    
}