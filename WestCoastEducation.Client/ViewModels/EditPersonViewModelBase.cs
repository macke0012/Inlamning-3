using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Controls.Notifications;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WestCoastEducation.Core;
using WestCoastEducation.Core.Enum;
using WestCoastEducation.Core.Models.People;
using WestCoastEducation.Core.Models.People.Staff;

namespace WestCoastEducation.Client.ViewModels;

public abstract partial class EditPersonViewModelBase(School school) : ViewModelBase(school)
{
    protected Person _person;
    [ObservableProperty] public partial string FirstName { get; set; } = "";
    [ObservableProperty] public partial string LastName { get; set; } = "";
    [ObservableProperty] public partial string Email { get; set; } = "";
    [ObservableProperty] public partial string PhoneNumber { get; set; } = "";
    [ObservableProperty] public partial string SSN { get; set; } = "";
    [ObservableProperty] public partial string Street { get; set; } = "";
    [ObservableProperty] public partial string ZipCode { get; set; } = "";
    [ObservableProperty] public partial string Town { get; set; } = "";
    [ObservableProperty] public partial Country Country { get; set; } = Country.Sweden;
    public Country[] Countries { get; } = Enum.GetValues<Country>();
    public virtual void Init(Person person)
    {
        this._person = person;
        
        FirstName = _person.FirstName;
        LastName = _person.LastName;
        Email = _person.Email;
        PhoneNumber = _person.PhoneNumber;
        SSN = _person.SSN;
        Street = _person.Address.Street;
        ZipCode = _person.Address.ZipCode;
        Town = _person.Address.Town;
        Country = _person.Address.Country;
        
    }
    
    [RelayCommand]
    protected virtual void EditPerson()
    {
        if (School is null) return;
        if (string.IsNullOrWhiteSpace(FirstName)|| string.IsNullOrWhiteSpace(LastName) || string.IsNullOrWhiteSpace(PhoneNumber))
        {
            ShowSnackbar("Förnamn, Efternamn och Telefon kan inte vara tomma.", NotificationType.Error);
            return;

        }
        _person.FirstName = FirstName;
        _person.LastName = LastName;
        _person.Email = Email;
        _person.PhoneNumber = PhoneNumber;
        _person.SSN = SSN;
        _person.Address.Street = Street;
        _person.Address.ZipCode = ZipCode;
        _person.Address.Town = Town;
        _person.Address.Country = Country;
        
    }
    
    protected virtual void ClearFields()
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