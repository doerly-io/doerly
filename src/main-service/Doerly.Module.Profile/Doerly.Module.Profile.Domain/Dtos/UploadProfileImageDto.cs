using Microsoft.AspNetCore.Http;

namespace Doerly.Module.Profile.Domain.Dtos;

public class UploadProfileImageDto
{
    public IFormFile Image { get; set; }
}