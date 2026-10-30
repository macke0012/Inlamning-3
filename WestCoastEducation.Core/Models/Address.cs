using System.ComponentModel;

namespace WestCoastEducation.Core.Models;

public class Address
{
    [DisplayName("Gatuadress")]
    public required string Street { get; set; }
    [DisplayName("Postnummer")]
    public required string ZipCode { get; set; }
    [DisplayName("Postort")]
    public required string Town { get; set; }
    [DisplayName("Land")]
    public required string Country { get; set; }

    public override string ToString()
    {
        return $"{Street}\n{ZipCode}\n{Town}\n{Country}";
    }
    

}
