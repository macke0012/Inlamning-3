using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using WestCoastEducation.Core.Interfaces;

namespace WestCoastEducation.Core.Models.People;

public abstract class Person : IStorageObject
{
    [Display(Name = "Förnamn")]
    public required string FirstName { get; set; }
    [Display(Name = "Efternamn")]
    public required string LastName { get; set; }
    [Display(Name ="Personnummer")]
    public required string SSN { get; set; }
    [Display(Name = "Telefonnummer")]
    public required string PhoneNumber { get; set; }
    [Display(Name = "E-postadress")]
    public required string Email { get; set; }
    [Display(Name = "Adress")]
    public required Address Address { get; set; }
    [BsonIgnore]
    [DisplayName("Roll")]
    public string Role => GetType().GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? GetType().Name;
    
    [JsonIgnore]
    public string FullName => $"{FirstName} {LastName}";
    [Editable(false)]
    public string Id { get; init; } = Guid.NewGuid().ToString();

    public override string ToString()
    {
        return $"{FullName}\nSSN: {SSN}\nPhone: {PhoneNumber}\nEmail: {Email}\n{Address}";
    }
}
