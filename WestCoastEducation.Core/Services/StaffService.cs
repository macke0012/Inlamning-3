using MongoDB.Driver;
using WestCoastEducation.Core.Interfaces;
using WestCoastEducation.Core.Models.People.Staff;

namespace WestCoastEducation.Core.Services;

public class StaffService : BaseService<BaseStaff>
{
    public List<BaseStaff> Staff { get; private set; } = [];
    public List<Teacher> Teachers => Staff.OfType<Teacher>().ToList();
    public List<Administrator> Administrators => Staff.OfType<Administrator>().ToList();
    public List<EducationLeader> EducationLeaders => Staff.OfType<EducationLeader>().ToList();

    public event EventHandler<BaseStaff>? StaffChanged;
    
    
    
    public StaffService(IStorageProvider db) : base(db)
    {
        Staff = Task.Run(db.GetAll<BaseStaff>).Result;
        
        
    }
    
    

    
   


    
}
