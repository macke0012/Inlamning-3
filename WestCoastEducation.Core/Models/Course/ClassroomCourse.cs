using MongoDB.Bson.Serialization.Attributes;
using WestCoastEducation.Core.Models.People.Staff;

namespace WestCoastEducation.Core.Models.Course;

[BsonDiscriminator("ClassroomCourse")]
public class ClassroomCourse : TeacherLeadBaseCourse
{
    public Address Location { get; set; }
    public string LectureHall { get; set; }
    public override CourseType CourseType => CourseType.Classroom;

    public ClassroomCourse(Address location, string lectureHall)
    {
        Location = location;
        LectureHall = lectureHall;
    }

    public override string ToString() => $"{base.ToString()}\nLecture Hall: {LectureHall}\n{Location}";
}
