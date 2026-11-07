using System;
using WestCoastEducation.Client.Enums;
using WestCoastEducation.Client.ViewModels;



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