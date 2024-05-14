namespace iTEMS.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string? Message { get; set; }
        public DateTime Timestamp { get; set; }
        public string UserName { get; set; } // Foreign key to the employee's email
        public Employee Employee { get; set; } // Navigation property to the employee receiving the notification
        public bool IsSent { get; set; }
        public bool IsOpened { get; set; }
    }

}
