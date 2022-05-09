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

namespace CourseProject.ViewModel
{
    class AuthorizationViewModel
    {
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


        public AuthorizationViewModel() // Конструктор класса
        {

            eFUser = new EFUserRepository(c);           // Репозиторий для работы с пользователями
            eFStudent = new EFStudentRepository(c);     // Репозиторий для работы с студентами
            users = eFUser.getUsers();                  // Получение всех пользователей
            students = eFStudent.getStudents();         // Получение всех студентов
        }


        public bool Registration(string password1, string password2) // Регистрация
        {
            Reg_Password = password1;
            Db_Password = password2; // сравнение с уже зашифрованным паролем 

            //if (String.IsNullOrEmpty(Name) && String.IsNullOrEmpty(Surname) && String.IsNullOrEmpty(Reg_Login) &&
            //    String.IsNullOrEmpty(Reg_Password) && String.IsNullOrEmpty(Db_Password))
            //{
            //    MessageBox.Show("Check entered data!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //    return false;
            //}

            ////if (!Regex.IsMatch(Email, @"^(\+)?((\d{2,3}) ?\d|\d)(([ -]?\d)|( ?(\d{2,3}) ?)){5,12}\d$"))
            ////{
            ////    MessageBox.Show("Incorrect email number input!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            ////    return false;
            ////}

            //if (!Regex.IsMatch(Phone, @"^(\+)?((\d{2,3}) ?\d|\d)(([ -]?\d)|( ?(\d{2,3}) ?)){5,12}\d$"))
            //{
            //    MessageBox.Show("Incorrect phone number input!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //    return false;
            //}

            //if (NumStudCard.ToString().Length != 8)
            //{
            //    MessageBox.Show("Student card number must contain 8 digits!", "Error", MessageBoxButton.OK,
            //        MessageBoxImage.Error);
            //    return false;
            //}

            //if (!password1.Equals(password2))
            //{
            //    MessageBox.Show("Passwords must match!", "Error", MessageBoxButton.OK,
            //        MessageBoxImage.Error);
            //    return false;
            //}

            //User tmp1 = eFUser.GetUserByLogin(Reg_Login);
            //Student tmp2 = eFStudent.GetStudentById(NumStudCard);

            //if (tmp2 != null)
            //{
            //    MessageBox.Show("A user with that student card number is already there!", "Error",
            //        MessageBoxButton.OK, MessageBoxImage.Error);
            //    return false;
            //}

            //if (tmp1 != null)
            //{
            //    MessageBox.Show("User with this login already exists!", "Error",
            //        MessageBoxButton.OK, MessageBoxImage.Error);
            //    return false;
            //}

            //eFStudent.addStudent(new Student(NumStudCard, Name));
            //eFUser.addUser(new User(NumStudCard, Reg_Login, User.getHash(Reg_Password)));
            //eFUser.Save();
            //return true;


            /*  ЭТО ПЕРВОНАЧАЛЬНЫЙ КОД */
             
            if (!String.IsNullOrEmpty(Name) && !String.IsNullOrEmpty(Surname) && !String.IsNullOrEmpty(Reg_Login) &&
                !String.IsNullOrEmpty(Reg_Password) && !String.IsNullOrEmpty(Db_Password))
            {
                
                        if (NumStudCard.ToString().Length == 8)
                        {
                            if (password1.Equals(password2))
                            {
                                User tmp1 = eFUser.GetUserByLogin(Reg_Login);
                                Student tmp2 = eFStudent.GetStudentById(NumStudCard);
                                if (tmp2 == null)
                                {
                                    if (tmp1 == null)
                                    {
                                        Student s = new Student(NumStudCard, Name, Surname, Email, Phone);
                                        eFStudent.addStudent(s);
                                        User u = new User(NumStudCard, Reg_Login, User.getHash(Reg_Password));
                                        eFUser.addUser(u);
                                        return true;
                                    }
                                    else
                                    {
                                MyMessageBox.Show("User with this login already exists!", MessageBoxButton.OK);
                                return false;
                                    }
                                }
                                else
                                {
                            MyMessageBox.Show("A user with that student card number is already there!", MessageBoxButton.OK);
                            return false;
                                }
                            }
                            else
                            {
                        MyMessageBox.Show("Passwords must match!", MessageBoxButton.OK);
                        return false;
                            }
                        }
                        else
                        {
                    MyMessageBox.Show("Student card number must contain 8 digits!", MessageBoxButton.OK);
                    return false;
                        }
                    }
            else
            {
                MyMessageBox.Show("Check entered data!", MessageBoxButton.OK);
                return false;
            }
        }


        public User СompareDataOfUser(string password)
        {
            Password = password;
            if (!String.IsNullOrEmpty(Login) && !String.IsNullOrEmpty(Password))
            {
                User tmp = eFUser.GetUserByLogin(Login);
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