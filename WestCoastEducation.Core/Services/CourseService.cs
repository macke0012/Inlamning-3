using WestCoastEducation.Core.Interfaces;
using WestCoastEducation.Core.Models.Course;

namespace WestCoastEducation.Core.Services;

public class CourseService : BaseService<BaseCourse> 
{
    public List<BaseCourse> Courses { get; private set; } = [];
    public List<RemoteCourse> RemoteCourses => Courses.OfType<RemoteCourse>().ToList();
    public List<ClassroomCourse> ClassroomCourses => Courses.OfType<ClassroomCourse>().ToList();

    public CourseService(IStorageProvider db) : base(db)
    {
        Courses = Task.Run(db.GetAll<BaseCourse>).Result;

    }











}
