using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Controls.Notifications;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WestCoastEducation.Core;
using WestCoastEducation.Core.Helpers;
using WestCoastEducation.Core.Models.People.Staff;

namespace WestCoastEducation.Client.ViewModels.Teachers;

public partial class TeachersViewModel : ViewModelBase
{
    
    [ObservableProperty] public partial ObservableCollection<Teacher> Teachers { get; set; } = [];
    [ObservableProperty] public partial ObservableCollection<Teacher> SelectedTeachers { get; set; } = [];
    [ObservableProperty] public partial bool IsEditable { get; set; } = false;
    
    
    public TeachersViewModel(School school) : base(school)
    {
        PageTitle = "Lärare";
        Task.Run(Init);
        SelectedTeachers.CollectionChanged += (_, _) =>
        {
            IsEditable = SelectedTeachers.Count == 1;
        };
    }

    public async Task Init()
    {
        List<Teacher> teachers = await School!.GetAllTeachers();
        Teachers = new ObservableCollection<Teacher>(teachers);
    }

    [RelayCommand]
    private async Task DeleteTeachers(Collection<Teacher> teachers)
    {
        SetShowPopUp();
        if (teachers.Count == 0) return;
        SetShowSpinner("Tar bort lärare...");
        foreach (var teacher in teachers)
        {
            await School!.DeleteStaff(teacher);
        }
        SetShowSpinner();
        await Init();
        SelectedTeachers.Clear();
        ShowSnackbar("Lärare borttagna!",NotificationType.Success);
        

    }
    
    [RelayCommand]
    private async Task GenerateTeacher()
    {
        SetShowSpinner("Genererar Lärare...");
        var gen = new RandomDataGenerator();
        var teachers = gen.CreateTeachers(5);
        foreach (var teacher in teachers)
        {
            await School!.AddStaff(teacher);
        }
        await Init();
        SetShowSpinner();
        
    }

    
}