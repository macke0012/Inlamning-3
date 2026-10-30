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


    public StaffService(IStorageProvider db) : base(db)
    {
        Staff = Task.Run(db.GetAll<BaseStaff>).Result;
        
        if (_cursor is null) return;

        _cursor.ForEachAsync(change =>
        {
            
            if (change.OperationType == ChangeStreamOperationType.Delete)
            {
                Staff.RemoveAll(s => s.Id == change.FullDocument.Id);
                return;
            }
            Staff.RemoveAll(s => s.Id == change.FullDocument.Id);
            Staff.Add(change.FullDocument);
        });
    }
    
    

    
   


    
}
