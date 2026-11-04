using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using WestCoastEducation.Core;
using WestCoastEducation.Core.Models.People;
using WestCoastEducation.Core.Models.People.Staff;

namespace WestCoastEducation.Client.ViewModels.Students;

public partial class StudentViewModel : ViewModelBase
{
    private readonly School _school;
    [ObservableProperty] public partial ObservableCollection<Student> Students { get; set; } = [];
    
    public StudentViewModel(School school)
    {
        _school = school;
        Init();
    }

    public async Task Init()
    {
        List<Student> students = await _school.GetAllStudents();
        Console.WriteLine(students.Count);
        Students = new ObservableCollection<Student>(students);
    }
    
}