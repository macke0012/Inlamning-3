using WestCoastEducation.Core.Interfaces;

namespace WestCoastEducation.Core.Models.People;

public abstract class Person : IStorageObject
{

    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Ssn { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Email { get; set; }
    public required Address Address { get; set; }
    public string FullName => $"{FirstName} {LastName}";
    public string Id { get; init; } = Guid.NewGuid().ToString();

    public override string ToString()
    {
        return $"{FullName}\nSSN: {Ssn}\nPhone: {PhoneNumber}\nEmail: {Email}\n{Address}";
    }
}
