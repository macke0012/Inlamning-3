using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PleasantUI.Controls;
using WestCoastEducation.Core;
using WestCoastEducation.Core.Enum;
using WestCoastEducation.Core.Models;
using WestCoastEducation.Core.Models.People.Staff;

namespace WestCoastEducation.Client.ViewModels.Teachers;

public partial class EditTeacherViewModel(School school) : ViewModelBase(school)
{
    Teacher _teacher;
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

    public void Init(Teacher teacher)
    {
        _teacher = teacher;
        
        FirstName = teacher.FirstName;
        LastName = teacher.LastName;
        Email = teacher.Email;
        PhoneNumber = teacher.PhoneNumber;
        SSN = teacher.SSN;
        Street = teacher.Address.Street;
        ZipCode = teacher.Address.ZipCode;
        Town = teacher.Address.Town;
        Country = teacher.Address.Country;
        SelectedCompetencies = new ObservableCollection<Competencies>(teacher.CompetenciesList ?? []);
    }

    [RelayCommand]
    private void EditTeacher()
    {
        if (School is null) return;
        _teacher.FirstName = FirstName;
        _teacher.LastName = LastName;
        _teacher.Email = Email;
        _teacher.PhoneNumber = PhoneNumber;
        _teacher.SSN = SSN;
        _teacher.Address.Street = Street;
        _teacher.Address.ZipCode = ZipCode;
        _teacher.Address.Town = Town;
        _teacher.Address.Country = Country;
        _teacher.CompetenciesList = SelectedCompetencies.ToList();
        

        Task.Run(() => School.UpdateTeacher(_teacher)).Wait();
        PleasantSnackbar.Show(new PleasantSnackbarOptions("Teacher Edited!"));
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