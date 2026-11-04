using System.ComponentModel;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using WestCoastEducation.Core.Models.Course;
using WestCoastEducation.Core.Enum;

namespace WestCoastEducation.Core.Models.People.Staff;

public class Teacher : BaseStaff
{
    private List<Competencies> _compentencies { get; set; }

    public string KnowledgeArea
    {
        get
        {
            string value = "";
            foreach (var comp in _compentencies) value += comp.ToString() + "\n";
            return value;
        }
    }

    [JsonIgnore]
    [BsonIgnore]
    public List<BaseCourse> Courses { get; private set; } = [];
    
    public List<string> CourseIds { get; private set; } = [];

    public void AddCourse(BaseCourse baseCourse)
    {
        if (Courses.Any(c => c.Id == baseCourse.Id))
        {
            return;
        }
        Courses.Add(baseCourse);
        CourseIds.Add(baseCourse.Id);
    }
    
    public void AddCompetencies(params List<Competencies> competencies)
    {
        _compentencies.AddRange(competencies);
    }

    public void RemoveCourse(BaseCourse baseCourse)
    {
        Courses.RemoveAll(c => c.Id == baseCourse.Id);
    }

    public override string ToString()
    {
        string text = $"{base.ToString()}\nKnowledge Area: {KnowledgeArea}";

        if (Courses.Count == 0)
        {
            return text + "\nNot responsible for any courses";
        }

        text += "\nResponsible for:";
        foreach (var course in Courses)
        {
            text += $"\n\t{course.CourseNumber} {course.Title} ({course.CourseType})";
        }
        return text;
    }
}
