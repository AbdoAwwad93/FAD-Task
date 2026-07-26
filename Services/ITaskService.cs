using System.Collections.Generic;
using FadTask.Models;

namespace FadTask.Services
{
    public interface ITaskService
    {
        IEnumerable<TaskItem> GetTasks();
        TaskItem CreateTask(string title, string? description);
    }
}
