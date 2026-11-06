using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Avalonia.Controls.Notifications;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WestCoastEducation.Client.Helpers;
using WestCoastEducation.Core;
using WestCoastEducation.Core.Models;
using WestCoastEducation.Core.Models.Course;

namespace WestCoastEducation.Client.ViewModels.Courses;

public partial class EditCourseViewModel : ViewModelBase
{
    private string _id;
    //BaseCourse Props
    [ObservableProperty] public partial string CourseNumber { get; set; }
    [ObservableProperty] public partial string Title { get; set; }
    [ObservableProperty] public partial DateTimeOffset StartDate { get; set; } = DateTimeOffset.Now;
    [ObservableProperty] public partial DateTimeOffset EndDate { get; set; } = DateTimeOffset.Now.AddYears(2);
    [ObservableProperty] public partial CourseType CourseType { get; set; } = CourseType.Classroom;
    public CourseType[] CourseTypes { get; } = Enum.GetValues<CourseType>();
    
    //ClassroomCourse Props
    [ObservableProperty] public partial string Street { get; set; }
    [ObservableProperty] public partial string ZipCode { get; set; }
    [ObservableProperty] public partial string Town { get; set; }
    [ObservableProperty] public partial string LectureHall { get; set; }
    [ObservableProperty] public partial Country Country { get; set; } = Country.Sweden;
    public Country[] Countries { get; } = Enum.GetValues<Country>();
    
    //RemoteCourse Props

    [ObservableProperty] public partial Platform SelectedPlatform { get; set; } = Platform.Teams;
    public Platform[] Platforms { get; } = Enum.GetValues<Platform>();
    [ObservableProperty] public partial string MeetingLink { get; set; }
    
    
    
    public bool IsRemoteCourse => CourseType == CourseType.Remote;
    public bool IsClassroomCourse => CourseType == CourseType.Classroom;

    public EditCourseViewModel(School school, NavigateBack navigateBack) : base(school, navigateBack)
    {
        
    }
    
    //So that the view updates the inputs when the course type changes
    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        if (e.PropertyName == nameof(CourseType))
        {
            OnPropertyChanged(nameof(IsRemoteCourse));
            OnPropertyChanged(nameof(IsClassroomCourse));
        }
    }

    public void Init(BaseCourse course)
    {
        _id = course.Id;
        CourseNumber = course.CourseNumber;
        Title = course.Title;
        StartDate = DateTimeOffset.Parse(course.StartDate.ToString());
        EndDate = DateTimeOffset.Parse(course.EndDate.ToString());
        CourseType = course.CourseType;
        if (course is ClassroomCourse classroomCourse)
        {
            Street = classroomCourse.Location.Street;
            ZipCode = classroomCourse.Location.ZipCode;
            Town = classroomCourse.Location.Town;
            LectureHall = classroomCourse.LectureHall;
            Country = classroomCourse.Location.Country;
            CourseType = CourseType.Classroom;
        }
        else if (course is RemoteCourse remoteCourse)
        {
            SelectedPlatform = remoteCourse.Platform;
            MeetingLink = remoteCourse.MeetingLink;
            CourseType = CourseType.Remote;
        }
    }

    [RelayCommand]
    private async Task AddCourse()
    {
        SetShowSpinner("Lägger till kurs...");
        BaseCourse? courseToAdd = null;
        
        switch (CourseType)
        {
            case CourseType.Classroom:
                courseToAdd = CreateClassroomCourse();
                break;
            case CourseType.Remote:
                courseToAdd = CreateRemoteCourse();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        /*Så att rätt kurs uppdateras. Id:et som genereras när objektet skapas försvinner.
         Detta är så att man ska kunna byta typ på kursen utan att behöva ta bort den först.*/
        courseToAdd.Id = _id;
        await School!.UpdateCourse(courseToAdd);
        SetShowSpinner();
        ShowSnackbar("Kursen blev uppdaterad!", NotificationType.Success);
        NavigateBack?.Invoke();
    }

    private ClassroomCourse CreateClassroomCourse()
    {
        ClassroomCourse classroomCourse = new ClassroomCourse
        {
            Title = Title,
            CourseNumber = CourseNumber,
            StartDate = DateOnly.FromDateTime(StartDate.DateTime),
            EndDate = DateOnly.FromDateTime(EndDate.DateTime),
            LectureHall = LectureHall,
            Location = new Address
            {
                Street = Street,
                ZipCode = ZipCode,
                Town = Town,
                Country = Country
            }

        };
        
        return classroomCourse;
        
    }

    private RemoteCourse CreateRemoteCourse()
    {
        RemoteCourse remoteCourse = new RemoteCourse
        {
            Title = Title,
            CourseNumber = CourseNumber,
            StartDate = DateOnly.FromDateTime(StartDate.DateTime),
            EndDate = DateOnly.FromDateTime(EndDate.DateTime),
            Platform = SelectedPlatform,
            MeetingLink = MeetingLink,
        };
        return remoteCourse;
    }
}