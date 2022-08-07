using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public partial class Course
    {
        public Course(string course)
        {
        }
    }

    public partial class Teacher
    {
        public Teacher(string username, string password, string name, string surname, string email)
        {
            this.Username = username;
            this.Password = password;
            this.Name = name;
            this.Surname = surname;
            this.Email = email;
        }
    }
}
