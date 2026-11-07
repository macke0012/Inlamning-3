using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls.Notifications;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WestCoastEducation.Client.Helpers;
using WestCoastEducation.Core;
using WestCoastEducation.Core.Enum;
using WestCoastEducation.Core.Models;
using WestCoastEducation.Core.Models.People.Staff;

namespace WestCoastEducation.Client.ViewModels.Teachers;

public partial class AddTeacherViewModel(School school, NavigateBack nb) : ViewModelBase(school, nb)
{
    [ObservableProperty] public partial string FirstName { get; set; } = "";
    [ObservableProperty] public partial string LastName { get; set; } = "";
    [ObservableProperty] public partial string Email { get; set; } = "";
    [ObservableProperty] public partial string PhoneNumber { get; set; } = "";
    [ObservableProperty] public partial string Ssn { get; set; } = "";
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
        if (string.IsNullOrWhiteSpace(FirstName)|| string.IsNullOrWhiteSpace(LastName) || string.IsNullOrWhiteSpace(PhoneNumber))
        {
            ShowSnackbar("Förnamn, Efternamn och Telefon kan inte vara tomma.", NotificationType.Error);
            return;

        }
        var teacher = new Teacher
        {
            FirstName = FirstName,
            LastName = LastName,
            Ssn = Ssn,
            Email = Email,
            PhoneNumber = PhoneNumber,
            Address = new Address
            {
                Street = Street,
                ZipCode = ZipCode,
                Town = Town,
                Country = Country
            },
            CompetenciesList = SelectedCompetencies.ToList()
            
        };

        Task.Run(() => School.AddStaff(teacher)).Wait();
        ShowSnackbar($"{FirstName} {LastName} har blivit tillagd som lärare.", NotificationType.Success);
       NavigateBack?.Invoke();
    }

    private void ClearFields()
    {
        FirstName = "";
        LastName = "";
        Email = "";
        PhoneNumber = "";
        Ssn = "";
        Street = "";
        ZipCode = "";
        Town = "";
        Country = Country.Sweden;
    }
    
}