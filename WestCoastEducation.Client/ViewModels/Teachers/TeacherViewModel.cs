using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PleasantUI.Controls;
using WestCoastEducation.Core;
using WestCoastEducation.Core.Models.People.Staff;

namespace WestCoastEducation.Client.ViewModels.Teachers;

public partial class TeacherViewModel : ViewModelBase
{
    
    [ObservableProperty] public partial ObservableCollection<Teacher> Teachers { get; set; } = [];
    [ObservableProperty] public partial Teacher SelectedTeacher { get; set; }
    
    
    
    public TeacherViewModel(School school) : base(school)
    {
        
        Init();
    }

    public async Task Init()
    {
        List<Teacher> teachers = await School.GetAllTeachers();
        Console.WriteLine(teachers.Count);
        Teachers = new ObservableCollection<Teacher>(teachers);
    }

    [RelayCommand]
    private async Task DeleteTeacher(Teacher obj)
    {
        if (obj is null) return;
        Console.WriteLine("Deleting: " + obj.FirstName + " " + obj.LastName);
        await School.DeleteStaff(obj);
        PleasantSnackbar.Show(new PleasantSnackbarOptions("Teacher deleted!"));
        await Init();
    }

    
}