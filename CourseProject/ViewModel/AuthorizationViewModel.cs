using CourseProject.DB;
using CourseProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using CourseProject.ErrorMessage;
using System.Windows.Input;
using System.Windows.Navigation;

namespace CourseProject.ViewModel
{
    class AuthorizationViewModel
    {
        private DelegateCommand updateAuth;
        IEnumerable<User> users;
        IEnumerable<Student> students;

        StudTasksEntities c = new StudTasksEntities();  // подключение к базе данных
        EFUserRepository eFUser;                        // Репозиторий для работы с пользователями
        EFStudentRepository eFStudent;                  // Репозиторий для работы с студентами

        public string Login { get; set; } 

        public int NumStudCard { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Reg_Login { get; set; }

        public string Password { get; set; }

        public string Reg_Password { get; set; }

        public string Db_Password { get; set; }
        


        public AuthorizationViewModel()                 // Конструктор класса
        {
            SingletonView.SingletonUser.getInstance(this);
            eFUser = new EFUserRepository(c);           // Репозиторий для работы с пользователями
            eFStudent = new EFStudentRepository(c);     // Репозиторий для работы с студентами
            users = eFUser.getUsers();                  // Получение всех пользователей
            students = eFStudent.getStudents();         // Получение всех студентов
        }

        public bool Registration(string password1, string password2) // Регистрация
        {
            Reg_Password = password1;
            Db_Password = password2;                     // сравнение с уже зашифрованным паролем 

            if (String.IsNullOrEmpty(Name) && String.IsNullOrEmpty(Surname) && String.IsNullOrEmpty(Reg_Login) &&
                String.IsNullOrEmpty(Reg_Password) && String.IsNullOrEmpty(Db_Password))
            {
                MyMessageBox.Show("Check entered data!", MessageBoxButton.OK);
                return false;
            }

            // регулярное выражение для электронной почты:
            if (!Regex.IsMatch(Email, @"^[-a-z0-9!#$%&'*+/=?^_`{|}~]+(\.[-a-z0-9!#$%&'*+/=?^_`{|}~]+)*@([a-z0-9]([-a-z0-9]{0,61}[a-z0-9])?\.)" +
                        "*(aero|arpa|asia|biz|cat|com|coop|edu|gov|info|int|jobs|mil|mobi|museum|name|net|org|pro|tel|travel|[a-z][a-z])$"))
            {
                MyMessageBox.Show("Incorrect email number input!", MessageBoxButton.OK);
                return false;
            }

            if (!Regex.IsMatch(Phone, @"^(\+375|80)(29|25|44|33|17)([0-9]{3}([0-9]{2}){2})$"))
            {
                MyMessageBox.Show("Incorrect phone number input!", MessageBoxButton.OK);
                return false;
            }

            if (NumStudCard.ToString().Length != 8)
            {
                MyMessageBox.Show("Student card number must contain 8 digits!", MessageBoxButton.OK);
                return false;
            }

            if (!password1.Equals(password2))
            {
                MyMessageBox.Show("Passwords must match!", MessageBoxButton.OK);
                return false;
            }

            User tmp1 = eFUser.GetUserByLogin(Reg_Login);
            Student tmp2 = eFStudent.GetStudentById(NumStudCard);

            if (tmp2 != null)
            {
                MyMessageBox.Show("A user with that student card number is already there!", MessageBoxButton.OK);
                return false;
            }

            if (tmp1 != null)
            {
                MyMessageBox.Show("User with this login already exists!", MessageBoxButton.OK);
                return false;
            }
            Student s = new Student(NumStudCard, Name, Surname, Email, Phone);
            eFStudent.addStudent(s);
            User u = new User(NumStudCard, Reg_Login, User.getHash(Reg_Password));
            eFUser.addUser(u);
            eFUser.Save();
            eFStudent.Save();

            EFTimeTableRepository TTRepository = new EFTimeTableRepository();
            List<TimeTable> timeTables = new List<TimeTable>();
            for (int i = 1; i <= 6; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    for (int k = 1; k <= 4; k++)
                    {
                        timeTables.Add(new TimeTable()
                        {
                            Day = i,
                            Week = j == 0 ? "First" : "Second",
                            idStudent = s.idStudent,
                            LessonNumber = k
                        });
                    }
                }
            }

            foreach (var timeTable in timeTables)
            {
                TTRepository.addTimeTable(timeTable);
            }
            TTRepository.Save();

            return true;
        }

        public User СompareDataOfUser(string password)
        {
                Password = password;
                if (!String.IsNullOrEmpty(Login) && !String.IsNullOrEmpty(Password))
                {
                    User tmp = eFUser.GetUserByLogin(Login);              
                    if (tmp.Password != User.getHash(Password))
                    {
                        return null;
                    }
                
                
                    if (tmp != null)
                    {
                        User.CurrentUser = tmp;
                        CreateRestoringFile(Login, Password);
                        return tmp;
                    }
                    else
                        return null;
                }
                return null;
        }


            public void GetTimeTable()
            {
                EFTimeTableRepository eFTimeTable = new EFTimeTableRepository();
                EFStudentRepository eFStudent = new EFStudentRepository();
                Student student = eFStudent.GetStudentById(User.CurrentUser.idStudent);
                List<TimeTable> TimeTables = eFTimeTable.GetTimeTable(student).ToList();

            
            }

            private void CreateRestoringFile(string login, string password)             // метод сохраняет данные последнего зашедшего в приложение пользователя в xml файл
            {
                XDocument doc = new XDocument(
                    new XElement("Usr",
                        new XAttribute("login", login),
                        new XAttribute("password", password)));
                doc.Save(@"RestUsr.xml");
                Directory.CreateDirectory(@"E:\Course project\Resources");
                doc.Save(@"E:\Course project\Resources\RestUsr.xml");
            }

            public IEnumerable<User> getUsers()
            {
                return eFUser.getUsers();
            }
    }
}