using CourseProject.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseProject.DB
{
    class EFProgressRepository
    {
        private StudTasksEntities context;

        public EFProgressRepository()                  
        {
            context = new StudTasksEntities();                                      // Создаем контекст данных
        }

        public IEnumerable<Progress> getProgress()      
        {
            return context.Progress;
        }

        public IEnumerable<Progress> getProgressById(Student student)               // Получаем прогресс по идентификатору студента
        {
            return context.Progress.Where(p => p.idStudent == student.idStudent);   // Возвращаем прогресс по идентификатору студента
        }

        public void addProgress(Progress progress)                                  // Добавляем прогресс
        {
            context.Progress.Add(progress);
            context.SaveChanges();
        }

        public void SaveProgress()
        {
            context.SaveChanges();
        }

        public void Update(Progress progress)                                        // Обновляем прогресс
        {
            context.Entry(progress).State = EntityState.Modified;                    
            context.SaveChanges();
        }

        public void Remove(Progress progress)                                        // Удаляем прогресс 
        {
            context.Progress.Remove(progress);
            context.SaveChanges();
        }

        public Progress Find(Progress progress)                                     // Поиск прогресса по идентификатору
        {
            return context.Progress.Find(progress.idProgress);
        }

        public Progress GetProgressById(Progress progress)                          // Получаем прогресс по идентификатору
        {
            return context.Progress.FirstOrDefault(p => p.idProgress == progress.idProgress);  
        }

        public void RemoveById(Progress progress)                                   // Удаляем прогресс по идентификатору
        {
            context.Progress.Remove(GetProgressById(progress));
            context.SaveChanges();
        }

        public void RemoveByStudId(Student student)                                 
        {
            foreach (Progress progress in getProgress())
            {
                if (progress.idStudent == student.idStudent)
                {
                    context.Progress.Remove(progress);
                }
            }
            context.SaveChanges();
        }

        public void OrderProgress(Student student)                                  // Сортировка прогресса по идентификатору студента
        {
            context.Progress.Where(p => p.idStudent == student.idStudent).OrderBy(p => p.LessonName).Load();
        }
    }
}