using System.Threading.Tasks;
using Avalonia.Controls.Notifications;
using WestCoastEducation.Client.Helpers;
using WestCoastEducation.Core;
using WestCoastEducation.Core.Models.People;

namespace WestCoastEducation.Client.ViewModels.Students;

public class EditStudentViewModel : EditPersonViewModelBase
{
    public EditStudentViewModel(School school, NavigateBack nb) : base(school, nb)
    {
        PageTitle = "Redigera elev";
    }

    protected override async Task EditPerson()
    {
        SetShowSpinner("Redigerar eleven...");
        await base.EditPerson();
        await School!.UpdateStudent(Person as Student);
        SetShowSpinner();
        ShowSnackbar("Eleven har redigerats.", NotificationType.Success);
        NavigateBack?.Invoke();
    }
}