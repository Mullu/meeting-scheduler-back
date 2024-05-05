using MeetingSchedulerApp.src.Configuration;
using MeetingSchedulerApp.src.Models;
using MeetingSchedulerApp.src.Utility;

namespace MeetingSchedulerApp.src.Services.Scheduler
{
    public class MeetingSchedulerService : IMeetingSchedulerService
    {
        public List<DateTime> GetMeetingSuggestions(
            List<string> participantIds,
            int meetingLength,
            string meetingStartTime,
            string meetingEndTime)
        {
            var (parsedMeetingStartTime, parsedMeetingEndTime) = InputValidator
                .GetValidatedDateTimes(meetingStartTime, meetingEndTime, meetingLength);

            int totalIntervals = (int)(parsedMeetingEndTime - parsedMeetingStartTime).TotalMinutes / 30 + 1;

            var timeSlots = Enumerable.Range(0, totalIntervals)
                .Select(i => parsedMeetingStartTime.AddMinutes(i * 30))
                .Select(startTime => (startTime, startTime.AddMinutes(meetingLength)))
                .ToList();

            var validTimeSlots = timeSlots.Where(slot =>
                IsWithinWorkingHours(slot.Item1, slot.Item2) &&
                AreAllParticipantsAvailable(participantIds, slot.Item1, slot.Item2))
                .ToList();

            var suggestions = validTimeSlots
                .OrderBy(slot => slot.Item1)
                .Take(3)
                .Select(slot => slot.Item1)
                .ToList();

            return suggestions;
        }

        private bool IsWithinWorkingHours(DateTime startDateTime, DateTime endDateTime)
        {
            return startDateTime.Hour >= 8
                && startDateTime.Hour < 17
                && (endDateTime.Hour < 17
                || endDateTime.Hour == 17 && endDateTime.Minute == 0);
        }

        public List<Employee> LoadEmployees()
        {
            return FreeBusyParser.ParseFile(AppConfig.FreeBusyFilePath);
        }

        private bool AreAllParticipantsAvailable(List<string> participantIds, DateTime start, DateTime end)
        {
            var employees = LoadEmployees();

            return participantIds.All(id =>
            {
                Employee participant = employees.FirstOrDefault(e => e.EmployeeId == id);

                return participant != null &&
                       participant.BusySlots.All(busySlot => !AreTimePeriodsOverlapping(busySlot, start, end));
            });
        }

        private bool AreTimePeriodsOverlapping(BusySlot busySlot, DateTime start, DateTime end)
        {
            return busySlot.StartTime < end && busySlot.EndTime > start;
        }
    }
}
