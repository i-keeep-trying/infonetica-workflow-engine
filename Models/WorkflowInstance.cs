namespace WorkflowEngine.Models
{
    public class WorkflowInstance
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();       // Unique instance ID
        public string DefinitionId { get; set; } = string.Empty;          // Link to the workflow definition
        public string CurrentStateId { get; set; } = string.Empty;        // The ID of the current state
        public List<ActionHistory> History { get; set; } = new();         // Log of actions taken
    }

    public class ActionHistory
    {
        public string ActionId { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
