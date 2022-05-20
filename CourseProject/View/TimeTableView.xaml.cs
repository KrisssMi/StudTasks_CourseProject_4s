using CourseProject.DB;
using CourseProject.Model;
using CourseProject.ViewModel;
using System.Windows.Controls;
namespace CourseProject.View
{
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
            DataContext = timeTableViewModel;
        }
    }
}
