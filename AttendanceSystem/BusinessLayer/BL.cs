using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using Domain;

namespace BusinessLayer
{
    public class BL
    {
        static DL dataLayer = new DL();

        public bool VerifyIfTeacherUsernameExists(string username)
        {
            Teacher matchingTeacher = dataLayer.VerifyIfTeacherUsernameExists(username);
            return matchingTeacher != null;
        }

        public bool VerifyIfTeacherPasswordIsCorrect(string username, string password)
        {
            bool credentialsMatch = dataLayer.VerifyIfTeacherPasswordIsCorrect(username, password);
            return credentialsMatch;
        }

        public int GetTeacherID(string username, string password)
        {
            int loggedInTeacherID = dataLayer.GetTeacherID(username, password);
            return loggedInTeacherID;
        }

        public void AddNewTeacher(string username, string password, string name, string surname, string email)
        {
            Teacher teacher = new Teacher(username, password, name, surname, email);
            dataLayer.AddNewTeacher(teacher);
        }
    }
}