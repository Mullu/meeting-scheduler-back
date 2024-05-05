using MeetingSchedulerApp.src.Models;
using Serilog;

namespace MeetingSchedulerApp.src.Utility
{
    public static class FreeBusyParser
    {
        public static List<Employee> ParseFile(string filePath)
        {
            List<Employee> employees = new List<Employee>();

            try
            {
                var lines = File.ReadLines(filePath);

                foreach (var line in lines)
                {
                    string[] parts = line.Split(';');

                    if (parts.Length.Equals(2))
                    {
                        Employee employee = CreateEmployee(parts);
                        employees.Add(employee);
                    }
                    else if (parts.Length >= 3)
                    {
                        AddBusySlotToEmployee(parts, employees);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error parsing file: {ex.Message}");
            }

            return employees;
        }

        private static Employee CreateEmployee(string[] parts)
        {
            return new Employee
            {
                EmployeeId = parts[0],
                DisplayName = parts[1],
                BusySlots = new List<BusySlot>()
            };
        }

        private static void AddBusySlotToEmployee(string[] parts, List<Employee> employees)
        {
            Employee employee = employees.Find(e => e.EmployeeId == parts[0]);

            if (employee != null)
            {
                BusySlot busySlot = ParseBusySlot(parts);
                employee.BusySlots.Add(busySlot);
            }
        }

        private static BusySlot ParseBusySlot(string[] parts)
        {
            return new BusySlot
            {
                StartTime = DateTime.Parse(parts[1]),
                EndTime = DateTime.Parse(parts[2])
            };
        }
    }
}
