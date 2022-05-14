using CourseProject.DB;
using StudTasksReminder.Model;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace StudTasksReminder
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        EFTimeTableRepository eFTimeTable = new EFTimeTableRepository();
        EFUserRepository eFUser = new EFUserRepository();
        EFTaskRepository eFTask = new EFTaskRepository();

        List<TimeTable> Lab = new List<TimeTable>();
        List<Model.Task> Tasks = new List<Model.Task>();

        User user = User.CurrentUser;
        bool IsICanWork;
        string week;

        public MainWindow()
        {
            InitializeComponent();
            SetWindowPosition();
        }

        private void SetWindowPosition()
        {
            var primaryMonitorArea = SystemParameters.WorkArea;
            Left = primaryMonitorArea.Right - Width;
            Top = primaryMonitorArea.Bottom - Height - 10;
        }


        private void SetTasks()
        {
            DateTime d = DateTime.Now.Date;
            Tasks = eFTask.GetTaskByStudId(user);
            DeleteLastTasks();
            try
            {
                if (Tasks.Count != 0)
                {
                    LabelTask1.Text = Tasks[0].Date + "   " + Tasks[0].Title + " " + Tasks[0].Content;
                    LabelTask2.Text = Tasks[1].Date + "   " + Tasks[1].Title + " " + Tasks[1].Content;
                    LabelTask3.Text = Tasks[2].Date + "   " + Tasks[2].Title + " " + Tasks[2].Content;
                }
                else LabelTask2.Text = "U have no tasks scheduled for the coming days :)";
            }
            catch (NullReferenceException) { }
            catch (ArgumentNullException) { }
            catch (ArgumentOutOfRangeException) { }
        }


        private void DeleteLastTasks()
        {
            try
            {
                if (Convert.ToDateTime(Tasks.First().DueDate) < DateTime.Now.Date)
                {
                    Tasks.Remove(Tasks.First());
                    DeleteLastTasks();
                }
            }
            catch (ArgumentNullException) { }
            catch (InvalidOperationException) { }
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GetUser();
            LetItWork();
            SetNotifications();
            SetTasks();
        }

        private void SetNotifications()
        {
            Hello.Content = "Hello, " + user.Student.Name;
            SetNotificationLabs(Lab.Count);
            SetNotificationReccomendations(Lab.Count);
        }

        private void SetNotificationLabs(int labsNumber)
        {
            string lab = "";
            foreach (TimeTable t in Lab)
            {
                if (t != Lab.Last())
                    lab += t.LessonName + ",";
                else lab += t.LessonName + ";";
            }
            switch (labsNumber)
            {
                case 0:
                    Labs.Text = "Tomorrow there is no labs, u can relax ))";
                    break;
                case 1:
                    Labs.Text = "Tomorrow " + Lab.Count + " lab: " + " " + lab;
                    break;
                default:
                    Labs.Text = "Tomorrow " + Lab.Count + " labs: " + " " + lab;
                    break;

            }
        }


        private void SetNotificationReccomendations(int labsNumber)
        {
            switch (labsNumber)
            {
                case 0:
                    LabelReccomendations.Content = "if u don't rest, then there will be no time";
                    break;
                case 1:
                    LabelReccomendations.Content = "Higly recommend making it!)";
                    break;
                default:
                    LabelReccomendations.Content = "Higly recommend making them!)";
                    break;
            }
        }

        private void LetItWork()
        {
            //week = CurrentWeek();
            LoadLabsOnTomorrow();
            IsICanWork = IsThereLabsOnTomorrow();

        }

        private bool IsThereLabsOnTomorrow()
        {
            if (Lab.Count() == 0)
                return false;
            else return true;
        }

        private void LoadLabsOnTomorrow()
        {
            int tomorrow = (int)DateTime.Now.DayOfWeek + 1;
            try
            {
                Lab = eFTimeTable.GetLabsOnTomorrow(week, user, tomorrow);
            }
            catch (System.Data.DataException) { }
            finally { }
        }

        private void GetUser()
        {
            try
            {
                XDocument doc = XDocument.Load(@"Resources\RestUsr.xml");
                user = eFUser.GetUserByLogin(doc.Root.Attribute("login").Value);
                if (user.Login == null) GetUser();
            }
            catch (NullReferenceException) { IsICanWork = false; }
            catch (System.Data.DataException) { IsICanWork = false; }
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        
    }
}
