using System.ComponentModel.DataAnnotations;

namespace FadTask.Dtos
{
    public class TaskCreateRequest
    {
        [Required]
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
    }
}
