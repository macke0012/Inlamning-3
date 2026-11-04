using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using WestCoastEducation.Core;
using WestCoastEducation.Core.Models.People.Staff;

namespace WestCoastEducation.Client.ViewModels.Teachers;

public partial class TeacherViewModel : ViewModelBase
{
    
    [ObservableProperty] public partial ObservableCollection<Teacher> Teachers { get; set; } = [];
    
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
    
}