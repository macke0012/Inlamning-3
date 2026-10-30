using WestCoastEducation.Core;

namespace WestCoastEducation.Client.ViewModels;

public class HomeViewModel : ViewModelBase
{
    private readonly School _school;
    public HomeViewModel(School school)
    {
        _school = school;
    }
}