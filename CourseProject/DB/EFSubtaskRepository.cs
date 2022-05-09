using CourseProject.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseProject.DB
{
    class EFSubtaskRepository
    {
        private StudTasksEntities context;

        public EFSubtaskRepository()
        {
            context = new StudTasksEntities();                                  // Создаем контекст для работы с БД
        }

        public IEnumerable<Model.Subtask> getSubtasks()                         // Получение все подзадачи
        {
            context.Subtask.Distinct().Load();                                  // Загружаем все подзадачи в контекст
            return context.Subtask;                                             // Возвращаем все подзадачи
        }

        public void addSubtask(Subtask subtask)         // добавление подзадачи
        {
            context.Subtask.Add(subtask);
            context.SaveChanges();
        }

        public void SaveSubtask()                      // Сохранение изменений подзадачи в БД
        {
            context.SaveChanges();
        }

        public void Update(Model.Subtask subtask)
        {
            context.Entry(subtask).State = EntityState.Modified;    // Обновляем подзадачу
            context.SaveChanges();
        }

        public void UpdateAll()
        {
            context.SaveChanges();
        }

        public void Remove(Model.Subtask subtask)
        {
            context.Subtask.Remove(subtask);
            context.SaveChanges();
        }

        public Model.Subtask GetSubaskById(Model.Subtask subtask)                          // Получение задачи по ее id
        {
            return context.Subtask.FirstOrDefault(p => p.idSubtask == subtask.idSubtask);
        }


        public IEnumerable<Model.Subtask> GetSubtasksById(Model.Task task)
        { 
            return context.Subtask.Where(p => p.idTask == task.idTask);
        }

        public void RemoveById(Model.Subtask subtask)
        {
            context.Subtask.Remove(GetSubaskById(subtask));
            context.SaveChanges();
        }

        public void RemoveByStudId(Model.Task task)                             // Удаление задач по id студента
        {
            foreach (Model.Subtask subtask in getSubtasks())
            {
                if (subtask.idTask == task.idTask)
                {
                    context.Subtask.Remove(subtask);
                }
            }
            context.SaveChanges();
        }
    }
}
