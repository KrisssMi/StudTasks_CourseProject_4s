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
using System.Windows;
using System.Windows.Input;

namespace CourseProject.ViewModel
{
    class TimeTableViewModel : INotifyPropertyChanged
    {
        User user = User.CurrentUser;
        Student stud = new Student();

        private List<TimeTable> allMonday = new List<TimeTable>();
        private List<TimeTable> allTuesday = new List<TimeTable>();
        private List<TimeTable> allWednesday = new List<TimeTable>();
        private List<TimeTable> allThursday = new List<TimeTable>();
        private List<TimeTable> allFriday = new List<TimeTable>();
        private List<TimeTable> allSaturday = new List<TimeTable>();

        EFTimeTableRepository eFTimeTable = new EFTimeTableRepository();
        EFStudentRepository eFStudent = new EFStudentRepository();

        public static List<TimeTable> MondayTimeTable { get; set; } = new List<TimeTable>();
        public static List<TimeTable> TuesdayTimeTable { get; set; } = new List<TimeTable>();
        public static List<TimeTable> WednesdayTimeTable { get; set; } = new List<TimeTable>();
        public static List<TimeTable> ThursdayTimeTable { get; set; } = new List<TimeTable>();
        public static List<TimeTable> FridayTimeTable { get; set; } = new List<TimeTable>();
        public static List<TimeTable> SaturdayTimeTable { get; set; } = new List<TimeTable>();

        public List<TimeTable> Monday
        {
            get
            {
                return MondayTimeTable;
            }
            set
            {
                MondayTimeTable = value;
                OnPropertyChanged("Monday");
            }
        }

        public List<TimeTable> Tuesday
        {
            get
            {
                return TuesdayTimeTable;
            }
            set
            {
                MondayTimeTable = value;
                OnPropertyChanged("Tuesday");
            }
        }

        public List<TimeTable> Wednesday
        {
            get
            {
                return WednesdayTimeTable;
            }
            set
            {
                MondayTimeTable = value;
                OnPropertyChanged("Wednesday");
            }
        }

        public List<TimeTable> Thursday
        {
            get
            {
                return ThursdayTimeTable;
            }
            set
            {
                MondayTimeTable = value;
                OnPropertyChanged("Thursday");
            }
        }

        public List<TimeTable> Friday
        {
            get
            {
                return FridayTimeTable;
            }
            set
            {
                MondayTimeTable = value;
                OnPropertyChanged("Friday");
            }
        }

        public List<TimeTable> Saturday
        {
            get
            {
                return SaturdayTimeTable;
            }
            set
            {
                MondayTimeTable = value;
                OnPropertyChanged("Saturday");
            }
        }

    public TimeTableViewModel()
        {
            stud = eFStudent.GetStudentById((int)user.idStudent);
            var timetables = eFTimeTable.GetTimeTable(stud);
            //UpdateLineCommand = new DelegateCommand(UpdateLine);

            // allMonday = timetables.Where(x => x.Day == 1).ToList();        // Получаем все занятия на понедельник
            // allTuesday = timetables.Where(x => x.Day == 2).ToList();       // Получаем все занятия на вторник
            // allWednesday = timetables.Where(x => x.Day == 3).ToList();     // Получаем все занятия на среду
            // allThursday = timetables.Where(x => x.Day == 4).ToList();      // Получаем все занятия на четверг
            // allFriday = timetables.Where(x => x.Day == 5).ToList();        // Получаем все занятия на пятницу
            // allSaturday = timetables.Where(x => x.Day == 6).ToList();      // Получаем все занятия на субботу

            // MondayTimeTable = timetables.Where(x => x.Day == 1).ToList();        // Получаем все занятия на понедельник
            //TuesdayTimeTable = timetables.Where(x => x.Day == 2).ToList();       // Получаем все занятия на вторник
            //WednesdayTimeTable = timetables.Where(x => x.Day == 3).ToList();     // Получаем все занятия на среду
            //ThursdayTimeTable = timetables.Where(x => x.Day == 4).ToList();      // Получаем все занятия на четверг
            //FridayTimeTable = timetables.Where(x => x.Day == 5).ToList();        // Получаем все занятия на пятницу
            //SaturdayTimeTable = timetables.Where(x => x.Day == 6).ToList();      // Получаем все занятия на субботу

            allMonday = MondayTimeTable;
            allTuesday = TuesdayTimeTable;
            allWednesday = WednesdayTimeTable;
            allThursday = ThursdayTimeTable;
            allFriday = FridayTimeTable;
            allSaturday = SaturdayTimeTable;

            MondayTimeTable = allMonday.Where(x => x.Week == "First").ToList();
            TuesdayTimeTable = allTuesday.Where(x => x.Week == "First").ToList();
            WednesdayTimeTable = allWednesday.Where(x => x.Week == "First").ToList();
            ThursdayTimeTable = allThursday.Where(x => x.Week == "First").ToList();
            FridayTimeTable = allFriday.Where(x => x.Week == "First").ToList();
            SaturdayTimeTable = allSaturday.Where(x => x.Week == "First").ToList();
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

        
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        private DelegateCommand updateLine;
        public ICommand UpdateLineCommand
        {
            get
            {
                return updateLine ?? (updateLine = new DelegateCommand(
                    (obj) =>
                    {
                        try
                        {
                            TimeTable updated;

                            using (StudTasksEntities db = new StudTasksEntities())
                            {

                                var DeletedTT = new ObservableCollection<TimeTable>(db.TimeTable).Where(x => x.Day == SelectedTimeTable.Day);
                                db.TimeTable.RemoveRange(new ObservableCollection<TimeTable>(db.TimeTable).Where(x => x.Day == SelectedTimeTable.Day && x.idStudent == SelectedTimeTable.idStudent));
                                updated = DeletedTT.Where(x => x.idTimeTable == SelectedTimeTable.idTimeTable).First();
                                updated.idTimeTable = SelectedTimeTable.idTimeTable;
                                updated.Auditorium = SelectedTimeTable.Auditorium;
                                updated.LessonName = SelectedTimeTable.LessonName;
                                updated.LessonNumber = SelectedTimeTable.LessonNumber;
                                updated.LessonType = SelectedTimeTable.LessonType;
                                db.TimeTable.AddRange(DeletedTT);
                                db.SaveChanges();


                            }

                            //TimeTable updated = SaveTT.GetTimeTable(stud).ToList().FirstOrDefault(t => t.idTimeTable == SelectedTimeTable.idTimeTable);



                            //SaveTT.UpdateTT(updated);
                            //SaveTT.Save();

                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.Message);
                        }
                        }
                    ));
            }
        }
        private void UpdateLine(object obj)
        {
            
        }
    }
}