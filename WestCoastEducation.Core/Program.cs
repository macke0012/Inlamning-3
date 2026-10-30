using Microsoft.Extensions.DependencyInjection;
using WestCoastEducation.Core.Models;
using WestCoastEducation.Core.Models.Course;
using WestCoastEducation.Core.Models.People;

namespace WestCoastEducation.Core;

class Program
{
    static void Main(string[] args)
    {
        var services = new ServiceCollection();
        services.AddServices();
        var provider = services.BuildServiceProvider();

        var school = provider.GetRequiredService<School>();

       




    }
}
