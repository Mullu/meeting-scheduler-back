using MeetingSchedulerApp.src.Services.Scheduler;

namespace MeetingSchedulerApp.src
{
    public class MeetingSchedulerAppRunner
    {
        private readonly IMeetingSchedulerService _meetingSchedulerService;
        public MeetingSchedulerAppRunner(IMeetingSchedulerService meetingSchedulerService)
        {
            _meetingSchedulerService = meetingSchedulerService;
        }

        public static void Main(string[] args)
        {
            var meetingSchedulerService = new MeetingSchedulerService();
            var runner = new MeetingSchedulerAppRunner(meetingSchedulerService);

            Console.WriteLine("Enter participant IDs (separated by comma):");
            List<string> participantIds = new List<string>(Console.ReadLine().Split(','));

            Console.WriteLine("Enter meeting length (minutes):");
            int meetingLength = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter meeting start time (yyyy-MM-dd HH:mm):");
            string meetingStartTime = Console.ReadLine();

            Console.WriteLine("Enter meeting end time (yyyy-MM-dd HH:mm):");
            string meetingEndTime = Console.ReadLine();

            runner.ProvideMeetingTimeSuggestions(
                "Input",
                participantIds,
                meetingLength,
                meetingStartTime,
                meetingEndTime);

            // Keep the following lines commented out as requested
            //TestMeetingSuggestions(
            //    "Input1",
            //    scheduler,
            //    new List<string>()
            //{
            //    "170378154979885419149243073079764064027",
            //    "139016136604805407078985976850150049467"
            //},
            //    30,
            //    "2015-01-03 11:30",
            //    "2015-01-04 11:30");

            //TestMeetingSuggestions(
            //    "Input2",
            //    scheduler,
            //new List<string>()
            //{
            //    "170378154979885419149243073079764064027",
            //    "139016136604805407078985976850150049467"
            //},
            //    60,
            //    "2015-01-03 11:30",
            //    "2015-01-04 11:30");
        }

        public void ProvideMeetingTimeSuggestions(
           string inputName,
           List<string> participantIds,
           int meetingLength,
           string meetingStartTime,
           string meetingEndTime)
        {
            Console.WriteLine($"Testing {inputName}:");

            List<DateTime> suggestions = _meetingSchedulerService.GetMeetingSuggestions(participantIds, meetingLength, meetingStartTime, meetingEndTime);

            Console.WriteLine("Meeting suggestions:");
            foreach (var suggestion in suggestions)
            {
                Console.WriteLine(suggestion.ToString("yyyy-MM-dd HH:mm"));
            }

            Console.WriteLine();
        }
    }
}
