using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls.Notifications;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PleasantUI.Controls;
using WestCoastEducation.Core;
using WestCoastEducation.Core.Enum;
using WestCoastEducation.Core.Models;
using WestCoastEducation.Core.Models.People;
using WestCoastEducation.Core.Models.People.Staff;

namespace WestCoastEducation.Client.ViewModels.Teachers;

public partial class EditTeacherViewModel(School school) : EditPersonViewModelBase(school)
{

    [ObservableProperty] public partial ObservableCollection<Competencies> SelectedCompetencies { get; set; } = [];
    
    public Competencies[] Competencies { get; } = Enum.GetValues<Competencies>();

    public override void Init(Person person)
    {
        base.Init(person);
        (_person as Teacher).CompetenciesList.ForEach(competency => SelectedCompetencies.Add(competency));
    }

    //Hanterar också kompetens listan.
    protected override void EditPerson()
    {
        base.EditPerson();
        (_person as Teacher).CompetenciesList = SelectedCompetencies.ToList();
        Task.Run(() => School.UpdateTeacher(_person as Teacher)).Wait();
        ShowSnackbar("Läraren har redigerats", NotificationType.Success);
        ClearFields();
        
    }

    override protected void ClearFields()
    {
        base.ClearFields();
        SelectedCompetencies.Clear();
    }
}