
using MongoDB.Bson.Serialization.Attributes;
using WestCoastEducation.Core.Models.Course;

namespace WestCoastEducation.Core.Models.People.Staff;

[BsonKnownTypes(typeof(Teacher), typeof(EducationLeader), typeof(Administrator))]
public class BaseStaff : Person
{
    
}