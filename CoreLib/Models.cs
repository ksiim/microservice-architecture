namespace CoreLib.Models
{
    public enum TaskPriority { LOW, MEDIUM, HIGH }
    public enum TaskStatus { TODO, IN_PROGRESS, DONE }

    public class Task
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ProjectId { get; set; }
        public TaskPriority Priority { get; set; }
        public TaskStatus Status { get; set; }
        public string AssigneeId { get; set; }
    }

    public class Project
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}