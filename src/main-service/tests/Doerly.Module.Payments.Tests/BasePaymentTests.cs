using Doerly.Module.Authorization.DataAccess;
using Doerly.Module.Payments.DataAccess;
using Microsoft.Extensions.Configuration;

namespace Doerly.Module.Authorization.Tests;

public class BasePaymentTests : IClassFixture<MsSqlTestContainerFixture>
{
    protected PaymentDbContext DbContext { get; private set; }

    public BasePaymentTests(MsSqlTestContainerFixture fixture)
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["ConnectionStrings:PaymentConnection"] = fixture.ConnectionString
            })
            .Build();

        DbContext = new PaymentDbContext(configuration);
        DbContext.Database.EnsureCreated();
    }
    
    
}
