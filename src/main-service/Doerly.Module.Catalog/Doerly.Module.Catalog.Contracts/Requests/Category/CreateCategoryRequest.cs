namespace Doerly.Module.Catalog.Contracts.Requests
{
    public class CreateCategoryRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int? ParentId { get; set; }
        public bool IsEnabled { get; set; }
    }
}
