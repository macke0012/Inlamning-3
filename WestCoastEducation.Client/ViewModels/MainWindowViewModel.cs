using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WestCoastEducation.Client.Enums;
using WestCoastEducation.Client.Factories;
using WestCoastEducation.Client.ViewModels.Courses;
using WestCoastEducation.Core;
using WestCoastEducation.Core.Models.Course;
using WestCoastEducation.Core.Models.People;
using WestCoastEducation.Core.Models.People.Staff;


namespace WestCoastEducation.Client.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly PageViewFactory _pageViewFactory;
    

    public delegate void NavigateToBack();


    [ObservableProperty] public partial ViewModelBase CurrentPage { get; set; }
    public string PreviousPage { get; set; } = ""; //Will contain the classname of the previous page
    


    public MainWindowViewModel(PageViewFactory pageViewFactory, School school) : base(school)
    {
        _pageViewFactory = pageViewFactory;
        CurrentPage = _pageViewFactory.LoadPageViewModel(PageViewsEnum.HOME);
        
    }
    
    private void NavigateTo(PageViewsEnum page)
    {   
        PreviousPage = CurrentPage.GetType().Name;
        CurrentPage = _pageViewFactory.LoadPageViewModel(page);
    }

    public void NavigateBackCommand()
    {
        List<string> words = ["Edit", "Add"];
        bool isAMainView = !words.Any(word => PreviousPage.Contains(word)); //Kind of redundant since i'm not going back to any other view than the main ones, but better safe than sorry
        List<string> substrings = Regex.Split(PreviousPage, @"(?<!^)(?=\p{Lu})").ToList(); //Splits the string where there are capitals
        substrings.RemoveAt(substrings.Count - 1);
        for (int i = 0; i < substrings.Count; i++)
        {
            substrings[i] = substrings[i].ToUpper().Trim();
        }

        string enumString = "";

        if (isAMainView)
        {
            enumString = substrings[0];
        }
        else
        {
            enumString += substrings[0] + "_" + substrings[1];
        }
        
        PageViewsEnum page = (PageViewsEnum) Enum.Parse(typeof(PageViewsEnum), enumString);
        NavigateTo(page);
    }
    
    [RelayCommand]
    private void NavigateToHome() => NavigateTo(PageViewsEnum.HOME);


    [RelayCommand]
    private void NavigateToStudentsPage() => NavigateTo(PageViewsEnum.STUDENTS);

    [RelayCommand]

    private void NavigateToCoursesPage() => NavigateTo(PageViewsEnum.COURSES);
    
    [RelayCommand]

    private void NavigateToTeachersPage() => NavigateTo(PageViewsEnum.TEACHERS);
    
    

    [RelayCommand]
    private void NavigateToAddTeacher() => NavigateTo(PageViewsEnum.ADD_TEACHER);
    [RelayCommand]
    private void NavigateToAddStudent() => NavigateTo(PageViewsEnum.ADD_STUDENT);
    [RelayCommand]
    private void NavigateToAddCourse() => NavigateTo(PageViewsEnum.ADD_COURSE);
    
    
    [RelayCommand]
    private void NavigateToEditTeacher(Teacher teacher)
    {
        NavigateTo(PageViewsEnum.EDIT_TEACHER);
        (CurrentPage as EditPersonViewModelBase)?.Init(teacher);

    }

    [RelayCommand]
    private void NavigateToEditStudent(Student student)
    {
        NavigateTo(PageViewsEnum.EDIT_STUDENT);
        (CurrentPage as EditPersonViewModelBase)?.Init(student);
    }

    [RelayCommand]
    private void NavigateToEditCourse(BaseCourse course)
    {
        NavigateTo(PageViewsEnum.EDIT_COURSE);
        (CurrentPage as EditCourseViewModel)?.Init(course);
    }
    
  
}