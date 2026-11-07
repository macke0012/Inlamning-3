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

public partial class EditTeacherViewModel : EditPersonViewModelBase
{
   
    public EditTeacherViewModel(School school, NavigateBack nb) : base(school, nb)
    {
        PageTitle = "Redigera Lärare";
    }

    [ObservableProperty] public partial ObservableCollection<Competencies> SelectedCompetencies { get; set; } = [];
    
    public Competencies[] Competencies { get; } = Enum.GetValues<Competencies>();

    public override void Init(Person person)
    {
        
        base.Init(person);
        (Person as Teacher)?.CompetenciesList.ForEach(competency => SelectedCompetencies.Add(competency));
    }

    //Hanterar också kompetens listan.
    protected override async Task EditPerson()
    {
        SetShowSpinner("Redigerar lärare...");
        await base.EditPerson();
        (Person as Teacher)?.CompetenciesList = SelectedCompetencies.ToList();
        await School!.UpdateTeacher(Person as Teacher);
        SetShowSpinner();
        ShowSnackbar("Läraren har redigerats", NotificationType.Success);
        NavigateBack?.Invoke();
        
    }

    protected override void ClearFields()
    {
        base.ClearFields();
        SelectedCompetencies.Clear();
    }
}