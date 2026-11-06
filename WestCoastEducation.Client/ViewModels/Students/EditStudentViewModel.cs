using System.Threading.Tasks;
using Avalonia.Controls.Notifications;
using WestCoastEducation.Core;
using WestCoastEducation.Core.Models.People;

namespace WestCoastEducation.Client.ViewModels.Students;

public class EditStudentViewModel(School school) : EditPersonViewModelBase(school)
{
    protected override void EditPerson()
    {
        base.EditPerson();
        Task.Run(() => school.UpdateStudent((_person as Student))).Wait();
        ShowSnackbar("Eleven har redigerats.", NotificationType.Success);
        ClearFields();
    }
}