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

        public Teacher VerifyIfTeacherPasswordIsCorrect(string username, string password)
        {
            Teacher matchingTeacher = new List<Teacher>(from teacher in ctx.Teacher
                                                        where teacher.Username == username && teacher.Password == password
                                                        select teacher).SingleOrDefault();
            return matchingTeacher;
        }
    }
}