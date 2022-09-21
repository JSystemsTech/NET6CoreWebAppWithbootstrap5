using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NET6CoreWebAppWithbootstrap5.Models
{
    public class ModalSize
    {
        public static string Sm = "modal-sm";
        public static string Default = "";
        public static string Lg = "modal-lg";
        public static string Xl = "modal-xl";

        public static string Map(string value)
            => string.IsNullOrWhiteSpace(value) ? Default :
            value.ToLower().Trim() == "sm" ? Sm :
            value.ToLower().Trim() == "lg" ? Lg :
            value.ToLower().Trim() == "xl" ? Xl : value;
    }
    public class ModalFullscreen
    {
        public static string Default = "";
        public static string Always = "modal-fullscreen";
        public static string SmDown = "modal-fullscreen-sm-down";
        public static string MdDown = "modal-fullscreen-md-down";
        public static string LgDown = "modal-fullscreen-lg-down";
        public static string XlDown = "modal-fullscreen-xl-down";
        public static string XxlDown = "modal-fullscreen-xxl-down";
        public static string Map(string value)
            => string.IsNullOrWhiteSpace(value) ? Default :
            value.ToLower().Trim() == "always" ? Always :
            value.ToLower().Trim() == "smdown" ? SmDown :
            value.ToLower().Trim() == "mddown" ? MdDown :
            value.ToLower().Trim() == "lgdown" ? LgDown :
            value.ToLower().Trim() == "xldown" ? XlDown :
            value.ToLower().Trim() == "xxldown" ? XxlDown : value;

    }
    public class Modal
    {

        public string? Title { get; set; }
        public string? Description { get; set; }
        public string Size { get; set; }
        public string Fullscreen { get; set; }
        public bool Backdrop { get; set; }
        public bool Static { get; set; }
        public bool Keyboard { get; set; }
        public bool Focus { get; set; }
        public bool Footer { get; set; }
        public bool Centered { get; set; }
        public bool Scrollable { get; set; }
        public bool Animation { get; set; }
        public string AnimationClass { get; set; }
        public string Loading { get; set; }
        public bool Close { get; set; }

        public Modal()
        {
            Size = ModalSize.Default;
            Fullscreen = ModalFullscreen.Default;
            Backdrop = true;
            Keyboard = true;
            Focus = true;
            Footer = true;
            Animation = true;
            AnimationClass = "fade";
            Loading = "Loading Content. Please Wait";
            Close = true;
            Scrollable = true;
        }
    }
}