using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using WestCoastEducation.Core.Models.Course;

namespace WestCoastEducation.Core.Models.People;

public class Student : Person
{
    
    [BsonIgnore]
    public List<Course.BaseCourse> Courses { get; private set; } = [];

    private List<string> _courseIds = new List<string>();
    

    public void Enroll(Course.BaseCourse baseCourse)
    {
        if (Courses.Any(c => c.Id == baseCourse.Id))
        {
            return;
        }
        Courses.Add(baseCourse);
        _courseIds.Add(baseCourse.Id);
    }

    public void Unenroll(Course.BaseCourse baseCourse)
    {
        Courses.RemoveAll(c => c.Id == baseCourse.Id);
        _courseIds.RemoveAll(c => c == baseCourse.Id);
    }

    public override string ToString()
    {
        string text = base.ToString();

        if (Courses.Count == 0)
        {
            return text + "\nNot enrolled in any courses";
        }

        return text;
    }
}
