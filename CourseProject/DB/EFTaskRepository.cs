﻿using CourseProject.Model;
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
            context = new StudTasksEntities();                                  // Создаем контекст для работы с БД
        }

        public IEnumerable<Model.Task> getTasks()                               // Получение всех задач
        {
            context.Task.Distinct().OrderByDescending(p => p.DueDate).Load();   // Загружаем все задачи в контекст
            return context.Task;                                                // Возвращаем все задачи
        }

        public IEnumerable<Model.Subtask> getSubtasks()                         // Получение все подзадачи
        {
            context.Subtask.Distinct().Load();                                  // Загружаем все подзадачи в контекст
            return context.Subtask;                                             // Возвращаем все подзадачи
        }
        

        public void addTask(Model.Task task)            // добавление задачи
        {
            context.Task.Add(task);
            context.SaveChanges();
        }

        public void addSubtask(Subtask subtask)         // добавление подзадачи
        {
            context.Subtask.Add(subtask);
            context.SaveChanges();
        }

        public void SaveTask()                          // Сохранение изменений в БД
        {
            context.SaveChanges();
        }

        public void SaveSubtask()                      // Сохранение изменений подзадачи в БД
        {
            context.SaveChanges();
        }


        public void Update(Model.Task task)
        {
            context.Entry(task).State = EntityState.Modified;                      // Обновляем задачу
            context.SaveChanges();
        }

        public void Update(Model.Subtask subtask)
        {
            context.Entry(subtask).State = EntityState.Modified;                   // Обновляем задачу
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

        public Model.Task GetTaskById(Model.Task task)                          // Получение задачи по ее id
        {
            return context.Task.FirstOrDefault(p => p.idTask == task.idTask);
        }

        public IEnumerable<Model.Task> GetTasksById(Student student)    
        {
            return context.Task.Where(p => p.idStudent == student.idStudent);
        }

        public void ChangeComplite(Model.Task task)                             // Изменение статуса задачи
        {
            GetTaskById(task).isComplite = task.isComplite;
            context.SaveChanges();
        }

        public void RemoveById(Model.Task task)
        {
            context.Task.Remove(GetTaskById(task));
            context.SaveChanges();
        }

        public void RemoveByStudId(Student student)                             // Удаление задач по id студента
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

        public void OrderTasks(Student student, string subject)                 // Сортировка задач по предмету
        {
            context.Task.Where(p => p.idStudent == student.idStudent).OrderBy(p => p.LessonName == subject).Load();
        }

        public IEnumerable<Model.Task> getEnum(Student student, string subject) // Получение задач по предмету
        {
            return context.Task.Where(p => p.idStudent == student.idStudent && p.LessonName == subject);
        }
    }
}
