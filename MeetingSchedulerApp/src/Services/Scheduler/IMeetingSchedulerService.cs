namespace MeetingSchedulerApp.src.Services.Scheduler
{
    public interface IMeetingSchedulerService
    {
        public List<DateTime> GetMeetingSuggestions(
            List<string> participantIds,
            int meetingLength,
            string meetingStartTime,
            string meetingEndTime);
    }
}
