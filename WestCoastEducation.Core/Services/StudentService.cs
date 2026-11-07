using WestCoastEducation.Core.Interfaces;
using WestCoastEducation.Core.Models.People;


namespace WestCoastEducation.Core.Services;

public class StudentService : BaseService<Student>
{
    public List<Student> Students { get; private set; } = [];
    public StudentService(IStorageProvider db) : base(db)
    {
        
        
        
    }

    

}
