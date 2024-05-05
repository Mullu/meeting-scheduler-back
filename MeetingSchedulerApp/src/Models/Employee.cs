namespace MeetingSchedulerApp.src.Models
{
    public class Employee
    {
        public string EmployeeId { get; set; }
        public string DisplayName { get; set; }
        public List<BusySlot> BusySlots { get; set; }
    }

}
