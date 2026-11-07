using MongoDB.Bson.Serialization.Attributes;
using WestCoastEducation.Core.Enum;

namespace WestCoastEducation.Core.Models.Course;

public enum Platform { Zoom, Teams, GoogleMeet, Discord }

[BsonDiscriminator("RemoteCourse")]
public class RemoteCourse : TeacherLeadBaseCourse
{
    public required Platform Platform { get; set; }
    public required string MeetingLink { get; set; }
    public override CourseType CourseType => CourseType.Remote;

    

    public override string ToString() => $"{base.ToString()}\nPlatform: {Platform}\nMeeting: {MeetingLink}";
}
