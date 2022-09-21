namespace NET6CoreWebAppWithbootstrap5.Models
{
    public class OffcanvasSize
    {
        private static string _prefx = "offcanvas";
        public static string Sm = $"{_prefx}-sm";
        public static string Default = "";
        public static string Lg = $"{_prefx}-lg";
        public static string Xl = $"{_prefx}-xl";
        public static string Xxl = $"{_prefx}-xxl";

        public static string Map(string value)
            => string.IsNullOrWhiteSpace(value) ? Default :
            value.ToLower().Trim() == "sm" ? Sm :
            value.ToLower().Trim() == "lg" ? Lg :
            value.ToLower().Trim() == "xl" ? Xl :
            value.ToLower().Trim() == "xxl" ? Xxl : value;
    }
    public class OffcanvasPlacement
    {
        private static string _prefx = "offcanvas";
        public static string Start = $"{_prefx}-start";
        public static string Default = "";
        public static string End = $"{_prefx}-end";
        public static string Top = $"{_prefx}-top";
        public static string Bottom = $"{_prefx}-bottom";

        public static string Map(string value)
            => string.IsNullOrWhiteSpace(value) ? Default :
            value.ToLower().Trim() == "start" ? Start :
            value.ToLower().Trim() == "end" ? End :
            value.ToLower().Trim() == "top" ? Top :
            value.ToLower().Trim() == "bottom" ? Bottom : value;
    }
    public class Offcanvas
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string Size { get; set; }
        public string Placement { get; set; }
        // public string Fullscreen { get; set; }
        public bool Backdrop { get; set; }
        public bool Static { get; set; }
        public bool Keyboard { get; set; }
        public bool Focus { get; set; }
        public bool Scroll { get; set; }
        public bool Animation { get; set; }
        public string AnimationClass { get; set; }
        public string Loading { get; set; }
        public bool Close { get; set; }

        public Offcanvas()
        {
            Size = OffcanvasSize.Default;
            Placement = OffcanvasPlacement.Default;
            //Fullscreen = ModalFullscreen.Default;
            Backdrop = true;
            Keyboard = true;
            Focus = true;
            Animation = true;
            AnimationClass = "fade";
            Loading = "Loading Content. Please Wait";
            Close = true;
            Scroll = false;

        }
    }
}
