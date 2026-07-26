using System.Collections.Generic;
using FadTask.Models;
using FadTask.Repositories;

namespace FadTask.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public IEnumerable<TaskItem> GetTasks()
        {
            return _taskRepository.GetTasks();
        }

        public TaskItem CreateTask(string title, string? description)
        {
            return _taskRepository.CreateTask(title, description);
        }
    }
}
