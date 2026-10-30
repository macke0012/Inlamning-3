using WestCoastEducation.Core.Models.People.Staff;

namespace WestCoastEducation.Core.Models.Course;

public enum Platform { Zoom, Teams, GoogleMeet, Discord }


public class RemoteBaseCourse : TeacherLeadBaseCourse
{
    public Platform Platform { get; set; }
    public string MeetingLink { get; set; }
    public override CourseType CourseType => CourseType.Remote;

    public RemoteBaseCourse(Platform platform, string meetingLink)
    {
        Platform = platform;
        MeetingLink = meetingLink;
    }

    public override string ToString() => $"{base.ToString()}\nPlatform: {Platform}\nMeeting: {MeetingLink}";
}
