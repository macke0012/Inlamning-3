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
        Task.Run(Init);
        SelectedCourses?.CollectionChanged += (_, _) =>
        {
            IsEditable = SelectedCourses.Count == 1;
        };
    }

    public async Task Init()
    {
        List<BaseCourse> courses = await School!.GetAllCourses();
        Console.WriteLine(courses.Count);
        Courses = new ObservableCollection<BaseCourse>(courses);
        courses.ForEach(c => Console.WriteLine(c.Id));
       
    }
    
    [RelayCommand]
    private async Task DeleteCourses(Collection<BaseCourse> courses)
    {
        SetShowPopUp();
        if (Courses.Count == 0) return;
        SetShowSpinner("Tar bort kurser...");
        foreach (var course in courses)
        {
            Console.WriteLine(course.Id);
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
        SetShowSpinner("Genererar Kurser...");
        var gen = new RandomDataGenerator();
        var students = gen.CreateCourses(5);
        students.ForEach(async void (c) =>
        {
            try
            {
                await School!.AddCourse(c);
            }
            catch (Exception e)
            {
                ShowSnackbar(e.Message, NotificationType.Error);
            }
        });
        await Init();
        SetShowSpinner();
        
    }
}