using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using WestCoastEducation.Core;
using WestCoastEducation.Core.Models.Course;
using WestCoastEducation.Core.Models.People;
using WestCoastEducation.Core.Models.People.Staff;

namespace WestCoastEducation.Client.ViewModels;

public partial class HomeViewModel : ViewModelBase
{
    private readonly School _school;
    [ObservableProperty] public partial ObservableCollection<Teacher> Teachers { get; set; } = [];
    [ObservableProperty] public partial ObservableCollection<BaseCourse> Courses { get; set; } = [];
    [ObservableProperty] public partial ObservableCollection<Student> Students { get; set; } = [];
    public HomeViewModel(School school)
    {
        _school = school;
        Task.Run(Init);
    }

    public async Task Init()
    {
        Teachers = new ObservableCollection<Teacher>(await _school.GetAllTeachers());
        Courses = new ObservableCollection<BaseCourse>(await _school.GetAllCourses());
        Students = new ObservableCollection<Student>(await _school.GetAllStudents());
    }
}