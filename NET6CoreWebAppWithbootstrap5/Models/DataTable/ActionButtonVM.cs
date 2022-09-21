using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NET6CoreWebAppWithbootstrap5.Models.DataTable
{
    public class ActionButtonVM
    {
        public string? ButtonClass { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Icon { get; set; }
        public string? Action { get; set; }
        public string? Href { get; set; }
        public bool IconRight { get; set; }

        public object? HtmlAttributes { get; set; }
    }
}