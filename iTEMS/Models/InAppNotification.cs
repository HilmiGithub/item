namespace iTEMS.Models
{
    public class InAppNotification
    {
        public int Id { get; set; } // Unique identifier for the notification
        public string UserName { get; set; } // Username of the recipient user
        public string Message { get; set; } // Content of the notification
        public DateTime Timestamp { get; set; } // Timestamp indicating when the notification was created
    }

}
