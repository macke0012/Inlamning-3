using MongoDB.Bson.Serialization.Attributes;
using WestCoastEducation.Core.Enum;

namespace WestCoastEducation.Core.Models.Course;

[BsonDiscriminator("ClassroomCourse")]
public class ClassroomCourse : TeacherLeadBaseCourse
{
    public required Address Location { get; set; }
    public required string LectureHall { get; set; }
    public override CourseType CourseType => CourseType.Classroom;
    

    public override string ToString() => $"{base.ToString()}\nLecture Hall: {LectureHall}\n{Location}";
}
