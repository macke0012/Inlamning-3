using System.ComponentModel;
using Avalonia.Controls.Notifications;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using PleasantUI.Controls;
using WestCoastEducation.Core;
using WestCoastEducation.Core.Interfaces;
using WestCoastEducation.Core.Models.People.Staff;

namespace WestCoastEducation.Client.ViewModels;

public partial class ViewModelBase : ObservableObject
{
    [ObservableProperty] public partial string PageTitle { get; set; } = "";
    
    [ObservableProperty] public partial bool ShowPopUp { get; set; } = false;
    [ObservableProperty] public partial string PopUpPrompt { get; set; } = "";
    
    [ObservableProperty] public partial bool ShowSpinner { get; set; } = false;
    [ObservableProperty] public partial string SpinnerText { get; set; } = "";
    protected readonly School? School;
    
    public ViewModelBase(School school)
    {
        School = school;
    }

    public ViewModelBase(){}

    //För att kunna visa en snackbar i all views
    protected void ShowSnackbar(string content, NotificationType messageType) =>
        PleasantSnackbar.Show(new PleasantSnackbarOptions(content)
        {
            NotificationType = messageType
        });

    /// <summary>
    /// Toggles the visibility of the pop-up and updates the prompt message.
    /// </summary>
    /// <param name="content">The message to display in the pop-up. Defaults to an empty string.</param>
    [RelayCommand]
    protected void SetShowPopUp(string content = "")
    {
        PopUpPrompt = content;
        ShowPopUp = !ShowPopUp;
    }

    /// <summary>
    /// Toggles the visibility of the loading spinner and updates the spinner message.
    /// </summary>
    /// <param name="content">The message to display with the spinner. Defaults to an empty string.</param>
    [RelayCommand]
    protected void SetShowSpinner(string content = "")
    {
        SpinnerText = content;
        ShowSpinner = !ShowSpinner;
    }
}