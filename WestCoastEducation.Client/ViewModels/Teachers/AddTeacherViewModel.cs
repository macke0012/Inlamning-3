using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WestCoastEducation.Core;
using WestCoastEducation.Core.Enum;
using WestCoastEducation.Core.Models;
using WestCoastEducation.Core.Models.People.Staff;

namespace WestCoastEducation.Client.ViewModels.Teachers;

public partial class AddTeacherViewModel(School school) : ViewModelBase(school)
{
    [ObservableProperty] public partial string FirstName { get; set; } = "";
    [ObservableProperty] public partial string LastName { get; set; } = "";
    [ObservableProperty] public partial string Email { get; set; } = "";
    [ObservableProperty] public partial string PhoneNumber { get; set; } = "";
    [ObservableProperty] public partial string SSN { get; set; } = "";
    [ObservableProperty] public partial string Street { get; set; } = "";
    [ObservableProperty] public partial string ZipCode { get; set; } = "";
    [ObservableProperty] public partial string Town { get; set; } = "";
    [ObservableProperty] public partial Country Country { get; set; } = Country.Sweden;
    [ObservableProperty] public partial ObservableCollection<Competencies> SelectedCompetencies { get; set; } = [];
    public Country[] Countries { get; } = Enum.GetValues<Country>();
    public Competencies[] Competencies { get; } = Enum.GetValues<Competencies>();
    



    [RelayCommand]
    private void AddTeacher()
    {
        if(School is null) return;
        var teacher = new Teacher
        {
            FirstName = FirstName,
            LastName = LastName,
            SSN = SSN,
            Email = Email,
            PhoneNumber = PhoneNumber,
            Address = new Address
            {
                Street = Street,
                ZipCode = ZipCode,
                Town = Town,
                Country = Country
            }
        };

        Task.Run(() => School.AddStaff(teacher)).Wait();
        ClearFields();
    }

    private void ClearFields()
    {
        FirstName = "";
        LastName = "";
        Email = "";
        PhoneNumber = "";
        SSN = "";
        Street = "";
        ZipCode = "";
        Town = "";
        Country = Country.Sweden;
    }
    
}