using CourseProject.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseProject.DB
{
    class EFTaskRepository
    {

        private StudTasksEntities context;

        public EFTaskRepository()
        {
            context = new StudTasksEntities();
        }

        public IEnumerable<Model.Task> getTasks()
        {
            context.Task.Distinct().OrderByDescending(p => p.DueDate).Load();
            return context.Task;
        }

        public void addTask(Model.Task task)
        {
            context.Task.Add(task);
            context.SaveChanges();
        }

        public void SaveTask()
        {
            context.SaveChanges();
        }

        public void Update(Model.Task task)
        {
            context.Entry(task).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void UpdateAll()
        {
            context.SaveChanges();
        }

        public void Remove(Model.Task task)
        {
            context.Task.Remove(task);
            context.SaveChanges();
        }

        public Model.Task GetTaskById(Model.Task task)
        {
            return context.Task.FirstOrDefault(p => p.idTask == task.idTask);
        }

        public IEnumerable<Model.Task> GetTasksById(Student student)
        {
            return context.Task.Where(p => p.idStudent == student.idStudent);
        }

        public void ChangeComplite(Model.Task task)
        {
            GetTaskById(task).isComplite = task.isComplite;
            context.SaveChanges();
        }

        public void RemoveById(Model.Task task)
        {
            context.Task.Remove(GetTaskById(task));
            context.SaveChanges();
        }

        public void RemoveByStudId(Student student)
        {
            foreach (Model.Task task in getTasks())
            {
                if (task.idStudent == student.idStudent)
                {
                    context.Task.Remove(task);
                }
            }
            context.SaveChanges();
        }

        public void OrderTasks(Student student, string subject)
        {
            context.Task.Where(p => p.idStudent == student.idStudent).OrderBy(p => p.LessonName == subject).Load();
        }

        public IEnumerable<Model.Task> getEnum(Student student, string subject)
        {
            return context.Task.Where(p => p.idStudent == student.idStudent && p.LessonName == subject);
        }
    }
}
