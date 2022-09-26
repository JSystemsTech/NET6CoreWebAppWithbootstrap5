using Microsoft.AspNetCore.Mvc.Rendering;
using NET6CoreWebAppWithbootstrap5.Extensions.Html;

namespace NET6CoreWebAppWithbootstrap5.Models.Home
{
    public class SampleDataTableModelVM
    {
        public string? DropdownValue { get; set; }

        public SelectList<(string text, string value)> SelectList => (new (string text, string value)[] { ("Value A", "A"), ("Value B", "B"), ("Value C", "C"), ("Value D", "D") }).ToSelectList(m=> m.text, m=> m.value);
    }
}
