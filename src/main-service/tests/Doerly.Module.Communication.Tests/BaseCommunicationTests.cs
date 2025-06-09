using Doerly.Domain.Models;
using Doerly.FileRepository;
using Doerly.Module.Communication.DataAccess;
using Doerly.Module.Communication.Tests.Fixtures;
using Doerly.Module.Profile.DataTransferObjects;
using Doerly.Proxy.Profile;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace Doerly.Module.Communication.Tests;

public class BaseCommunicationTests : IClassFixture<PostgresTestContainerFixture>, IAsyncLifetime
{
    protected CommunicationDbContext DbContext { get; }
    protected readonly Mock<IProfileModuleProxy> ProfileModuleMock;
    protected readonly Mock<IFileRepository> FileRepositoryMock;

    public BaseCommunicationTests(PostgresTestContainerFixture fixture)
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["ConnectionStrings:CommunicationConnection"] = fixture.ConnectionString
            })
            .Build();

        DbContext = new CommunicationDbContext(configuration);
        ProfileModuleMock = new Mock<IProfileModuleProxy>();
        FileRepositoryMock = new Mock<IFileRepository>();
    }
    
    protected async Task SetupUserProfiles(params int[] userIds)
    {
        foreach (var userId in userIds)
        {
            var profileDto = new ProfileDto
            {
                Id = userId,
                FirstName = "Test",
                LastName = "User",
                ImageUrl = null,
            };

            ProfileModuleMock
                .Setup(x => x.GetProfileAsync(userId))
                .ReturnsAsync(OperationResult.Success(profileDto));
        }
    }

    protected void SetupFileRepository(string filePath = "test/path/file.pdf")
    {
        FileRepositoryMock
            .Setup(x => x.UploadFileAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<byte[]>()))
            .Returns(Task.CompletedTask);

        FileRepositoryMock
            .Setup(x => x.GetSasUrlAsync(
                It.IsAny<string>(),
                It.IsAny<string>()))
            .ReturnsAsync(filePath);
    }

    public async Task InitializeAsync()
    {
        await DbContext.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        await DbContext.Database.EnsureDeletedAsync();
        DbContext.Dispose();
    }
} 