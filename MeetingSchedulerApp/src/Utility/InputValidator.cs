namespace MeetingSchedulerApp.src.Utility
{
    public static class InputValidator
    {
        public static (DateTime, DateTime) GetValidatedDateTimes(string meetingStartTime, string meetingEndTime, int meetingLength)
        {
            DateTime parsedMeetingStartTime, parsedMeetingEndTime;

            ValidateDateTimeFormat(meetingStartTime, meetingEndTime);

            if (!DateTime.TryParseExact(meetingStartTime, "yyyy-MM-dd HH:mm", null, System.Globalization.DateTimeStyles.AssumeLocal, out parsedMeetingStartTime))
                throw new ArgumentException("Invalid start time format. Expected format: yyyy-MM-dd HH:mm");

            if (!DateTime.TryParseExact(meetingEndTime, "yyyy-MM-dd HH:mm", null, System.Globalization.DateTimeStyles.AssumeLocal, out parsedMeetingEndTime))
                throw new ArgumentException("Invalid end time format. Expected format: yyyy-MM-dd HH:mm");

            if (parsedMeetingStartTime >= parsedMeetingEndTime)
                throw new ArgumentException("Meeting start time should be before the end time.");

            ValidateMeetingLength(meetingLength);

            return (parsedMeetingStartTime, parsedMeetingEndTime);
        }

        private static void ValidateDateTimeFormat(string startTime, string endTime)
        {
            if (!DateTime.TryParseExact(startTime, "yyyy-MM-dd HH:mm", null, System.Globalization.DateTimeStyles.AssumeLocal, out _))
                throw new ArgumentException("Invalid start time format. Expected format: yyyy-MM-dd HH:mm");

            if (!DateTime.TryParseExact(endTime, "yyyy-MM-dd HH:mm", null, System.Globalization.DateTimeStyles.AssumeLocal, out _))
                throw new ArgumentException("Invalid end time format. Expected format: yyyy-MM-dd HH:mm");
        }

        private static void ValidateMeetingLength(int meetingLength)
        {
            if (meetingLength <= 0 || meetingLength % 30 != 0)
                throw new ArgumentException("Meeting length should be a positive integer multiple of 30 minutes.");
        }
    }
}
