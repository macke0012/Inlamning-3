using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls.Notifications;
using CommunityToolkit.Mvvm.ComponentModel;
using WestCoastEducation.Client.Helpers;
using WestCoastEducation.Core;
using WestCoastEducation.Core.Enum;
using WestCoastEducation.Core.Models.People;
using WestCoastEducation.Core.Models.People.Staff;

namespace WestCoastEducation.Client.ViewModels.Teachers;

public partial class EditTeacherViewModel(School school, NavigateBack nb) : EditPersonViewModelBase(school, nb)
{

    [ObservableProperty] public partial ObservableCollection<Competencies> SelectedCompetencies { get; set; } = [];
    
    public Competencies[] Competencies { get; } = Enum.GetValues<Competencies>();

    public override void Init(Person person)
    {
        base.Init(person);
        (Person as Teacher)?.CompetenciesList.ForEach(competency => SelectedCompetencies.Add(competency));
    }

    //Hanterar också kompetens listan.
    protected override void EditPerson()
    {
        base.EditPerson();
        (Person as Teacher)?.CompetenciesList = SelectedCompetencies.ToList();
        Task.Run(() => School!.UpdateTeacher(Person as Teacher)).Wait();
        ShowSnackbar("Läraren har redigerats", NotificationType.Success);
        NavigateBack?.Invoke();
        
    }

    protected override void ClearFields()
    {
        base.ClearFields();
        SelectedCompetencies.Clear();
    }
}