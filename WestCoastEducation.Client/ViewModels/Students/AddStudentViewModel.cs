using System;
using System.Threading.Tasks;
using Avalonia.Controls.Notifications;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WestCoastEducation.Client.Helpers;
using WestCoastEducation.Core;
using WestCoastEducation.Core.Enum;
using WestCoastEducation.Core.Models;
using WestCoastEducation.Core.Models.People;

namespace WestCoastEducation.Client.ViewModels.Students;

public partial class AddStudentViewModel(School school, NavigateBack navigateBack) : ViewModelBase(school, navigateBack)
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
    public Country[] Countries { get; } = Enum.GetValues<Country>();

   
    
    [RelayCommand]
    private void AddStudent()
    {
        if(School is null) return;
        if (string.IsNullOrWhiteSpace(FirstName)|| string.IsNullOrWhiteSpace(LastName) || string.IsNullOrWhiteSpace(PhoneNumber))
        {
            ShowSnackbar("Förnamn, Efternamn och Telefon kan inte vara tomma.", NotificationType.Error);
            return;

        }
        var student = new Student
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
            
            
        };

        Task.Run(() => School.AddStudent(student)).Wait();
        ShowSnackbar($"{FirstName} {LastName} har blivit tillagd som student.", NotificationType.Success);
        NavigateBack?.Invoke();
    }
}