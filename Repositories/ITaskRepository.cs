using System.Collections.Generic;
using FadTask.Models;

namespace FadTask.Repositories
{
    public interface ITaskRepository
    {
        IEnumerable<TaskItem> GetTasks();
        TaskItem CreateTask(string title, string? description);
    }
}
