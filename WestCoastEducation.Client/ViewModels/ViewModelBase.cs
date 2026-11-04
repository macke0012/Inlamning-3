using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using WestCoastEducation.Core;
using WestCoastEducation.Core.Interfaces;
using WestCoastEducation.Core.Models.People.Staff;

namespace WestCoastEducation.Client.ViewModels;

public partial class ViewModelBase : ObservableObject
{
    [ObservableProperty] public partial string PageTitle { get; set; } = ""; 
    protected readonly School? School;
    
    public ViewModelBase(School school)
    {
        School = school;
    }

    public ViewModelBase(){}

}