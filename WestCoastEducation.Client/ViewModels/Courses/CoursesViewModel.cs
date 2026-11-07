using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Controls.Notifications;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WestCoastEducation.Core;
using WestCoastEducation.Core.Helpers;
using WestCoastEducation.Core.Models.Course;

namespace WestCoastEducation.Client.ViewModels.Courses;

public partial class CoursesViewModel : ViewModelBase
{
    [ObservableProperty] public partial ObservableCollection<BaseCourse> Courses { get; set; } = [];
    [ObservableProperty] public partial ObservableCollection<BaseCourse> SelectedCourses { get; set; } = [];
    [ObservableProperty] public partial bool IsEditable { get; set; } = false;
    public CoursesViewModel(School school) : base(school)
    {
        PageTitle = "Kurser";
        Task.Run(Init);
        SelectedCourses?.CollectionChanged += (_, _) =>
        {
            IsEditable = SelectedCourses.Count == 1;
        };
    }

    public async Task Init()
    {
        List<BaseCourse> courses = await School!.GetAllCourses();
        Courses = new ObservableCollection<BaseCourse>(courses);
        
       
    }
    
    [RelayCommand]
    private async Task DeleteCourses(Collection<BaseCourse> courses)
    {
        SetShowPopUp();
        if (courses.Count == 0) return;
        SetShowSpinner("Tar bort kurser...");
        foreach (var course in courses)
        {
            await School!.DeleteCourse(course);
        }
        SetShowSpinner();
        await Init();
        SelectedCourses.Clear();
        ShowSnackbar("Kurser borttagna!",NotificationType.Success);
        

    }

    [RelayCommand]
    private async Task GenerateCourses()
    {
        SetShowSpinner("Genererar kurser...");
        var gen = new RandomDataGenerator();
        var courses = gen.CreateCourses(5);
        foreach (var course in courses)
        {
            await School!.AddCourse(course);
        }
        await Init();
        SetShowSpinner();
        
    }
}