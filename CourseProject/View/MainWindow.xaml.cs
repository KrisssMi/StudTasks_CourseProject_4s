using CourseProject.DB;
using CourseProject.ErrorMessage;
using CourseProject.Model;
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
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        User User = User.CurrentUser;
        Student stud = new Student();
        EFStudentRepository eFStudent = new EFStudentRepository();

        public MainWindow()
        {
            InitializeComponent();

            stud = eFStudent.GetStudentById((int)User.idStudent);

            if (stud.isAdmin == true)
            {
                Group_list.Visibility = Visibility.Visible;
            }
            else Group_list.Visibility = Visibility.Hidden;

            DataContext = this;
            //Choose_Theme_Unchecked(this, new RoutedEventArgs());
            AddRemindersToAutorun();
        }

        private void AddRemindersToAutorun()
        {
            // открываем нужную ветку в реестре   
            // @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run\"  

            Microsoft.Win32.RegistryKey Key =
                Microsoft.Win32.Registry.LocalMachine.OpenSubKey(
                "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run\\", true);

            string path = System.IO.Path.GetFullPath(@"StudTasksReminder\StudTasksReminder.exe");
            //добавляем первый параметр - название ключа  
            // Второй параметр - это путь к   
            // исполняемому файлу программы.  
            Key.SetValue("NtOrg", "\"" + path + "\"");
            Key.Close();
        }
        

        private void Exit_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var x = MyMessageBox.Show("Do you really want to leave?", MessageBoxButton.YesNo);
            if (x.Equals(MessageBoxResult.Yes))
            {
                MainWindow.GetWindow(this).Close();
                
                // завершение программы:
                Environment.Exit(0);
            }
            else if (x.Equals(MessageBoxResult.No))
            {
                e.Handled = true;
            }
        }

        private void Support_PreviewMouseDown(object sender, RoutedEventArgs e)
        {
            MyMessageBox.Show("created by Kristina Minevich\n2022, Minsk", MessageBoxButton.OK);
        }


        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Visible;
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
        }

        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
            ButtonCloseMenu.Visibility = Visibility.Visible;
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();                 // окно можно перемещать
        }


        // переход между окнами

        private void Task_List_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            MainPage.Content = new TaskListView();
        }

        private void TimeTable_List_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            MainPage.Content = new TimeTableView();
        }

        private void Progress_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            MainPage.Content = new ProgressView();
        }

        private void Profile_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            MainPage.Content = new ProfileView();
        }

        private void Group_list_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            MainPage.Content = new StudentsListView();
        }


        

        private void Choose_Theme_Checked(object sender, RoutedEventArgs e)         // тёмная тема
        {
            try
            {
                this.Resources = new ResourceDictionary()
                {
                    Source = new Uri("pack://application:,,/Resources/Themes/DarkTheme.xaml")
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show("error: " + ex.Message);
            }
        }

        private void Choose_Theme_Unchecked(object sender, RoutedEventArgs e)       // светлая тема
        {
            try
            {
                this.Resources = new ResourceDictionary()
                {
                    Source = new Uri("pack://application:,,/Resources/Themes/LightTheme.xaml")
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show("error: " + ex.Message);
            }
        }
    }
}
