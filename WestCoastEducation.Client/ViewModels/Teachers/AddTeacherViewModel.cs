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

public partial class AddTeacherViewModel : ViewModelBase
{
    
    public AddTeacherViewModel(School school, NavigateBack nb) : base(school, nb)
    {
        PageTitle = "Lägg till Lärare";
    }

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
    private async Task AddTeacher()
    {
        if (School is null) return;
        if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName) ||
            string.IsNullOrWhiteSpace(PhoneNumber))
        {
            ShowSnackbar("Förnamn, Efternamn och Telefon kan inte vara tomma.", NotificationType.Error);
            return;
        }
        SetShowSpinner("Lägger till lärare...");

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

        await School.AddStaff(teacher);
        SetShowSpinner();
        ShowSnackbar($"{FirstName} {LastName} har blivit tillagd som lärare.", NotificationType.Success);
        NavigateBack?.Invoke();
    }
    
}