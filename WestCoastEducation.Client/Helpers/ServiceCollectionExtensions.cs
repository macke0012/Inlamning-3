using System;
using Microsoft.Extensions.DependencyInjection;
using WestCoastEducation.Client.Factories;
using WestCoastEducation.Client.ViewModels;
using WestCoastEducation.Core.Interfaces;
using WestCoastEducation.Core.Services;
using WestCoastEducation.Core;
using WestCoastEducation.Core.Repositories;
using WestCoastEducation.Client.Enums;
using WestCoastEducation.Client.ViewModels.Courses;
using WestCoastEducation.Client.ViewModels.Students;
using WestCoastEducation.Client.ViewModels.Teachers;


namespace WestCoastEducation.Client.Helpers;

public static class ServiceCollectionExtensions
{
    // Syftet är att skapa en metod som lägger till tjänser services
    // i ServiceCollection objektet. Som vi fått via Microsoft.Extensions.DependencyInjection...
    public static void AddServices(this IServiceCollection collection)
    {
        //PageViewFactory
        collection.AddSingleton<PageViewFactory>();
        
        //Main Viewmodels
        collection.AddSingleton<MainWindowViewModel>();
        collection.AddTransient<HomeViewModel>();
        
        //View Data Models
        collection.AddTransient<CoursesViewModel>();
        collection.AddTransient<TeachersViewModel>();
        collection.AddTransient<StudentsViewModel>();
        
        //Add data models
        collection.AddTransient<AddTeacherViewModel>();
        collection.AddTransient<AddStudentViewModel>();
        collection.AddTransient<AddCourseViewModel>();
        
        //Edit data models
        collection.AddTransient<EditTeacherViewModel>();
        collection.AddTransient<EditStudentViewModel>();
        collection.AddTransient<EditCourseViewModel>();
        
        //Repository
        collection.AddSingleton<IStorageProvider, DataBaseClient>();
        
        //Navigation Helper
        collection.AddSingleton<NavigateBack>(c => 
            () => c.GetRequiredService<MainWindowViewModel>().NavigateBackCommand());
        
        //Backend Services
        collection.AddSingleton<StaffService>();
        collection.AddSingleton<StudentService>();
        collection.AddSingleton<CourseService>();
        collection.AddSingleton<School>();

        collection.AddSingleton<Func<PageViewsEnum, ViewModelBase>>(c => name => name switch
        {
            PageViewsEnum.HOME => c.GetRequiredService<HomeViewModel>(),
            
            PageViewsEnum.COURSES => c.GetRequiredService<CoursesViewModel>(),
            PageViewsEnum.TEACHERS => c.GetRequiredService<TeachersViewModel>(),
            PageViewsEnum.STUDENTS => c.GetRequiredService<StudentsViewModel>(),
            
            PageViewsEnum.ADD_TEACHER => c.GetRequiredService<AddTeacherViewModel>(),
            PageViewsEnum.ADD_STUDENT => c.GetRequiredService<AddStudentViewModel>(),
            PageViewsEnum.ADD_COURSE => c.GetRequiredService<AddCourseViewModel>(),
            
            PageViewsEnum.EDIT_TEACHER => c.GetRequiredService<EditTeacherViewModel>(),
            PageViewsEnum.EDIT_STUDENT => c.GetRequiredService<EditStudentViewModel>(),
            PageViewsEnum.EDIT_COURSE => c.GetRequiredService<EditCourseViewModel>(),
            _ => throw new InvalidOperationException()
        });

    }
}
