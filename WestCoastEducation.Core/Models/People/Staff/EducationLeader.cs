using System.ComponentModel;
using System.Text.Json.Serialization;
using WestCoastEducation.Core.Models.Course;

namespace WestCoastEducation.Core.Models.People.Staff;
[DisplayName("Utbildningsledare")]
public class EducationLeader : PermanentStaff
{
    [JsonInclude]
    private List<Teacher> _responsibleTeachers = new List<Teacher>(); //Utbildningsledaren ansvarar för en eller flera lärare. 

    [JsonIgnore]
    public IReadOnlyCollection<TeacherLeadBaseCourse> ResponsibleCourses => _responsibleTeachers //Utbildningsledaren har alla kurser som lärarna ansvarar för.
        .SelectMany(t => t.Courses)
        .OfType<TeacherLeadBaseCourse>()
        .DistinctBy(c => c.Id)
        .ToList();

    [JsonIgnore]
    public IReadOnlyCollection<Teacher> ResponsibleTeachers => _responsibleTeachers;

    public void AssignTeacher(Teacher teacher)
    {
        if (IsTeacherAssigned(teacher)) return;
        _responsibleTeachers.Add(teacher);
    }

    public void RemoveTeacher(Teacher teacher)
    {
        _responsibleTeachers.RemoveAll(t => t.Id == teacher.Id);
    }

    public Teacher? GetTeacher(string id) => _responsibleTeachers.FirstOrDefault(t => t.Id == id);

    public List<Teacher> GetTeacher(string LastName, string FirstName)
    {
        return _responsibleTeachers.Where(t => t.LastName.Contains(LastName) && t.FirstName.Contains(FirstName)).ToList();
    }

    private bool IsTeacherAssigned(Teacher teacher) => _responsibleTeachers.Any(t => t.Id == teacher.Id);

    public override string ToString()
    {
        string text = base.ToString();

        if (_responsibleTeachers.Count == 0)
        {
            return text + "\nNot responsible for any teachers";
        }

        text += "\nResponsible for teachers:";
        foreach (var teacher in _responsibleTeachers)
        {
            text += $"\n\t{teacher.FullName} ({teacher.KnowledgeArea})";
        }
        return text;
    }
}