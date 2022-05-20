using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CourseProject.DB;
using CourseProject.Model;

namespace CourseProject.ViewModel
{
    internal class TimeTableViewModel : INotifyPropertyChanged
    {
        private readonly EFStudentRepository eFStudent = new EFStudentRepository();
        private readonly EFTimeTableRepository eFTimeTable = new EFTimeTableRepository();

        private TimeTable selectedTimeTable;        // выбранная запись в таблице
        private Student stud = new Student();

        private DelegateCommand updateLine;
        private readonly User user = User.CurrentUser;
        private List<TimeTable> _monday = new List<TimeTable>();
        private List<TimeTable> _tuesday;
        private List<TimeTable> _wednesday;
        private List<TimeTable> _thursday;
        private List<TimeTable> _friday;
        private List<TimeTable> _saturday;
        private ComboBoxItem selectedWeek;

        public TimeTableViewModel()
        {
            eFTimeTable.Clear();
            var temp = eFTimeTable.getTimeTableLocal().Where(x=> x.idStudent==User.CurrentUser.idStudent).ToList();
            TimeTables = new ObservableCollection<TimeTable>(temp);
        }

        public ComboBoxItem SelectedWeek
        {
            get => selectedWeek;
            set
            {
                selectedWeek = value;
                OnPropertyChanged();

                switch (selectedWeek.Content)
                {
                    case "First":
                        Monday = TimeTables.Where(x => x.Week == "First" && x.Day==1).ToList();
                        Tuesday = TimeTables.Where(x => x.Week == "First" && x.Day==2).ToList();
                        Wednesday = TimeTables.Where(x => x.Week == "First" && x.Day==3).ToList();
                        Thursday = TimeTables.Where(x => x.Week == "First" && x.Day==4).ToList();
                        Friday = TimeTables.Where(x => x.Week == "First" && x.Day==5).ToList();
                        Saturday = TimeTables.Where(x => x.Week == "First" && x.Day==6).ToList();
                        break;
                    case "Second": 
                        Monday = TimeTables.Where(x => x.Week == "Second" && x.Day==1).ToList();
                        Tuesday = TimeTables.Where(x => x.Week == "Second" && x.Day==2).ToList();
                        Wednesday = TimeTables.Where(x => x.Week == "Second" && x.Day==3).ToList();
                        Thursday = TimeTables.Where(x => x.Week == "Second" && x.Day==4).ToList();
                        Friday = TimeTables.Where(x => x.Week == "Second" && x.Day==5).ToList();
                        Saturday = TimeTables.Where(x => x.Week == "Second" && x.Day==6).ToList();
                        break;
                }
            }
        }

        public List<TimeTable> Monday
        {
            get => _monday;
            set
            {
                _monday = value;
                OnPropertyChanged();
            }
        }
        public List<TimeTable> Tuesday
        {
            get => _tuesday;
            set
            {
                _tuesday = value;
                OnPropertyChanged();
            }
        }
        public List<TimeTable> Wednesday
        {
            get => _wednesday;
            set
            {
                _wednesday = value;
                OnPropertyChanged();
            }
        }
        public List<TimeTable> Thursday
        {
            get => _thursday;
            set
            {
                _thursday = value;
                OnPropertyChanged();
            }
        }
        public List<TimeTable> Friday
        {
            get => _friday;
            set
            {
               _friday = value;
                OnPropertyChanged();
            }
        }
        public List<TimeTable> Saturday
        {
            get => _saturday;
            set
            {
                _saturday = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<TimeTable> TimeTables { get; set; }

        public TimeTable SelectedTimeTable
        {
            get => selectedTimeTable;
            set
            {
                if (value != null)
                {
                    selectedTimeTable = value;
                    Save();
                    OnPropertyChanged();
                }
            }
        }

        public ICommand UpdateLineCommand
        {
            get
            {
                return updateLine ?? (updateLine = new DelegateCommand(
                    obj =>
                    {
                        try
                        {
                            TimeTable updated;

                            using (StudTasksEntities db = new StudTasksEntities())
                            {
                                updated = db.TimeTable.FirstOrDefault(x =>
                                    x.idTimeTable == selectedTimeTable.idTimeTable);

                                updated.Auditorium = SelectedTimeTable.Auditorium;
                                updated.LessonName = SelectedTimeTable.LessonName;
                                updated.LessonType = SelectedTimeTable.LessonType;
                                db.SaveChanges();
                            }
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.Message);
                        }
                    }
                ));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<TimeTable> GetByWeek(string week, int id)
        {
            TimeTables.Clear();
            foreach (var tt in getTimeTable())
                if (tt.idStudent == id && tt.Week == week)
                    TimeTables.Add(tt);
            return TimeTables;
        }

        public ObservableCollection<TimeTable> GetByWeekAdmin(string week, int idStudent)
        {
            TimeTables.Clear();
            foreach (var tt in getTimeTable())
            {
                stud = eFStudent.GetStudentById(tt.idStudent);
                if (stud.idStudent == tt.idStudent) TimeTables.Add(tt);
            }

            return TimeTables;
        }

        public IEnumerable<TimeTable> getTimeTable()
        {
            return eFTimeTable.getTimeTable();
        }

        public void Save()
        {
            eFTimeTable.Save();
        }

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}