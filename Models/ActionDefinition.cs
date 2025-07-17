namespace WorkflowEngine.Models
{
    public class ActionDefinition
    {
        public string Id { get; set; } = string.Empty;                // Unique action ID
        public string Name { get; set; } = string.Empty;              // Action name (e.g., "Submit", "Approve")
        public bool Enabled { get; set; } = true;                     // Is this action currently allowed?
        public List<string> FromStates { get; set; } = new();        // List of state IDs this can originate from
        public string ToState { get; set; } = string.Empty;           // ID of the target state
        public string? Description { get; set; }                      // Optional description
    }
}
