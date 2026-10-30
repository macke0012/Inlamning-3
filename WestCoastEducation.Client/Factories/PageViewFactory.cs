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

public class PageViewFactory(School school)
{
    
    public ViewModelBase LoadPageViewModel(PageViewsEnum page)
    {
        return page switch
        {
            PageViewsEnum.HOME => new HomeViewModel(school),
            PageViewsEnum.STAFF => new StaffViewModel(school), //TODO: Implementera Staff view model
            PageViewsEnum.STUDENT => throw new NotImplementedException(), //TODO: Implementera Student view model
            PageViewsEnum.COURSES => throw new NotImplementedException(),
            _ => throw new InvalidOperationException("Det fanns inte det du ville ha"),
        };
    }

    public ViewModelBase LoadPageViewModel(object target) => target switch
    {
        
        _ => throw new NotSupportedException($"Det finns ingen redigeringsvy för {target.GetType().Name}")
    };
}