using System;
using WestCoastEducation.Client.Enums;
using WestCoastEducation.Client.ViewModels;
using WestCoastEducation.Core;
using WestCoastEducation.Core.Interfaces;
using WestCoastEducation.Core.Models.Course;
using WestCoastEducation.Core.Models.People;
using WestCoastEducation.Core.Models.People.Staff;
using WestCoastEducation.Core.Services;


namespace WestCoastEducation.Client.Factories;

public class PageViewFactory
{
    
     private readonly Func<PageViewsEnum, ViewModelBase> _pageFactory;
     
    public PageViewFactory(Func<PageViewsEnum, ViewModelBase> pageFactory)
    {
        
        _pageFactory = pageFactory;
    }

    public ViewModelBase LoadPageViewModel(PageViewsEnum page)
    {
        return _pageFactory.Invoke(page);
    }
}