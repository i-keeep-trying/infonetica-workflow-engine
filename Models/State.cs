namespace WorkflowEngine.Models
{
    public class State
    {
        public string Id { get; set; } = string.Empty;          // Unique identifier for the state
        public string Name { get; set; } = string.Empty;        // Display name
        public bool IsInitial { get; set; }                     // Marks the starting state
        public bool IsFinal { get; set; }                       // Marks the ending state
        public bool Enabled { get; set; } = true;               // Can this state be used in transitions?
        public string? Description { get; set; }                // Optional extra info
    }
}
