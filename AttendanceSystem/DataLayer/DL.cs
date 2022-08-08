using System;
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