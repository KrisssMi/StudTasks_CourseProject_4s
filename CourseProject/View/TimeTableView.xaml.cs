using CourseProject.DB;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CourseProject.View
{
    /// <summary>
    /// Логика взаимодействия для TimeTableView.xaml
    /// </summary>
    public partial class TimeTableView : Page
    {
        User user = User.CurrentUser;
        Student stud = new Student();

        TimeTableViewModel timeTableViewModel;
        EFStudentRepository eFStudent = new EFStudentRepository();
        
        public TimeTableView()
        {
            stud = eFStudent.GetStudentById((int)user.idStudent);
            
            InitializeComponent();
            timeTableViewModel = new TimeTableViewModel();
        }


        private void LoadTimeTableIfEmpty()
        {
            timeTableViewModel.LoadTT();
        }
    }
}
