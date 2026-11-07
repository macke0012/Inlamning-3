using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Controls.Notifications;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WestCoastEducation.Core;
using WestCoastEducation.Core.Helpers;
using WestCoastEducation.Core.Models.People;

namespace WestCoastEducation.Client.ViewModels.Students;

public partial class StudentsViewModel : ViewModelBase
{
   
    [ObservableProperty] public partial ObservableCollection<Student> Students { get; set; } = [];

    [ObservableProperty] public partial ObservableCollection<Student> SelectedStudents { get; set; } = [];
    [ObservableProperty] public partial bool IsEditable { get; set; }

    


    public StudentsViewModel(School school) : base(school)
    {
        
        Task.Run(Init);
        //Updatera bool värdet, om bara en elev är vald, kan man redigera den
        SelectedStudents.CollectionChanged += (_, _) =>
        {
            IsEditable = SelectedStudents.Count == 1;
        };
    }

    public async Task Init()
    {
        List<Student> students = await School!.GetAllStudents();
        Console.WriteLine(students.Count);
        Students = new ObservableCollection<Student>(students);
       
    }
    
    [RelayCommand]
    private async Task DeleteStudents(Collection<Student> students)
    {
        SetShowPopUp();
        if (students.Count == 0) return;
        SetShowSpinner("Tar bort elever...");
        foreach (var student in students)
        {
            await School!.DeleteStudent(student);
        }
        SetShowSpinner();
        await Init();
        SelectedStudents.Clear();
        ShowSnackbar("Elever borttagna!",NotificationType.Success);
        

    }

    [RelayCommand]
    private async Task GenerateStudent()
    {
        var gen = new RandomDataGenerator();
        var students = gen.CreateStudents(5);
        students.ForEach(async void (s) =>
        {
            try
            {
                await School!.AddStudent(s);
            }
            catch (Exception e)
            {
                ShowSnackbar(e.Message, NotificationType.Error);
            }
        });
        await Init();
        
    }
    
}