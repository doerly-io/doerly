using Doerly.Module.Profile.DataTransferObjects;

namespace Doerly.Module.Catalog.Contracts.Responses
{
    public class GetServiceResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public int UserId { get; set; }
        public ProfileDto? User { get; set; }
        public decimal? Price { get; set; }
        public List<string>? CategoryPath { get; set; } = new();
        public bool IsEnabled { get; set; }
        public bool IsDeleted { get; set; }

        public List<FilterValueResponse> FilterValues { get; set; } = new();
        public List<GetServiceResponse> RecommendedServices { get; set; } = new();
    }
}
