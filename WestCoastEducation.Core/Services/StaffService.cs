using WestCoastEducation.Core.Interfaces;
using WestCoastEducation.Core.Models.People.Staff;

namespace WestCoastEducation.Core.Services;

public class StaffService(IStorageProvider db) : BaseService<BaseStaff>(db)
{
    
}
