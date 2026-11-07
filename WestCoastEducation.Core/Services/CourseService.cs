using WestCoastEducation.Core.Interfaces;
using WestCoastEducation.Core.Models.Course;

namespace WestCoastEducation.Core.Services;

public class CourseService(IStorageProvider db) : BaseService<BaseCourse>(db)
{
   
}
