using System.ComponentModel;

namespace WestCoastEducation.Core.Models;

public class Address
{
    
    public required string Street { get; set; }
    public required string ZipCode { get; set; }
    public required string Town { get; set; }
    public required Country Country { get; set; }

    public override string ToString()
    {
        return $"{Street}\n{ZipCode}\n{Town}\n{Country}";
    }
    

}
