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
                {
                    result = "Could not find any teacher with the corresponding ID!";
                }
                return result;
            }
            catch(DbUpdateException ex)
            {
                return ex.Message;
            }
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
    }
}