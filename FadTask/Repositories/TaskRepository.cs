using System.Collections.Generic;
using FadTask.Models;

namespace FadTask.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly List<TaskItem> _tasks = new();
        private int _nextId = 1;

        public IEnumerable<TaskItem> GetTasks()
        {
            return _tasks;
        }

        public TaskItem CreateTask(string title, string? description)
        {
            var task = new TaskItem
            {
                Id = _nextId++,
                Title = title,
                Description = description,
                IsCompleted = false,
                CreatedAt = DateTime.UtcNow
            };

            _tasks.Add(task);
            return task;
        }
    }
}
