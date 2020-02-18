using System;

namespace TimeTrackerApp.Extra
{
    public class TimeInfo
    {
        public string ProjectName { get; set; }
        public string Time { get; set; }
        public TimeInfo(string projectName, TimeSpan time)
        {
            ProjectName = projectName;
            Time = string.Format("{0:00}:{1:00}", time.Hours, time.Minutes);
        }
    }
}
