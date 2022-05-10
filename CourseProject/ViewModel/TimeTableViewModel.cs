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
        public List<TimeTable> TuesdayTimeTable { get; set; }
        public List<TimeTable> WednesdayTimeTable { get; set; }
        public List<TimeTable> ThursdayTimeTable { get; set; }
        public List<TimeTable> FridayTimeTable { get; set; }
        public List<TimeTable> SaturdayTimeTable { get; set; }


        public TimeTableViewModel()
        {
           stud = eFStudent.GetStudentById((int)user.idStudent);
           var timetables =  eFTimeTable.GetTimeTable(stud);
            
           MondayTimeTable = timetables.Where(x => x.Day == 1).ToList();        // Получаем все занятия на понедельник
           TuesdayTimeTable = timetables.Where(x => x.Day == 2).ToList();       // Получаем все занятия на вторник
           WednesdayTimeTable = timetables.Where(x => x.Day == 3).ToList();     // Получаем все занятия на среду
           ThursdayTimeTable = timetables.Where(x => x.Day == 4).ToList();      // Получаем все занятия на четверг
           FridayTimeTable = timetables.Where(x => x.Day == 5).ToList();        // Получаем все занятия на пятницу
           SaturdayTimeTable = timetables.Where(x => x.Day == 6).ToList();      // Получаем все занятия на субботу
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

        public ObservableCollection<TimeTable> GetByWeek(string week, int id)
        {
            TimeTables.Clear();
            foreach (TimeTable tt in getTimeTable())
            {
                if (tt.idStudent == id && tt.Week == week)
                {
                    TimeTables.Add(tt);
                }
            }
            return TimeTables;
        }

        public ObservableCollection<TimeTable> GetByWeekAdmin(string week, int idStudent)
        {
            TimeTables.Clear();
            foreach (TimeTable tt in getTimeTable())
            {
                stud = eFStudent.GetStudentById((int)tt.idStudent);
                if (stud.idStudent == tt.idStudent)
                {
                    TimeTables.Add(tt);
                }
            }
            return TimeTables;
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

        public string CurrentWeek()
        {
            int dayStart = FirstSeptDay().DayOfYear - (int)FirstSeptDay().DayOfWeek + 1;//Номер понедельника в году в неделе с первым сентября
            if ((DaysSinceStart(dayStart) / 7) % 2 == 0)
            {
                return "First";
            }
            else return "Second";
        }


        private int DaysSinceStart(int dayStart)
        {
            if (DateTime.Now.Month > 8)
                return DateTime.Now.DayOfYear - dayStart;
            else
                if (DateTime.IsLeapYear(FirstSeptDay().Year))
                return 366 - dayStart + DateTime.Now.DayOfYear;
            else
                return 365 - dayStart + DateTime.Now.DayOfYear;
        }

        private DateTime FirstSeptDay()
        {
            DateTime d = DateTime.Now;
            DateTime ds;
            if (d.Month < 9)
                ds = new DateTime(DateTime.Now.Year - 1, 9, 1);
            else
                ds = new DateTime(DateTime.Now.Year, 9, 1);
            return ds;
        }        

        
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        
    }
}