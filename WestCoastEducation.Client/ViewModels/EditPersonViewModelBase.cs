using System;
using Avalonia.Controls.Notifications;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WestCoastEducation.Client.Helpers;
using WestCoastEducation.Core;
using WestCoastEducation.Core.Enum;
using WestCoastEducation.Core.Models.People;

namespace WestCoastEducation.Client.ViewModels;

public abstract partial class EditPersonViewModelBase(School school, NavigateBack nb) : ViewModelBase(school, nb)
{
    protected Person? Person;
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
    public virtual void Init(Person person)
    {
        this.Person = person;
        
        FirstName = Person.FirstName;
        LastName = Person.LastName;
        Email = Person.Email;
        PhoneNumber = Person.PhoneNumber;
        Ssn = Person.Ssn;
        Street = Person.Address.Street;
        ZipCode = Person.Address.ZipCode;
        Town = Person.Address.Town;
        Country = Person.Address.Country;
        
    }
    
    [RelayCommand]
    protected virtual void EditPerson()
    {
        if (School is null || Person is null) return;
        if (string.IsNullOrWhiteSpace(FirstName)|| string.IsNullOrWhiteSpace(LastName) || string.IsNullOrWhiteSpace(PhoneNumber))
        {
            ShowSnackbar("Förnamn, Efternamn och Telefon kan inte vara tomma.", NotificationType.Error);
            return;

        }
        Person.FirstName = FirstName;
        Person.LastName = LastName;
        Person.Email = Email;
        Person.PhoneNumber = PhoneNumber;
        Person.Ssn = Ssn;
        Person.Address.Street = Street;
        Person.Address.ZipCode = ZipCode;
        Person.Address.Town = Town;
        Person.Address.Country = Country;
        
    }
    
    protected virtual void ClearFields()
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