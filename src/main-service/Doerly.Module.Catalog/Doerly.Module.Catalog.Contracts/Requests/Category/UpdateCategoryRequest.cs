namespace Doerly.Module.Catalog.Contracts.Requests
{
    public class UpdateCategoryRequest
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int? ParentId { get; set; }
        public bool IsEnabled { get; set; }
    }

}
