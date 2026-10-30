using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using WestCoastEducation.Core.Interfaces;
using WestCoastEducation.Core.Models.People.Staff;

namespace WestCoastEducation.Client.ViewModels;

public partial class ViewModelBase : ObservableObject
{
    [ObservableProperty] public partial string PageTitle { get; set; } = ""; 
    [ObservableProperty] public partial IStorageObject? SelectedObject { get; set; } = null;
    
    [RelayCommand]
    void SetActiveObject(BaseStaff obj) => SelectedObject = obj;

   
}