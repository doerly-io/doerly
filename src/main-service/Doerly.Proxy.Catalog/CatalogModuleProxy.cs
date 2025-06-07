using Doerly.Module.Catalog.Api.ModuleWrapper;

namespace Doerly.Proxy.Catalog;

public class CatalogModuleProxy : ICatalogModuleProxy
{
    private readonly ICatalogModuleWrapper _catalogModuleWrapper;

    public CatalogModuleProxy(ICatalogModuleWrapper catalogModuleWrapper)
    {
        _catalogModuleWrapper = catalogModuleWrapper;
    }

    
    
}