using WestCoastEducation.Core.Models;
using WestCoastEducation.Core.Models.Course;
using WestCoastEducation.Core.Models.People;
using WestCoastEducation.Core.Models.People.Staff;


namespace WestCoastEducation.Core;

//Denna klass och DataLoader är AI-genererad då jag inte orkade komma på flera olika exempel på personer.
public static class DataCreator
{
    public static void CreateAndSave(School school)
    {
        var anna = new Teacher
        {
            FirstName = "Anna",
            LastName = "Svensson",
            SSN = "19800312-1234",
            PhoneNumber = "0701112233",
            Email = "anna@wce.se",
            Address = CreateAdress("Storgatan 1", "41101", "Göteborg"),
            KnowledgeArea = "C# och .NET"
        };
        var erik = new Teacher
        {
            FirstName = "Erik",
            LastName = "Lindqvist",
            SSN = "19751120-5678",
            PhoneNumber = "0702223344",
            Email = "erik@konsult.se",
            Address = CreateAdress("Kungsgatan 12", "11143", "Stockholm"),
            KnowledgeArea = "Databaser"
        };
        school.AddStaff(anna);
        school.AddStaff(erik);

        var oop = new RemoteCourse(Platform.Teams, "https://teams.microsoft.com/wce-oop")
        {
            CourseNumber = "WCE-101",
            Title = "Objektorienterad programmering",
            StartDate = new DateOnly(2026, 11, 2),
            EndDate = new DateOnly(2026, 12, 18),

        };
        oop.AssignTeacher(anna);
        var sql = new ClassroomCourse(CreateAdress("Lärargatan 5", "41102", "Göteborg"), "Sal A1")
        {
            CourseNumber = "WCE-201",
            Title = "SQL och databasdesign",
            StartDate = new DateOnly(2026, 11, 9),
            EndDate = new DateOnly(2026, 12, 4),

        };
        sql.AssignTeacher(erik);
        school.AddCourse(oop);
        school.AddCourse(sql);
        school.AssignCourseToTeacher(anna, oop);
        school.AssignCourseToTeacher(erik, sql);

        var johan = new Student
        {
            FirstName = "Johan",
            LastName = "Ek",
            SSN = "20010101-1111",
            PhoneNumber = "0731000001",
            Email = "johan@student.se",
            Address = CreateAdress("Vasagatan 2", "41124", "Göteborg")
        };
        var marta = new Student
        {
            FirstName = "Märta",
            LastName = "Ståhl",
            SSN = "20020202-2222",
            PhoneNumber = "0731000002",
            Email = "marta@student.se",
            Address = CreateAdress("Drottninggatan 8", "11151", "Stockholm")
        };
        school.AddStudent(johan);
        school.AddStudent(marta);
        school.EnrollStudent(johan, oop);
        school.EnrollStudent(johan, sql);
        school.EnrollStudent(marta, oop);

        var karin = new EducationLeader
        {
            FirstName = "Karin",
            LastName = "Åkesson",
            SSN = "19700707-7777",
            PhoneNumber = "0709000001",
            Email = "karin@wce.se",
            Address = CreateAdress("Avenyn 10", "41136", "Göteborg"),
            EmploymentDate = new DateTime(2015, 8, 17)
        };
        school.AddStaff(karin);
        school.AssignTeacherToLeader(karin, anna);
        school.AssignTeacherToLeader(karin, erik);

        var maria = new Administrator
        {
            FirstName = "Maria",
            LastName = "Lund",
            SSN = "19850909-9999",
            PhoneNumber = "0709000003",
            Email = "maria@wce.se",
            Address = CreateAdress("Hamngatan 1", "41106", "Göteborg"),
            EmploymentDate = new DateTime(2018, 3, 1)
        };
        school.AddStaff(maria);
    }

    private static Address CreateAdress(string street, string zipCode, string town)
    {
        return new Address { Street = street, ZipCode = zipCode, Town = town, Country = "Sweden" };
    }
}
