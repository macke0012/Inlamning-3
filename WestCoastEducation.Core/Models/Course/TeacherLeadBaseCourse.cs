using MongoDB.Bson.Serialization.Attributes;
using WestCoastEducation.Core.Models.People.Staff;

namespace WestCoastEducation.Core.Models.Course;

public abstract class TeacherLeadBaseCourse : BaseCourse
{
    [BsonIgnore]
    public Teacher? Teacher { get; set; } 
    private string? _teacherId;
    //LärarId så att man ska kunna hitta läraren som ansvarar för kursen.

    
    
    public void AssignTeacher(Teacher teacher) => Teacher = teacher;
}
