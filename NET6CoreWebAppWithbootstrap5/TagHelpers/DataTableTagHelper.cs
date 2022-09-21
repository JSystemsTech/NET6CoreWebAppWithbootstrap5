using Microsoft.AspNetCore.Razor.TagHelpers;

namespace NET6CoreWebAppWithbootstrap5.TagHelpers
{
    public class DataTableTagHelper:TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "table";    // Replaces <email> with <a> tag
            output.Attributes.SetAttribute("class", "table table");
        }
    }
}
