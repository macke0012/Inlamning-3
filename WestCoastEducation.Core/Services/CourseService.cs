using MongoDB.Driver;
using WestCoastEducation.Core.Interfaces;
using WestCoastEducation.Core.Models.Course;
using WestCoastEducation.Core.Repositories;

namespace WestCoastEducation.Core.Services;

public class CourseService : BaseService<BaseCourse> 
{
    public List<BaseCourse> Courses { get; private set; } = [];
    public List<RemoteCourse> RemoteCourses => Courses.OfType<RemoteCourse>().ToList();
    public List<ClassroomCourse> ClassroomCourses => Courses.OfType<ClassroomCourse>().ToList();

    public CourseService(IStorageProvider db) : base(db)
    {
        Courses = Task.Run(db.GetAll<BaseCourse>).Result;

        if (_cursor is null) return;

        _cursor.ForEachAsync(change =>
        {
            if (change.OperationType == ChangeStreamOperationType.Delete)
            {
                Courses.RemoveAll(c => c.Id == change.FullDocument.Id);
                return;
            }
            Console.WriteLine(change.FullDocument);
            Courses.RemoveAll(c => c.Id == change.FullDocument.Id);
            Courses.Add(change.FullDocument);
            
        });

    }











}
