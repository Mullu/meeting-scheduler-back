using MeetingSchedulerApp.src.Services.Scheduler;
using Xunit;

namespace MeetingSchedulerApp.Tests
{
    public class MeetingSchedulerServiceTest
    {
        private readonly MeetingSchedulerService _scheduler;
        private readonly List<string> _participantIds;

        public MeetingSchedulerServiceTest()
        {
            _scheduler = new MeetingSchedulerService();
            _participantIds = new List<string>()
            {
                "170378154979885419149243073079764064027",
                "139016136604805407078985976850150049467"
            };
        }

        [Theory]
        [InlineData(30, "2015-01-03 11:30", "2015-01-04 11:30", 3, new string[] { "2015-01-03 11:30", "2015-01-03 13:30", "2015-01-04 08:00" })]
        [InlineData(60, "2015-01-04 08:00", "2015-01-04 09:00", 3, new string[] { "2015-01-04 08:00", "2015-01-04 08:30", "2015-01-04 09:00" })]
        public void GetMeetingSuggestions_ValidInput_ReturnsExpectedMeetingSuggestions(
            int meetingLength,
            string meetingStartTime,
            string meetingEndTime,
            int expectedCount,
            string[] expectedSuggestions)
        {
            // Act
            var suggestions = _scheduler.GetMeetingSuggestions(
                _participantIds,
                meetingLength,
                meetingStartTime,
                meetingEndTime);

            // Assert
            Assert.Equal(expectedCount, suggestions.Count);

            for (int i = 0; i < expectedCount; i++)
            {
                Assert.Equal(DateTime.Parse(expectedSuggestions[i]), suggestions[i]);
            }
        }

        [Fact]
        public void GetMeetingSuggestions_InvalidStartTimeInput_ThrowsArgumentException()
        {
            // Arrange
            string invalidStartTime = "2023-13-01 08:00"; // Invalid month
            string validEndTime = "2023-01-01 09:00";
            int meetingLength = 60;
            string expectedErrorMessage = "Invalid start time format. Expected format: yyyy-MM-dd HH:mm";

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
            {
                _scheduler.GetMeetingSuggestions(
                    _participantIds,
                    meetingLength,
                    invalidStartTime,
                    validEndTime);
            });

            Assert.Equal(expectedErrorMessage, exception.Message);
        }

        [Fact]
        public void GetMeetingSuggestions_InvalidEndTimeInput_ThrowsArgumentException()
        {
            // Arrange
            string validStartTime = "2023-01-01 08:00";
            string inValidEndTime = "2023-01-01 24:20"; // Invalid hour input
            int meetingLength = 60;
            string expectedErrorMessage = "Invalid end time format. Expected format: yyyy-MM-dd HH:mm";

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
            {
                _scheduler.GetMeetingSuggestions(
                    _participantIds,
                    meetingLength,
                    validStartTime,
                    inValidEndTime);
            });

            Assert.Equal(expectedErrorMessage, exception.Message);
        }

        [Fact]
        public void GetValidatedDateTimes_InvalidMeetingLength_ThrowsArgumentException()
        {
            // Arrange
            string startTime = "2023-01-01 08:00";
            string endTime = "2023-01-01 09:00";
            int inValidMeetingLength = -60; // Negative meeting length
            string expectedErrorMessage = "Meeting length should be a positive integer multiple of 30 minutes.";

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
            {
                _scheduler.GetMeetingSuggestions(
                    _participantIds,
                    inValidMeetingLength,
                    startTime,
                    endTime);
            });

            Assert.Equal(expectedErrorMessage, exception.Message);
        }
    }
}
