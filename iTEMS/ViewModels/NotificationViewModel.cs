using iTEMS.Models;

namespace iTEMS.ViewModels
{
    public class NotificationViewModel
    {
        public List<Notification> Notifications { get; set; }
        public bool HasNewNotifications => Notifications != null && Notifications.Any(n => !n.IsSent);
    }
}
