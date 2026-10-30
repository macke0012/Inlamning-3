using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using WestCoastEducation.Client.Enums;
using WestCoastEducation.Client.Factories;
using WestCoastEducation.Core;
using WestCoastEducation.Core.Models.People;


namespace WestCoastEducation.Client.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly PageViewFactory _pageViewFactory;
    private readonly School _school;


    [ObservableProperty] public partial ViewModelBase CurrentPage { get; set; } = new();


    public MainWindowViewModel(PageViewFactory pageViewFactory, School school)
    {
        _pageViewFactory = pageViewFactory;
        CurrentPage = _pageViewFactory.LoadPageViewModel(PageViewsEnum.HOME);
        _school = school;
        
        WeakReferenceMessenger.Default.RegisterAll(this);
    }

    [RelayCommand]
    private void NavigateToStaffPage() => CurrentPage = _pageViewFactory.LoadPageViewModel(PageViewsEnum.STAFF);

    [RelayCommand]
    private void NavigateToStudentsPage() => CurrentPage = _pageViewFactory.LoadPageViewModel(PageViewsEnum.STUDENT);

    [RelayCommand]
    private void NavigateToCoursesPage() => CurrentPage = _pageViewFactory.LoadPageViewModel(PageViewsEnum.COURSES);

    [RelayCommand]
    private void NavigateToAddPersonPage() => CurrentPage = _pageViewFactory.LoadPageViewModel(PageViewsEnum.ADDPERSON);

    
    //MainWindowViewModel hanterar fortfarande navigationen, men får förfrågan från en annan viewmodel som vill redigera ett object.
    
}