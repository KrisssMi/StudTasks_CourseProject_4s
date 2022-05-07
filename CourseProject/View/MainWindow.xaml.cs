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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Exit_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var x = MessageBox.Show("Do you really want to leave?", "Exit", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);
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
            MessageBox.Show("created by Kristina Minevich\n2022, Minsk", "Support", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
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
                    Source = new Uri("pack://application:,,/Themes/Dark.xaml")
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
                    Source = new Uri("pack://application:,,/Themes/Light.xaml")
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show("error: " + ex.Message);
            }
        }
    }
}
