using CourseProject.DB;
using CourseProject.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CourseProject.View
{
    /// <summary>
    /// Логика взаимодействия для ProfileView.xaml
    /// </summary>
    public partial class ProfileView : Page
    {
        User user = User.CurrentUser;
        Student stud = new Student();

        EFStudentRepository eFStudent = new EFStudentRepository();

        public ProfileView()
        {
            user = User.CurrentUser;
            InitializeComponent();
            stud = eFStudent.GetStudentById(user.idStudent);
            if (stud != null)
            {
                tbName.Text = stud.Name;
                tbSurname.Text = stud.Surname;
                tbNumber.Text = stud.idStudent.ToString();
                tbPhone.Text = stud.Phone;
                tbEmail.Text = stud.Email;

                // редактирование профиля
                //if (user.idStudent == stud.idStudent)
                //{
                //    btnEdit.Visibility = Visibility.Visible;
                //    btnSave.Visibility = Visibility.Hidden;
                //    btnCancel.Visibility = Visibility.Hidden;
                //}
                //else
                //{
                //    btnEdit.Visibility = Visibility.Hidden;
                //    btnSave.Visibility = Visibility.Hidden;
                //    btnCancel.Visibility = Visibility.Hidden;
                //}
            }
        }
    }
}
