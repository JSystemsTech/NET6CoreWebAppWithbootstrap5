using DbFacade.DataLayer.Models;

namespace ApplicationInfoServiceAPI.DomainLayer.Models.Data
{
    public class ApplicationInfo : DbDataModel
    {
        public string AppId { get; set; } = "";
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string Type { get; set; } = "";
        public string DefaultRedirectUrl { get; set; } = "";

        protected override void Init()
        {
            AppId = GetColumn("AppId", "");
            Title = GetColumn("Title", "");
            Description = GetColumn("Description", "");
            Type = GetColumn("Type", "");
            DefaultRedirectUrl = GetColumn("DefaultRedirectUrl", "");
        }
        protected override async Task InitAsync()
        {
            AppId = await GetColumnAsync("AppId", "");
            Title = await GetColumnAsync("Title", "");
            Description = await GetColumnAsync("Description", "");
            Type = await GetColumnAsync("Type", "");
            DefaultRedirectUrl = await GetColumnAsync("DefaultRedirectUrl", "");
        }
    }
}
