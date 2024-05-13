namespace iTEMS.Models
{
    public class Notify
    {
        public int Id { get; set; }
        public string? Message { get; set; }
        public DateTime Timestamp { get; set; }
        public string EmployeeEmail { get; set; } // Foreign key to the employee's email
        public Employee Employee { get; set; } // Navigation property to the employee receiving the notification
    }
}
