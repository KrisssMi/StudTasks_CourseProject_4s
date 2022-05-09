using CourseProject.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseProject.DB
{
    class EFTimeTableRepository
    {
        private StudTasksEntities context;

        public EFTimeTableRepository()
        {
            context = new StudTasksEntities();
        }

        public IEnumerable<TimeTable> getTimeTable()            // получение списка таблицы
        {
            return context.TimeTable.Local;
        }

        public ObservableCollection<TimeTable> getTimeTableLocal()
        {
            return context.TimeTable.Local;
        }

        public void addTimeTable(TimeTable timeTable)
        {
            context.TimeTable.Add(timeTable);
            context.SaveChanges();
        }

        public void Clear()
        {
            context.TimeTable.Local.Clear();
        }

        public void Update(TimeTable timeTable)
        {
            context.Entry(timeTable).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void UpdateTT(TimeTable timeTable)
        {
            TimeTable tmp = context.TimeTable.FirstOrDefault(x => x.idTimeTable == timeTable.idTimeTable);
            tmp.LessonName = timeTable.LessonName;
            tmp.Auditorium = timeTable.Auditorium;
            tmp.LessonType = timeTable.LessonType;
            tmp.Week = timeTable.Week;
            Save();
        }

        public void Remove(TimeTable timeTable)
        {
            context.TimeTable.Remove(timeTable);
            context.SaveChanges();
        }



        public IEnumerable<TimeTable> GetTimeTable(Student student)     // получение предметов по студенту
        {

            // сделать
            return context.TimeTable.Where(x => x.idStudent == student.idStudent);
        }

        public IEnumerable<string> GetSubjects(Student student)     // получение предметов по студенту
        {
            return context.TimeTable.Where(p =>  (p.LessonName != "")).OrderBy(p => p.LessonName).Select(p => p.LessonName).Distinct().ToList();
        }
    }
}
