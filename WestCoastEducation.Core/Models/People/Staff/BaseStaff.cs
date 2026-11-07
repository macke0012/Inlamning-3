
using MongoDB.Bson.Serialization.Attributes;

namespace WestCoastEducation.Core.Models.People.Staff;

[BsonKnownTypes(typeof(Teacher), typeof(EducationLeader), typeof(Administrator))]
public class BaseStaff : Person
{
    
}