using WestCoastEducation.Core.Repositories;
namespace WestCoastEducation.Core;

static class DataLoader
{
    public static void LoadAndPrint(School school)
    {
        PrintAll("Courses", Task.Run(school.GetAllCourses).Result);
        PrintAll("Teachers", Task.Run(school.GetAllTeachers).Result);
        PrintAll("Students", Task.Run(school.GetAllStudents).Result);
        PrintAll("Staff", Task.Run(school.GetAllStaff).Result);
    }

    

    //QOL Metod
    private static void PrintAll<T>(string heading, List<T> items)
    {
        Console.WriteLine($"===== {heading} ({items.Count}) =====");

        if (items.Count == 0)
        {
            Console.WriteLine("Nothing saved yet");
            Console.WriteLine();
            return;
        }

        foreach (var item in items)
        {
            Console.WriteLine(item);
            Console.WriteLine();
        }
    }
}
