using Doerly.Domain.Factories;
using Doerly.Proxy.Catalog;

namespace Doerly.Module.Catalog.Api.ModuleWrapper;

public class CatalogModuleWrapper : ICatalogModuleWrapper
{
    private readonly IHandlerFactory _handlerFactory;

    public CatalogModuleWrapper(IHandlerFactory handlerFactory)
    {
        _handlerFactory = handlerFactory;
    }

    
    
}
