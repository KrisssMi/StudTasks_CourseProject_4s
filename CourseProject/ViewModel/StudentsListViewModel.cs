using CourseProject.DB;
using CourseProject.ErrorMessage;
using CourseProject.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CourseProject.ViewModel
{
    class StudentsListViewModel : BaseViewModel
    {
        EFUserRepository eFUser = new EFUserRepository();
        EFProgressRepository eFProgress = new EFProgressRepository();
        EFTaskRepository eFTask = new EFTaskRepository();
        EFStudentRepository eFStudent = new EFStudentRepository();
        EFTimeTableRepository eFTimeTable = new EFTimeTableRepository();
        EFMessageRepository eFMessage = new EFMessageRepository();


        public IEnumerable<Student> getStudents()
        {
            return eFStudent.getStudents();
        }

        public IEnumerable<Student> getStudentsNoAdmin()
        {
            return eFStudent.getStudentsNoAdmin();
        }

        public void UpdateAll()
        {
            eFStudent.UpdateAll();
        }

        public void RemoveStudent(Student student)
        {
            eFStudent.Remove(student);
        }

        public void RemoveAllInfAboutStudent(Student student)
        {
            try
            {
            eFTimeTable.RemoveByStudId(student);
            eFProgress.RemoveByStudId(student);
            eFTask.RemoveByStudId(student);
            eFMessage.RemoveByStudId(student);
            eFUser.RemoveUserById(student);
            eFStudent.RemoveStudentById(student);
            tmpStudents.Remove(student);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(ex.Message, MessageBoxButton.OK);
            }  
        }

        User User = User.CurrentUser;

        ObservableCollection<Student> tmpStudents = new ObservableCollection<Student>();

        public ObservableCollection<Student> Students
        {
            get { return tmpStudents; }
        }

        Student selectedItem;

        public Student SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                OnPropertyChanged("SelectedItem");
            }
        }

        public StudentsListViewModel()
        {
            var students = this.getStudents();
            tmpStudents.Clear();
            foreach (Student a in students)
                tmpStudents.Add(a);
            SingletonView.SingletonAdmin.getInstance(null).StudentsListViewModel = this;
        }
    }
}
