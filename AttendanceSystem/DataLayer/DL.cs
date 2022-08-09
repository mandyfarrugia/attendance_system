using System;
using System.Data.Entity.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace DataLayer
{
    public class DL
    {
        public static AttendanceSystemEntities ctx = new AttendanceSystemEntities();

        public Teacher VerifyIfTeacherUsernameExists(string username)
        {
            Teacher matchingTeacher = new List<Teacher>(from teacher in ctx.Teacher
                                                        where teacher.Username == username
                                                        select teacher).SingleOrDefault();
            return matchingTeacher;
        }

        public bool VerifyIfTeacherPasswordIsCorrect(string username, string password)
        {
            Teacher matchingTeacher = new List<Teacher>(from teacher in ctx.Teacher
                                                        where teacher.Username == username && teacher.Password == password
                                                        select teacher).FirstOrDefault();
            if(matchingTeacher != null)
                return matchingTeacher.TeacherID != 0;
            return false;
        }

        public Teacher VerifyIfTeacherEmailExists(string email)
        {
            Teacher matchingTeacher = new List<Teacher>(from teacher in ctx.Teacher
                                                        where teacher.Email == email
                                                        select teacher).SingleOrDefault();
            return matchingTeacher;
        }

        public Student VerifyIfStudentEmailExists(string email)
        {
            Student matchingStudent = new List<Student>(from student in ctx.Student
                                                        where student.Email == email
                                                        select student).SingleOrDefault();
            return matchingStudent;
        }

        public int GetTeacherID(string username, string password)
        {
            Teacher matchingTeacher = new List<Teacher>(from teacher in ctx.Teacher
                                                        where teacher.Username == username && teacher.Password == password
                                                        select teacher).FirstOrDefault();
            return matchingTeacher.TeacherID;
        }

        public Teacher GetTeacherById(int teacherID)
        {
            Teacher matchingTeacher = new List<Teacher>(from teacher in ctx.Teacher
                                                      where teacher.TeacherID == teacherID
                                                      select teacher).FirstOrDefault();
            return matchingTeacher;
        }

        public string EditTeacher(int teacherID, string newUsername, string newPassword, string newName, string newSurname, string newEmail)
        {
            try
            {
                int editCount = 0;
                string result = string.Empty;
                Teacher teacherToEdit = this.GetTeacherById(teacherID);
                if (teacherToEdit != null)
                {
                    string fullName = $"{teacherToEdit.Name} {teacherToEdit.Surname}";
                    if (!newUsername.Equals(string.Empty))
                    {
                        string oldUsername = teacherToEdit.Username;
                        result += $"The username {oldUsername} has been changed to {newUsername}.\n";
                        teacherToEdit.Username = newUsername;
                        editCount++;
                    }
                    if (!newPassword.Equals(string.Empty))
                    {
                        string oldPassword = teacherToEdit.Password;
                        result += $"The password {oldPassword} has been changed to {newPassword}.\n";
                        teacherToEdit.Password = newPassword;
                        editCount++;
                    }
                    if (!newName.Equals(string.Empty))
                    {
                        string oldName = teacherToEdit.Name;
                        result += $"The name {oldName} has been changed to {newName}.\n";
                        teacherToEdit.Name = newName;
                        editCount++;
                    }
                    if (!newSurname.Equals(string.Empty))
                    {
                        string oldSurname = teacherToEdit.Surname;
                        result += $"The surname {oldSurname} has been changed to {newSurname}.\n";
                        teacherToEdit.Surname = newSurname;
                        editCount++;
                    }
                    if (!newEmail.Equals(string.Empty))
                    {
                        string oldEmail = teacherToEdit.Email;
                        result += $"The email {oldEmail} has been changed to {newEmail}.\n";
                        teacherToEdit.Email = newEmail;
                        editCount++;
                    }

                    if(editCount != 0)
                        result += $"A total of {((editCount == 1) ? $"{editCount} change has" : $"{editCount} changes have")} been inflicted on {fullName}.";

                    ctx.SaveChanges();
                }
                else
                    result = "Could not find any teacher with the corresponding ID!";

                return result;
            }
            catch(DbUpdateException ex)
            {
                return ex.Message;
            }
        }

        public string EditStudent(int studentID, string newName, string newSurname, string newEmail, int newGroupID)
        {
            try
            {
                int editCount = 0;
                string result = string.Empty;
                Student studentToEdit = this.VerifyIfStudentExists(studentID);
                if (studentToEdit != null)
                {
                    string fullName = $"{studentToEdit.Name} {studentToEdit.Surname}";
                    if (!newName.Equals(string.Empty))
                    {
                        string oldName = studentToEdit.Name;
                        result += $"The name {oldName} has been changed to {newName}.\n";
                        studentToEdit.Name = newName;
                        editCount++;
                    }
                    if (!newSurname.Equals(string.Empty))
                    {
                        string oldSurname = studentToEdit.Surname;
                        result += $"The surname {oldSurname} has been changed to {newSurname}.\n";
                        studentToEdit.Surname = newSurname;
                        editCount++;
                    }
                    if (!newEmail.Equals(string.Empty))
                    {
                        string oldEmail = studentToEdit.Email;
                        result += $"The email {oldEmail} has been changed to {newEmail}.\n";
                        studentToEdit.Email = newEmail;
                        editCount++;
                    }
                    studentToEdit.GroupID = newGroupID;

                    if (editCount != 0)
                        result += $"A total of {((editCount == 1) ? $"{editCount} change has" : $"{editCount} changes have")} been inflicted on {fullName}.";

                    ctx.SaveChanges();
                }
                else
                    result = "Could not find any teacher with the corresponding ID!";

                return result;
            }
            catch (DbUpdateException ex)
            {
                return ex.Message;
            }
        }

        public List<Course> GetAllCourses()
        {
            List<Course> allCourses = new List<Course>(from course in ctx.Course
                                                       select course);
            return allCourses;
        }

        public List<Group> GetAllGroups()
        {
            List<Group> allGroups = new List<Group>(from grp in ctx.Group
                                                    select grp);
            return allGroups;
        }

        public List<Student> GetAllStudents()
        {
            List<Student> allStudents = new List<Student>(from student in ctx.Student
                                                          select student);
            return allStudents;
        }

        public Student VerifyIfStudentExists(int studentID)
        {
            Student matchingStudent = new List<Student>(from student in ctx.Student
                                                        where student.StudentID == studentID
                                                        select student).FirstOrDefault();
            return matchingStudent;
        }

        public Group VerifyIfGroupExists(int groupID)
        {
            Group matchingGroup = new List<Group>(from grp in ctx.Group
                                                  where grp.GroupID == groupID
                                                  select grp).FirstOrDefault();
            return matchingGroup;
        }

        public Course VerifyIfCourseExistsById(int courseID)
        {
            Course matchingCourse = new List<Course>(from course in ctx.Course
                                                     where course.CourseID == courseID
                                                     select course).FirstOrDefault();
            return matchingCourse;
        }

        public Course GetCourseByTitle(string courseTitle)
        {
            Course matchingCourse = new List<Course>(from course in ctx.Course
                                                     where course.CourseTitle == courseTitle
                                                     select course).FirstOrDefault();
            return matchingCourse;
        }

        public void AddNewGroup(Group group)
        {
            ctx.Group.Add(group);
            ctx.SaveChanges();
        }

        public void AddNewCourse(Course course)
        {
            ctx.Course.Add(course);
            ctx.SaveChanges();
        }

        public void AddNewTeacher(Teacher teacher)
        {
            ctx.Teacher.Add(teacher);
            ctx.SaveChanges();
        }

        public void AddNewStudent(Student student)
        {
            ctx.Student.Add(student);
            ctx.SaveChanges();
        }
    }
}