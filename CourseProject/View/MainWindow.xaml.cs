using CourseProject.DB;
using CourseProject.ErrorMessage;
using CourseProject.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    public partial class MainWindow : Window
    {
        User User = User.CurrentUser;
        Student stud = new Student();
        EFMessageRepository eFMessage = new EFMessageRepository();
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
            if (stud.IsAdmin == false)
            {
                MainPage.Content = new StartPage();
            }
            else
            {
                MainPage.Content = new AdminPage();
            }

            Count_Message.Badge = eFMessage.CountMessages();
            Choose_Theme_Unchecked(this, new RoutedEventArgs());


            
            App.LanguageChanged += LanguageChanged;

            
            Language.Items.Add(new CultureInfo("en-US"));
            Language.Items.Add(new CultureInfo("ru-RU"));
        }

        private void LanguageChanged(Object sender, EventArgs e)
        {
            CultureInfo currLang = App.Language;

            //Отмечаем нужный пункт смены языка как выбранный язык
            foreach (CultureInfo i in Language.Items)
            {
                CultureInfo ci = i;
                ci.Equals(currLang);
            }
        }

        private void Language_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CultureInfo lang = Language.SelectedItem as CultureInfo;
            if (lang != null)
            {
                App.Language = lang;
            }
        }

        private void Language_Loaded(object sender, RoutedEventArgs e)
        {     
        }


        private void Exit_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var x = MyMessageBox.Show("Do you really want to leave?", MessageBoxButton.YesNo);
            if (x.Equals(MessageBoxResult.Yes))
            {
                AuthorizationView authorizationView = new AuthorizationView();
                authorizationView.Show();
                MainWindow.GetWindow(this).Close();
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

        private void Message_List_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            MainPage.Content = new MessageView();
            Count_Message.Badge = eFMessage.CountMessages();
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


        private void Roll_Up_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }
    }
}