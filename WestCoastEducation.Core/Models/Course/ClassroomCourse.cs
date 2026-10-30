using WestCoastEducation.Core.Models.People.Staff;

namespace WestCoastEducation.Core.Models.Course;

public class ClassroomBaseCourse : TeacherLeadBaseCourse
{
    public Address Location { get; set; }
    public string LectureHall { get; set; }
    public override CourseType CourseType => CourseType.Classroom;

    public ClassroomBaseCourse(Address location, string lectureHall)
    {
        Location = location;
        LectureHall = lectureHall;
    }

    public override string ToString() => $"{base.ToString()}\nLecture Hall: {LectureHall}\n{Location}";
}
