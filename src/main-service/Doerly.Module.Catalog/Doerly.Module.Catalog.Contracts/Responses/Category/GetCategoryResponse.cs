namespace Doerly.Module.Catalog.Contracts.Responses
{
    public class GetCategoryResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsEnabled { get; set; }
        public List<GetCategoryResponse> Children { get; set; } = new();
    }
}
