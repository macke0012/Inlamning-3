using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using WestCoastEducation.Core.Interfaces;

namespace WestCoastEducation.Core.Models.People;

public abstract class Person : IStorageObject
{

    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string SSN { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Email { get; set; }
    public required Address Address { get; set; }
    public string Role => GetType().GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? GetType().Name;    
    public string FullName => $"{FirstName} {LastName}";
    public string Id { get; init; } = Guid.NewGuid().ToString();

    public override string ToString()
    {
        return $"{FullName}\nSSN: {SSN}\nPhone: {PhoneNumber}\nEmail: {Email}\n{Address}";
    }
}
