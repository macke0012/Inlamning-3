using System;
using System.ComponentModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WestCoastEducation.Core.Models;
using WestCoastEducation.Core.Models.Course;

namespace WestCoastEducation.Client.ViewModels.Courses;

public partial class AddCourseViewModel : ViewModelBase
{
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

    [RelayCommand]
    private async Task AddCourse()
    {
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
        School.AddCourse(courseToAdd);
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