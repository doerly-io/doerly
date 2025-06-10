using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Doerly.Module.Profile.DataTransferObjects;
using Doerly.Module.Profile.Enums;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Profile.Tests.Integration;

public class ProfileIntegrationTests : BaseProfileTests, IAsyncLifetime
{
    private readonly ProfileTestServer _testServer;
    private readonly HttpClient _client;
    private readonly ITestOutputHelper _output;

    public ProfileIntegrationTests(
        PostgresTestContainerFixture fixture,
        ITestOutputHelper output)
        : base(fixture)
    {
        _output = output;
        _testServer = new ProfileTestServer(fixture.ConnectionString);
        _client = _testServer.Client;
        
        // Setup authentication if needed
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "test-token");
    }

    public async ValueTask InitializeAsync()
    {
        // Ensure database is created and clean
        await DbContext.Database.EnsureCreatedAsync();
        
        // Clear any existing data
        if (await DbContext.Profiles.AnyAsync())
        {
            var profiles = await DbContext.Profiles.ToListAsync();
            DbContext.Profiles.RemoveRange(profiles);
            await DbContext.SaveChangesAsync();
        }
    }

    public async ValueTask DisposeAsync()
    {
        _testServer?.Dispose();
    }

    [Fact]
    public async Task CreateProfile_WithValidData_ShouldReturnCreated()
    {
        // Arrange
        var profileDto = new ProfileSaveDto
        {
            UserId = 1,
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateOnly(1990, 1, 1),
            Sex = ESex.Male,
            Bio = "Test bio",
            CityId = 1
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/profile", profileDto);

        // Assert
        _output.WriteLine($"Response Status: {response.StatusCode}");
        var responseContent = await response.Content.ReadAsStringAsync();
        _output.WriteLine($"Response Content: {responseContent}");
        
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        
        // Verify the profile was created in the database
        var createdProfile = await DbContext.Profiles
            .FirstOrDefaultAsync(p => p.UserId == profileDto.UserId);
        
        Assert.NotNull(createdProfile);
        Assert.Equal(profileDto.FirstName, createdProfile.FirstName);
        Assert.Equal(profileDto.LastName, createdProfile.LastName);
        Assert.Equal(profileDto.DateOfBirth, createdProfile.DateOfBirth);
        Assert.Equal(profileDto.Sex, createdProfile.Sex);
        Assert.Equal(profileDto.Bio, createdProfile.Bio);
        Assert.Equal(profileDto.CityId, createdProfile.CityId);
    }

    [Fact]
    public async Task CreateProfile_WithDuplicateUserId_ShouldReturnConflict()
    {
        // Arrange
        var existingProfileDto = new ProfileSaveDto
        {
            UserId = 2,
            FirstName = "Jane",
            LastName = "Doe",
            Sex = ESex.Female
        };

        // Create the first profile
        var firstResponse = await _client.PostAsJsonAsync("/api/profile", existingProfileDto);
        Assert.Equal(HttpStatusCode.Created, firstResponse.StatusCode);

        var duplicateProfileDto = new ProfileSaveDto
        {
            UserId = 2, // Same UserId
            FirstName = "John",
            LastName = "Smith",
            Sex = ESex.Male
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/profile", duplicateProfileDto);

        // Assert
        _output.WriteLine($"Response Status: {response.StatusCode}");
        var responseContent = await response.Content.ReadAsStringAsync();
        _output.WriteLine($"Conflict Response: {responseContent}");
        
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    [Theory]
    [InlineData("", "LastName")] // Empty first name
    [InlineData("FirstName", "")] // Empty last name
    public async Task CreateProfile_WithInvalidData_ShouldReturnBadRequest(
        string firstName, 
        string lastName)
    {
        // Arrange
        var profileDto = new ProfileSaveDto
        {
            UserId = Random.Shared.Next(1000, 9999),
            FirstName = firstName,
            LastName = lastName,
            Sex = ESex.Male
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/profile", profileDto);

        // Assert
        _output.WriteLine($"Response Status: {response.StatusCode}");
        var responseContent = await response.Content.ReadAsStringAsync();
        _output.WriteLine($"Validation Error Response: {responseContent}");
        
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task UpdateProfile_WithValidData_ShouldReturnOk()
    {
        // Arrange - First create a profile
        var originalProfileDto = new ProfileSaveDto
        {
            UserId = 3,
            FirstName = "Original",
            LastName = "User",
            Sex = ESex.Male,
            Bio = "Original bio"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/profile", originalProfileDto);
        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        var updatedProfileDto = new ProfileSaveDto
        {
            UserId = 3,
            FirstName = "Updated",
            LastName = "User",
            DateOfBirth = new DateOnly(1985, 5, 15),
            Sex = ESex.Male,
            Bio = "Updated bio with more details",
            CityId = 2
        };

        // Act
        var response = await _client.PutAsJsonAsync("/api/profile", updatedProfileDto);

        // Assert
        _output.WriteLine($"Response Status: {response.StatusCode}");
        var responseContent = await response.Content.ReadAsStringAsync();
        _output.WriteLine($"Update Response: {responseContent}");
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        // Verify the profile was updated in the database
        var updatedProfile = await DbContext.Profiles
            .FirstOrDefaultAsync(p => p.UserId == updatedProfileDto.UserId);
        
        Assert.NotNull(updatedProfile);
        Assert.Equal(updatedProfileDto.FirstName, updatedProfile.FirstName);
        Assert.Equal(updatedProfileDto.LastName, updatedProfile.LastName);
        Assert.Equal(updatedProfileDto.DateOfBirth, updatedProfile.DateOfBirth);
        Assert.Equal(updatedProfileDto.Bio, updatedProfile.Bio);
        Assert.Equal(updatedProfileDto.CityId, updatedProfile.CityId);
    }

    [Fact]
    public async Task UpdateProfile_WithNonExistentUserId_ShouldReturnNotFound()
    {
        // Arrange
        var nonExistentProfileDto = new ProfileSaveDto
        {
            UserId = 99999, // Non-existent user ID
            FirstName = "Non",
            LastName = "Existent",
            Sex = ESex.Male
        };

        // Act
        var response = await _client.PutAsJsonAsync("/api/profile", nonExistentProfileDto);

        // Assert
        _output.WriteLine($"Response Status: {response.StatusCode}");
        var responseContent = await response.Content.ReadAsStringAsync();
        _output.WriteLine($"Not Found Response: {responseContent}");
        
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CreateAndUpdateProfile_FullWorkflow_ShouldWorkCorrectly()
    {
        // Arrange
        var userId = Random.Shared.Next(10000, 99999);
        var createDto = new ProfileSaveDto
        {
            UserId = userId,
            FirstName = "WorkFlow",
            LastName = "Test",
            Sex = ESex.Female
        };

        // Act 1 - Create Profile
        var createResponse = await _client.PostAsJsonAsync("/api/profile", createDto);
        
        // Assert 1
        _output.WriteLine($"Create Response Status: {createResponse.StatusCode}");
        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        // Act 2 - Update Profile
        var updateDto = new ProfileSaveDto
        {
            UserId = userId,
            FirstName = "Updated WorkFlow",
            LastName = "Updated Test",
            DateOfBirth = new DateOnly(1992, 12, 25),
            Sex = ESex.Female,
            Bio = "Updated during workflow test",
            CityId = 3
        };

        var updateResponse = await _client.PutAsJsonAsync("/api/profile", updateDto);
        
        // Assert 2
        _output.WriteLine($"Update Response Status: {updateResponse.StatusCode}");
        Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);
        
        // Verify final state
        var finalProfile = await DbContext.Profiles
            .FirstOrDefaultAsync(p => p.UserId == userId);
        
        Assert.NotNull(finalProfile);
        Assert.Equal(updateDto.FirstName, finalProfile.FirstName);
        Assert.Equal(updateDto.LastName, finalProfile.LastName);
        Assert.Equal(updateDto.DateOfBirth, finalProfile.DateOfBirth);
        Assert.Equal(updateDto.Bio, finalProfile.Bio);
        Assert.Equal(updateDto.CityId, finalProfile.CityId);
    }
}
