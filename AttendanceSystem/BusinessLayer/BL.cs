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
            Teacher matchingTeacher = dataLayer.VerifyIfTeacherPasswordIsCorrect(username, password);
            return matchingTeacher != null;
        }
    }
}