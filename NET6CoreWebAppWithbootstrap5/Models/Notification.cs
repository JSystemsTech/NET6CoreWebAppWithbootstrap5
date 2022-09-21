using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace NET6CoreWebAppWithbootstrap5.Models
{
    public class Notification
    {
        [JsonProperty("title")]
        [JsonPropertyName("title")]
        public string Title { get; internal set; }

        [JsonProperty("type")]
        [JsonPropertyName("type")]
        public string Type { get; internal set; }

        [JsonProperty("message")]
        [JsonPropertyName("message")]
        public string Message { get; internal set; }

        [JsonProperty("delay")]
        [JsonPropertyName("delay")]
        public int? Delay { get; internal set; }

        [JsonProperty("autohide")]
        [JsonPropertyName("autohide")]
        public bool Autohide { get; internal set; }

        [JsonProperty("html")]
        [JsonPropertyName("html")]
        public bool Html { get; internal set; }

        [JsonProperty("icon")]
        [JsonPropertyName("icon")]
        public string Icon { get; internal set; }

        
        internal Notification(string title, string message, string type, NotificationOptions options)
        {
            Title = title;
            Type = type;
            Message = message;
            Delay = options.Delay;
            Autohide = options.Autohide;
            Icon = "";
        }
        internal static Notification Primary(string title, string message, NotificationOptions options) => new Notification(title, message, "primary", options) { Icon = "fa-info-circle" };
        internal static Notification Secondary(string title, string message, NotificationOptions options) => new Notification(title, message, "secondary", options) { Icon = "fa-info-circle" };
        internal static Notification Info(string title, string message, NotificationOptions options) => new Notification(title, message, "info", options) { Icon = "fa-info-circle" };
        internal static Notification Success(string title, string message, NotificationOptions options) => new Notification(title, message, "success", options) { Icon = "fa-check-circle" };
        internal static Notification Warning(string title, string message, NotificationOptions options) => new Notification(title, message, "warning", options) { Icon = "fa-exclamation-circle" };
        internal static Notification Danger(string title, string message, NotificationOptions options) => new Notification(title, message, "danger", options) { Icon = "fa-exclamation-triangle" };
        internal static Notification Error(string title, Exception ex, NotificationOptions options) => new Notification(title, ex.Message, "danger", options) { Icon = "fa-exclamation-triangle" };
    }
    public class NotificationOptions
    {
        public int? Delay { get; set; }
        public bool Autohide { get; set; }
    }
    public interface INotificationFactory
    {
        Notification Primary(string title, string message);
        Notification Secondary(string title, string message);
        Notification Info(string title, string message);
        Notification Success(string title, string message);
        Notification Warning(string title, string message);
        Notification Danger(string title, string message);
        Notification Error(string title, Exception ex);
    }
    public class NotificationFactory: INotificationFactory
    {
        private NotificationOptions _notificationOptions;
        public NotificationFactory(IOptions<NotificationOptions> notificationOptions)
        {
            _notificationOptions = notificationOptions.Value;
        }
        public Notification Primary(string title, string message) => Notification.Primary(title, message, _notificationOptions);
        public Notification Secondary(string title, string message) => Notification.Secondary(title, message, _notificationOptions);
        public Notification Info(string title, string message) => Notification.Info(title, message, _notificationOptions);
        public Notification Success(string title, string message) => Notification.Success(title, message, _notificationOptions);
        public Notification Warning(string title, string message) => Notification.Warning(title, message, _notificationOptions);
        public Notification Danger(string title, string message) => Notification.Danger(title, message, _notificationOptions);
        public Notification Error(string title, Exception ex) => Notification.Error(title, ex, _notificationOptions);
    }
}
