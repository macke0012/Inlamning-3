using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WestCoastEducation.Client.Views;
using WestCoastEducation.Core;
using WestCoastEducation.Core.Interfaces;
using WestCoastEducation.Core.Models;
using WestCoastEducation.Core.Models.People.Staff;
using WestCoastEducation.Core.Services;

namespace WestCoastEducation.Client.ViewModels;

public partial class StaffViewModel : ViewModelBase
{
    private readonly School _school;
    [ObservableProperty] public partial ObservableCollection<BaseStaff> PermanentStaff { get; set; }
    public IEnumerable<Teacher> Teachers => PermanentStaff.OfType<Teacher>().ToList();
    
    
    public StaffViewModel(School school)
    {
        _school = school;
        
        Task.Run(LoadStaff).Wait();
    }

    public async Task LoadStaff()
    {
        try
        {
            Console.WriteLine("Loading Staff");
            var staff = await _school.GetAllStaff();
            PermanentStaff = new ObservableCollection<BaseStaff>(staff);
            OnPropertyChanged(nameof(PermanentStaff));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            
        }
    }
    
    
    
    
    
    
}