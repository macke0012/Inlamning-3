using MongoDB.Driver;
using WestCoastEducation.Core.Interfaces;
using WestCoastEducation.Core.Models.People;


namespace WestCoastEducation.Core.Services;

public class StudentService : BaseService<Student>
{
    public List<Student> Students { get; private set; } = [];
    public StudentService(IStorageProvider db) : base(db)
    {
        Students = Task.Run(db.GetAll<Student>).Result;
        if (_cursor is null) return;

        _cursor.ForEachAsync(change =>
        {
            if (change.OperationType == ChangeStreamOperationType.Delete)
            {
                Students.RemoveAll(s => s.Id == change.FullDocument.Id);
                return;
            }
            Students.RemoveAll(s => s.Id == change.FullDocument.Id);
            Students.Add(change.FullDocument);
        });
        
    }

    

}
