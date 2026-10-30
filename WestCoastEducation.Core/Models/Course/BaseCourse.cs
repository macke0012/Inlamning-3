using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using WestCoastEducation.Core.Interfaces;

namespace WestCoastEducation.Core.Models.Course;

[JsonDerivedType(typeof(RemoteBaseCourse), "remote")]
[JsonDerivedType(typeof(ClassroomBaseCourse), "classroom")]
[BsonKnownTypes(typeof(RemoteBaseCourse), typeof(ClassroomBaseCourse))]//Detta så att json serialization ska veta vilken typ av kurs det är när man deserialiserar.

public abstract class BaseCourse : IStorageObject
{
    public required string CourseNumber { get; set; }
    public required string Title { get; set; }
    public required DateOnly StartDate { get; set; }
    public required DateOnly EndDate { get; set; }
    public int DurationInWeeks => (EndDate.DayNumber - StartDate.DayNumber) / 7;

    public abstract CourseType CourseType { get; }

    public string Id { get; } = Guid.NewGuid().ToString();

    public override string ToString() =>
        $"Course Number: {CourseNumber}\nTitle: {Title}\nType: {CourseType}\nDates: {StartDate:yyyy-MM-dd} - {EndDate:yyyy-MM-dd} ({DurationInWeeks} weeks)";
}
