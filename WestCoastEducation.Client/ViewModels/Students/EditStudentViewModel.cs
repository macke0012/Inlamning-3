using System.Threading.Tasks;
using Avalonia.Controls.Notifications;
using WestCoastEducation.Client.Helpers;
using WestCoastEducation.Core;
using WestCoastEducation.Core.Models.People;

namespace WestCoastEducation.Client.ViewModels.Students;

public class EditStudentViewModel(School school, NavigateBack nb) : EditPersonViewModelBase(school, nb)
{
    protected override void EditPerson()
    {
        base.EditPerson();
        Task.Run(() => School?.UpdateStudent(Person as Student)).Wait();
        ShowSnackbar("Eleven har redigerats.", NotificationType.Success);
        NavigateBack?.Invoke();
    }
}