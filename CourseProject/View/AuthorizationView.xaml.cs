using CourseProject.ErrorMessage;
using CourseProject.Model;
using CourseProject.ViewModel;
using System;
using System.Collections.Generic;
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
    public partial class AuthorizationView : Window
    {
        AuthorizationViewModel authorizationViewModel;
        public AuthorizationView()
        {
            InitializeComponent();
            authorizationViewModel = new AuthorizationViewModel();
            DataContext = authorizationViewModel;
        }

        private void To_Sign_Up_PreviewMouseDown(object sender, MouseButtonEventArgs e)     // вход пользователя
        {
            Login.Visibility = Visibility.Hidden;
            Password.Visibility = Visibility.Hidden;
            Sign_In.Visibility = Visibility.Hidden;
            Logo.Visibility = Visibility.Hidden;

            NumStudCard.Visibility = Visibility.Visible;
            Name.Visibility = Visibility.Visible;
            Surname.Visibility = Visibility.Visible;
            Phone.Visibility = Visibility.Visible;
            Email.Visibility = Visibility.Visible;
            Reg_Login.Visibility = Visibility.Visible;
            Reg_Password.Visibility = Visibility.Visible;
            Db_Password.Visibility = Visibility.Visible;
            Sign_Up.Visibility = Visibility.Visible;
        }


        private void To_Sign_In_PreviewMouseDown(object sender, MouseButtonEventArgs e)     // регистрация пользователя
        {
                Login.Visibility = Visibility.Visible;
                Password.Visibility = Visibility.Visible;
                Sign_In.Visibility = Visibility.Visible;
                Logo.Visibility = Visibility.Visible;

                NumStudCard.Visibility = Visibility.Hidden;
                Name.Visibility = Visibility.Hidden;
                Surname.Visibility = Visibility.Hidden;
                Phone.Visibility = Visibility.Hidden;
                Email.Visibility = Visibility.Hidden;
                Reg_Login.Visibility = Visibility.Hidden;
                Reg_Password.Visibility = Visibility.Hidden;
                Db_Password.Visibility = Visibility.Hidden;
                Sign_Up.Visibility = Visibility.Hidden;
        }


         public void SignUp()
         {
                if (authorizationViewModel.Registration(Reg_Password.Password, Db_Password.Password))
                {
                    MyMessageBox.Show("Registration completed successfully!", MessageBoxButton.OK);
                }
         }


         public void SignIn()
         {
           User currentUser = authorizationViewModel.СompareDataOfUser(Password.Password);
            

             if (!String.IsNullOrEmpty(Password.Password) && !String.IsNullOrEmpty(Login.Text))
             {
               if (currentUser != null)
                {
                        Hide();
                        User.CurrentUser = currentUser;
                        authorizationViewModel.GetTimeTable();
                        MainWindow mainWindow = new MainWindow();
                        mainWindow.Show();
                    }
                    else MyMessageBox.Show("Check entered data!", MessageBoxButton.OK);
                }
                else MyMessageBox.Show("Enter data!", MessageBoxButton.OK);
            }

        private void Sign_Up_Click(object sender, RoutedEventArgs e)
        {
            SignUp();
        }

        private void Sign_In_Click(object sender, RoutedEventArgs e)
        {
            SignIn();
        }
    }
 }
