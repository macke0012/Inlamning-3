using WestCoastEducation.Core.Models.Course;
using WestCoastEducation.Core.Models.People;
using WestCoastEducation.Core.Models.People.Staff;
using WestCoastEducation.Core.Services;

namespace WestCoastEducation.Core;

public class School(CourseService courseService, StudentService studentService, StaffService staffService)
{
    public async Task AddCourse(BaseCourse baseCourse) => await courseService.Create(baseCourse);
    public async Task AddStaff(BaseStaff staff) => await staffService.Create(staff);
    public async Task AddStudent(Student student) => await studentService.Create(student);
    
    
    public async Task<List<BaseCourse>> GetAllCourses() => await courseService.GetAll();
    public async Task<List<Teacher>> GetAllTeachers() => (await staffService.GetAll()).OfType<Teacher>().ToList();
    public async Task<List<Student>> GetAllStudents() => await studentService.GetAll();
    public async Task<List<BaseStaff>> GetAllStaff() => await staffService.GetAll();

    public async Task<Teacher> GetTeacher(string id) => (Teacher)await staffService.GetSingle(id);
    public async Task<Student> GetStudent(string id) => await studentService.GetSingle(id);
    public async Task<EducationLeader> GetEducationLeader(string id) => (EducationLeader)await staffService.GetSingle(id);
    public async Task<Administrator> GetAdministrator(string id) => (Administrator)await staffService.GetSingle(id);
    public async Task<RemoteCourse> GetRemoteCourse(string id) => (RemoteCourse)await courseService.GetSingle(id);
    public async Task<ClassroomCourse> GetClassroomCourse(string id) => (ClassroomCourse)await courseService.GetSingle(id);

    public async Task UpdateStudent(Student? newStudentData) => await studentService.Update(newStudentData!);
    public async Task UpdateTeacher(Teacher? newTeacherData) => await staffService.Update(newTeacherData!);
    public async Task UpdateEducationLeader(EducationLeader newLeaderData) => await staffService.Update(newLeaderData);
    public async Task UpdateAdministrator(Administrator newAdminData) => await staffService.Update(newAdminData);
    public async Task UpdateCourse(BaseCourse newBaseCourseData) => await courseService.Update(newBaseCourseData);
    
    public async Task DeleteStaff(BaseStaff staff) => await staffService.Delete(staff);
    public async Task DeleteStudent(Student student) => await studentService.Delete(student);
    public async Task DeleteCourse(BaseCourse course) => await courseService.Delete(course);
    

    public void AssignCourseToTeacher(Teacher teacher, TeacherLeadBaseCourse baseCourse)
    {
        baseCourse.Teacher = teacher;
        teacher.AddCourse(baseCourse);
        Task.Run(() => courseService.Update(baseCourse)).Wait();
        Task.Run(() => staffService.Update(teacher)).Wait();
    }

    public void EnrollStudent(Student student, BaseCourse baseCourse)
    {
        student.Enroll(baseCourse);
        Task.Run(() => studentService.Update(student));
    }

    public void AssignTeacherToLeader(EducationLeader leader, Teacher teacher)
    {
        leader.AssignTeacher(teacher);
        Task.Run(() => staffService.Update(leader));
    }
}
