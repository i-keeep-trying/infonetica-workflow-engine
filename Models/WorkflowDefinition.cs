using System.ComponentModel.DataAnnotations;

namespace WorkflowEngine.Models
{
    public class WorkflowDefinition
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();  // Unique ID for the workflow
        public string Name { get; set; } = string.Empty;              // Friendly name

        [Required]
        public List<State> States { get; set; } = new();              // All states in this workflow

        [Required]
        public List<ActionDefinition> Actions { get; set; } = new(); // All actions/transitions

        public string? Description { get; set; }                      // Optional notes

        // Basic validation helper: check if there's exactly one initial state
        public bool HasSingleInitialState()
        {
            return States.Count(s => s.IsInitial) == 1;
        }
    }
}
