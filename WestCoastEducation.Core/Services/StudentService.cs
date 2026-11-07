using WestCoastEducation.Core.Interfaces;
using WestCoastEducation.Core.Models.People;


namespace WestCoastEducation.Core.Services;

public class StudentService(IStorageProvider db) : BaseService<Student>(db)
{
}
