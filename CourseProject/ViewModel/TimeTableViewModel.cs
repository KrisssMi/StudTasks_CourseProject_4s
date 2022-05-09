using CourseProject.DB;
using CourseProject.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CourseProject.ViewModel
{
    class TimeTableViewModel : INotifyPropertyChanged
    {
        User user = User.CurrentUser;
        Student stud = new Student();

        EFTimeTableRepository eFTimeTable = new EFTimeTableRepository();
        EFStudentRepository eFStudent = new EFStudentRepository();

        public List<TimeTable> MondayTimeTable { get; set; }

        public TimeTableViewModel()
        {
            stud = eFStudent.GetStudentById((int)user.idStudent);

           var timetables =  eFTimeTable.GetTimeTable(stud);

            MondayTimeTable = timetables.Where(x => x.Day == 1).ToList();


        }

        private TimeTable selectedTimeTable;        // выбранная запись в таблице

        private ObservableCollection<TimeTable> timeTables = new ObservableCollection<TimeTable>();

        public ObservableCollection<TimeTable> TimeTables
        {
            get { return timeTables; }
            set { timeTables = value; }
        }

        public TimeTable SelectedTimeTable
        {
            get
            {
                return selectedTimeTable;
            }
            set
            {
                if (value != null)
                {
                    selectedTimeTable = value;
                    Save();
                    OnPropertyChanged("SelectedTimeTable");
                }
            }
        }

        public void LoadTT()
        {
                for (int i = 1; i < 7; i++)
                {
                    for (int j = 1; j < 5; j++)
                    {
                        TimeTable t1 = new TimeTable { Day = i, LessonNumber = j, Week = "First", Auditorium = "", LessonName = "", LessonType = "" };
                        TimeTable t2 = new TimeTable { Day = i, LessonNumber = j, Week = "Second", Auditorium = "", LessonName = "", LessonType = "" };
                        eFTimeTable.addTimeTable(t1);
                        eFTimeTable.addTimeTable(t2);
                        TimeTables.Add(t1);
                        TimeTables.Add(t2);
                    }
                }
        }


        public TimeTableViewModel(int id, string week)
        {
            eFTimeTable.Clear();
            TimeTables = eFTimeTable.getTimeTableLocal();
        }

        public IEnumerable<TimeTable> getTimeTable()
        {
            return eFTimeTable.getTimeTable();
        }

        public void Save()
        {
            eFTimeTable.Save();
        }
        

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}