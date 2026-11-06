using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WestCoastEducation.Core;
using WestCoastEducation.Core.Models.Course;

namespace WestCoastEducation.Client.ViewModels.Courses;

public partial class CourseViewModel : ViewModelBase
{
    [ObservableProperty] public partial ObservableCollection<BaseCourse> Courses { get; set; }
    [ObservableProperty] public partial BaseCourse SelectedCourse { get; set; }

    public CourseViewModel(School school) : base(school)
    {
        Init();
    }

    private void Init()
    {
        var courses = Task.Run(School.GetAllCourses).Result;
        Courses = new ObservableCollection<BaseCourse>(courses);
    }

    [RelayCommand]
    private void DeleteCourse(BaseCourse obj)
    {
        Task.Run(() => School.DeleteCourse(obj)).Wait();
    }
}