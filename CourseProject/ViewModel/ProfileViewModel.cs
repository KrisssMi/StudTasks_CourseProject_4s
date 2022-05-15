using CourseProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseProject.ViewModel
{
    class ProfileViewModel : BaseViewModel
    {
        //User user = User.CurrentUser;
        Student stud = new Student();

        public User LoggerUser { get; set; }
        
        public ProfileViewModel()
        {
            // вывести имя текущего пользователя:
            LoggerUser = User.CurrentUser;
        }
    }
}
